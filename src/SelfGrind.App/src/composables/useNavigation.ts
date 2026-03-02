import type { IconName } from '@/components/icons';
import type { AppRouteNames } from '@/router';
import { useRoute } from 'vue-router';

type NavigationItem = {
    name: AppRouteNames;
    label: string;
    icon: IconName;
    isActive?: boolean;
};

const isRouteActive = (routeName: AppRouteNames): boolean => {
    const route = useRoute();
    return route.name === routeName;
};
const enhanceNavigationItem = (i: NavigationItem) => ({ ...i, isActive: isRouteActive(i.name) });

const navigationList: NavigationItem[] = [
    {
        name: 'dashboard',
        label: 'Dashboard',
        icon: 'house',
    },
];

export default function () {
    return {
        currentNavigationItem: navigationList.filter((i) => i.isActive),
        navigationList: navigationList.map(enhanceNavigationItem),
    };
}
