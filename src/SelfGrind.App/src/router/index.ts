import { refreshTokensOnce } from '@/api/tokenRefresh';
import { useAuthStore } from '@/stores/useAuthStore';
import { LayoutType } from '@/types';
import { createRouter, createWebHistory } from 'vue-router';

// Refresh the access token proactively when it expires within this window
const REFRESH_BUFFER_MS = 60_000;

export type AppRouteNames = keyof typeof AppViews;

export const AppViews = {
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
        { path: '/', redirect: { name: 'dashboard' } },
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
    const accountPage = ['register', 'login'].includes(to.name as string);
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

export default router;
