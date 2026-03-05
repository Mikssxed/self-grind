import { useAuthStore } from '@/stores/useAuthStore';
import { LayoutType } from '@/types';
import { createRouter, createWebHistory } from 'vue-router';

export type AppRouteNames = keyof typeof AppViews;

export const AppViews = {
    dashboard: () => import('@/views/DashboardView.vue'),
    dailyTasks: () => import('@/views/DailyTasksView.vue'),
    contributionGrid: () => import('@/views/ContributionGridView.vue'),
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
        createRoute('/dashboard', 'dashboard', LayoutType.WITH_SIDEBAR, true),
        createRoute('/daily-tasks', 'dailyTasks', LayoutType.WITH_SIDEBAR, true),
        createRoute('/contribution-grid', 'contributionGrid', LayoutType.WITH_SIDEBAR, true),
        createRoute('/register', 'register', LayoutType.WITHOUT_SIDEBAR, false),
        createRoute('/login', 'login', LayoutType.WITHOUT_SIDEBAR, false),
    ],
});

router.beforeEach((to, _from, next) => {
    const authStore = useAuthStore();
    const requiresAuth =
        import.meta.env.VITE_DISABLE_AUTH !== 'true' && to.meta.requiresAuth !== false;
    const accountPage = ['register', 'login'].includes(to.name as string);
    const isAuthenticated = authStore.isAuthenticated;
    if (requiresAuth) {
        // needs authentication - if not authenticated, redirect to login
        if (isAuthenticated) {
            next();
        } else {
            next({ name: 'login', query: { returnUrl: to.fullPath } });
        }
    } else if (accountPage) {
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
