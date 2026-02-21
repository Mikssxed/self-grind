import { useAuthStore } from '@/stores/useAuthStore';
import { createRouter, createWebHistory } from 'vue-router';

export type AppRouteNames = keyof typeof AppViews;

export const AppViews = {
    home: () => import('@/views/HomeView.vue'),
    register: () => import('@/views/auth/RegisterView.vue'),
    login: () => import('@/views/auth/LoginView.vue'),
};

const createRoute = (path: string, name: AppRouteNames, requiresAuth = true, props = false) => {
    return {
        path,
        name,
        component: AppViews[name],
        props,
        meta: {
            requiresAuth,
        },
    };
};

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        createRoute('/', 'home', true),
        createRoute('/register', 'register', false),
        createRoute('/login', 'login', false),
    ],
});

router.beforeEach((to, _from, next) => {
    const authStore = useAuthStore();
    const requiresAuth = to.meta.requiresAuth !== false;
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
        // pages for unauthenticated users - if authenticated, redirect to home
        if (isAuthenticated) {
            next({ name: 'home' });
        } else {
            next();
        }
    } else {
        // public page - no auth required
        next();
    }
});

export default router;
