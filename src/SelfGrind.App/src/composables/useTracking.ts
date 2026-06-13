// Google Analytics 4 visitor tracking with Consent Mode v2.
//
// The measurement ID is injected at build time via VITE_GA_MEASUREMENT_ID (see Dockerfile build arg).
// When the variable is absent (local dev) every gtag export below is a no-op, so analytics never runs
// in development and never throws. Consent state is still persisted to localStorage so the banner can
// be tested locally.
//
// Consent flow:
//   - Defaults are set to "denied" for all storage BEFORE gtag.js loads (Consent Mode v2 requirement).
//     While denied, GA4 sends cookieless, modeled pings rather than nothing.
//   - EEA visitors with no stored choice keep "denied" until they accept via the banner.
//   - Visitors outside the EEA (no banner shown) are granted implicitly on load.
//   - Accepting/rejecting calls gtag('consent','update',...) and persists the choice.

type GtagFn = (...args: unknown[]) => void;

declare global {
    interface Window {
        dataLayer?: unknown[];
        gtag?: GtagFn;
    }
}

const measurementId = import.meta.env.VITE_GA_MEASUREMENT_ID as string | undefined;

const CONSENT_STORAGE_KEY = 'selfgrind_analytics_consent';
type ConsentChoice = 'granted' | 'denied';

let initialized = false;

function getStoredConsent(): ConsentChoice | null {
    if (typeof window === 'undefined') return null;
    const value = window.localStorage.getItem(CONSENT_STORAGE_KEY);
    return value === 'granted' || value === 'denied' ? value : null;
}

function setStoredConsent(choice: ConsentChoice): void {
    if (typeof window === 'undefined') return;
    window.localStorage.setItem(CONSENT_STORAGE_KEY, choice);
}

/**
 * Heuristic EEA/Europe detection from the browser time zone. Intentionally over-inclusive (any
 * `Europe/*` zone) so the banner is shown rather than skipped when in doubt — precise geo would need
 * a server-side IP lookup.
 */
function isLikelyEuUser(): boolean {
    try {
        const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone ?? '';
        return timeZone.startsWith('Europe/');
    } catch {
        return true;
    }
}

/** The banner is shown only to likely-EEA visitors who have not yet made a choice. */
export function shouldShowConsentBanner(): boolean {
    return getStoredConsent() === null && isLikelyEuUser();
}

/** Resolves the consent state to apply on load before any banner interaction. */
function resolveInitialConsent(): ConsentChoice {
    const stored = getStoredConsent();
    if (stored) return stored;
    // No prior choice: EEA visitors must opt in (stay denied until the banner is used);
    // everyone else is granted implicitly.
    return isLikelyEuUser() ? 'denied' : 'granted';
}

/**
 * Loads gtag.js and configures GA4 with Consent Mode v2. Safe to call once at app startup.
 * SPA navigations are tracked manually via trackPageView (the initial page_view is disabled here
 * so the router hook does not double-count the first load).
 */
export function initTracking(): void {
    if (initialized || !measurementId || typeof window === 'undefined') return;
    initialized = true;

    window.dataLayer = window.dataLayer || [];
    window.gtag = function gtag(...args: unknown[]) {
        window.dataLayer!.push(args);
    };

    // Consent Mode v2 defaults — must be queued before gtag.js loads and before `config`.
    window.gtag('consent', 'default', {
        ad_storage: 'denied',
        ad_user_data: 'denied',
        ad_personalization: 'denied',
        analytics_storage: 'denied',
        wait_for_update: 500,
    });

    // Apply a prior decision (or implied consent outside the EEA) immediately.
    if (resolveInitialConsent() === 'granted') {
        window.gtag('consent', 'update', { analytics_storage: 'granted' });
    }

    const script = document.createElement('script');
    script.async = true;
    script.src = `https://www.googletagmanager.com/gtag/js?id=${measurementId}`;
    document.head.appendChild(script);

    window.gtag('js', new Date());
    window.gtag('config', measurementId, { send_page_view: false });
}

/**
 * Records the visitor's analytics-consent choice: persists it so the banner does not reappear and
 * updates Consent Mode. A no-op for the gtag call when analytics is disabled (local dev).
 */
export function updateAnalyticsConsent(granted: boolean): void {
    const choice: ConsentChoice = granted ? 'granted' : 'denied';
    setStoredConsent(choice);
    if (!measurementId || typeof window === 'undefined' || !window.gtag) return;
    window.gtag('consent', 'update', { analytics_storage: choice });
}

/** Reports a single-page-app navigation as a page_view event. */
export function trackPageView(path: string): void {
    if (!measurementId || typeof window === 'undefined' || !window.gtag) return;
    window.gtag('event', 'page_view', {
        page_path: path,
        page_location: window.location.origin + path,
    });
}
