import { createRouter, createWebHistory } from "vue-router";

export type AppRouteNames = keyof typeof AppViews;

export const AppViews = {
  home: () => import("@/views/HomeView.vue"),
  register: () => import("@/views/auth/RegisterView.vue"),
};

const createRoute = (
  path: string,
  name: AppRouteNames,
  // permissions: ApplicationUserPermission[] = [],
  // forceAuth = false,
  props = false,
) => {
  return {
    path,
    name,
    component: AppViews[name],
    props,
    // meta: {
    //     permissions,
    //     requiresAuth: permissions.length > 0 || forceAuth,
    // },
  };
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [createRoute("/", "home"), createRoute("/register", "register")],
});

export default router;
