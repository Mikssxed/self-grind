// Google Analytics 4 visitor tracking.
//
// The measurement ID is injected at build time via VITE_GA_MEASUREMENT_ID (see Dockerfile build arg).
// When the variable is absent (local dev) every export below is a no-op, so analytics never runs
// in development and never throws.

type GtagFn = (...args: unknown[]) => void;

declare global {
    interface Window {
        dataLayer?: unknown[];
        gtag?: GtagFn;
    }
}

const measurementId = import.meta.env.VITE_GA_MEASUREMENT_ID as string | undefined;

let initialized = false;

/**
 * Loads gtag.js and configures GA4. Safe to call once at app startup.
 * SPA navigations are tracked manually via trackPageView (the initial page_view is disabled here
 * so the router hook does not double-count the first load).
 */
export function initTracking(): void {
    if (initialized || !measurementId || typeof window === 'undefined') return;
    initialized = true;

    const script = document.createElement('script');
    script.async = true;
    script.src = `https://www.googletagmanager.com/gtag/js?id=${measurementId}`;
    document.head.appendChild(script);

    window.dataLayer = window.dataLayer || [];
    window.gtag = function gtag(...args: unknown[]) {
        window.dataLayer!.push(args);
    };
    window.gtag('js', new Date());
    window.gtag('config', measurementId, { send_page_view: false });
}

/** Reports a single-page-app navigation as a page_view event. */
export function trackPageView(path: string): void {
    if (!measurementId || typeof window === 'undefined' || !window.gtag) return;
    window.gtag('event', 'page_view', {
        page_path: path,
        page_location: window.location.origin + path,
    });
}
