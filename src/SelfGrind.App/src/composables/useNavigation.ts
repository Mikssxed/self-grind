import type { IconName } from '@/components/icons';
import type { AppRouteNames } from '@/router';
import { computed } from 'vue';
import { useRoute } from 'vue-router';

export type NavigationItem = {
    name: AppRouteNames;
    label: string;
    icon: IconName;
    isActive: boolean;
};

const navigationItems: Omit<NavigationItem, 'isActive'>[] = [
    {
        name: 'dashboard',
        label: 'Dashboard',
        icon: 'house',
    },
    {
        name: 'dailyTasks',
        label: 'Tasks & Habits',
        icon: 'dailyTasks',
    },
    {
        name: 'contributionGrid',
        label: 'Contribution Grid',
        icon: 'contributionGrid',
    },
    {
        name: 'character',
        label: 'Character',
        icon: 'character',
    },
    {
        name: 'analytics',
        label: 'Analytics',
        icon: 'analytics',
    },
];

export default function () {
    const route = useRoute();

    const navigationList = computed<NavigationItem[]>(() =>
        navigationItems.map((item) => ({
            ...item,
            isActive: route.name === item.name,
        }))
    );

    const currentNavigationItem = computed(() =>
        navigationList.value.filter((i) => i.isActive)
    );

    return {
        currentNavigationItem,
        navigationList,
    };
}
