import { refreshTokensOnce } from '@/api/tokenRefresh';
import { trackPageView } from '@/composables/useTracking';
import { useAuthStore } from '@/stores/useAuthStore';
import { LayoutType } from '@/types';
import { createRouter, createWebHistory } from 'vue-router';

// Refresh the access token proactively when it expires within this window
const REFRESH_BUFFER_MS = 60_000;

export type AppRouteNames = keyof typeof AppViews;

export const AppViews = {
    landing: () => import('@/views/LandingView.vue'),
    dashboard: () => import('@/views/DashboardView.vue'),
    dailyTasks: () => import('@/views/DailyTasksView.vue'),
    tasks: () => import('@/views/AllTasksView.vue'),
    contributionGrid: () => import('@/views/ContributionGridView.vue'),
    character: () => import('@/views/CharacterView.vue'),
    analytics: () => import('@/views/AnalyticsView.vue'),
    community: () => import('@/views/CommunityView.vue'),
    register: () => import('@/views/auth/RegisterView.vue'),
    login: () => import('@/views/auth/LoginView.vue'),
};

const createRoute = (
    path: string,
    name: AppRouteNames,
    layoutType: LayoutType,
    requiresAuth = true,
    props = false
) => {
    return {
        path,
        name,
        component: AppViews[name],
        props,
        meta: {
            requiresAuth,
            layoutType,
        },
    };
};

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        createRoute('/', 'landing', LayoutType.WITHOUT_SIDEBAR, false),
        createRoute('/dashboard', 'dashboard', LayoutType.WITH_SIDEBAR, true),
        createRoute('/daily-tasks', 'dailyTasks', LayoutType.WITH_SIDEBAR, true),
        createRoute('/tasks', 'tasks', LayoutType.WITH_SIDEBAR, true),
        createRoute('/contribution-grid', 'contributionGrid', LayoutType.WITH_SIDEBAR, true),
        createRoute('/character', 'character', LayoutType.WITH_SIDEBAR, true),
        createRoute('/analytics', 'analytics', LayoutType.WITH_SIDEBAR, true),
        createRoute('/community', 'community', LayoutType.WITH_SIDEBAR, true),
        createRoute('/register', 'register', LayoutType.WITHOUT_SIDEBAR, false),
        createRoute('/login', 'login', LayoutType.WITHOUT_SIDEBAR, false),
    ],
});

router.beforeEach(async (to, _from, next) => {
    const authStore = useAuthStore();
    const requiresAuth =
        import.meta.env.VITE_DISABLE_AUTH !== 'true' && to.meta.requiresAuth !== false;
    // Public pages that authenticated users should be bounced away from, straight to their dashboard
    const accountPage = ['register', 'login', 'landing'].includes(to.name as string);
    const isNoNamePage = to.name === undefined;
    const isAuthenticated = authStore.isAuthenticated;
    if (requiresAuth) {
        // needs authentication - if not authenticated, redirect to login
        if (isAuthenticated) {
            // Refresh ahead of expiry so the first request already carries a fresh token.
            // On failure refreshTokensOnce resolves null and the reactive 401 path takes over.
            if (authStore.tokenData?.refreshToken && !authStore.isAccessTokenFresh(REFRESH_BUFFER_MS)) {
                await refreshTokensOnce();
            }
            next();
        } else {
            next({ name: 'login', query: { returnUrl: to.fullPath } });
        }
    } else if (accountPage || isNoNamePage) {
        // pages for unauthenticated users - if authenticated, redirect to dashboard
        if (isAuthenticated) {
            next({ name: 'dashboard' });
        } else {
            next();
        }
    } else {
        // public page - no auth required
        next();
    }
});

// The landing page keeps the keyword-rich title from index.html; app routes get a "Page | SelfGrind" title
const LANDING_TITLE = 'SelfGrind | Gamified Task Manager — Turn Tasks into XP';

const routeTitles: Partial<Record<AppRouteNames, string>> = {
    dashboard: 'Dashboard',
    dailyTasks: 'Daily Tasks',
    tasks: 'All Tasks',
    contributionGrid: 'Contribution Grid',
    character: 'Character',
    analytics: 'Analytics',
    community: 'Community',
    register: 'Create your free account',
    login: 'Sign in',
};

const resolveDocumentTitle = (routeName: unknown): string => {
    if (typeof routeName !== 'string') return LANDING_TITLE;
    const title = routeTitles[routeName as AppRouteNames];
    return title ? `${title} | SelfGrind` : LANDING_TITLE;
};

// Update the document title, then report the navigation to GA4 (gtag enhanced measurement does not
// reliably catch SPA route changes; the title must be set first so the page_view picks it up).
router.afterEach((to) => {
    document.title = resolveDocumentTitle(to.name);
    trackPageView(to.fullPath);
});

export default router;
