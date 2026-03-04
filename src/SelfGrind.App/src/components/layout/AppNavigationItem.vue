<template>
    <RouterLink
        :to="{ name: name }"
        :class="containerClasses"
    >
        <BaseIcon
            :name="iconName"
            class="w-4 h-4"
        />
        <BaseText :class="textClasses">{{ label }}</BaseText>
    </RouterLink>
</template>
<script setup lang="ts">
    import type { AppRouteNames } from '@/router';
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';
    import BaseIcon from '../base/BaseIcon.vue';
    import BaseText from '../base/BaseText.vue';
    import type { IconName } from '../icons';

    interface AppNavigationItemProps {
        name: AppRouteNames;
        label: string;
        iconName: IconName;
        isActive?: boolean;
    }

    const props = withDefaults(defineProps<AppNavigationItemProps>(), {
        isActive: false,
    });

    const containerClasses = computed(() => {
        return twMerge(
            'p-3 flex gap-3 items-center rounded-lg cursor-pointer transition-colors duration-300 capitalize',
            props.isActive ? 'bg-gradient-purple' : 'hover:bg-primary-900'
        );
    });

    const textClasses = computed(() => {
        return twMerge(props.isActive && 'text-white');
    });
</script>
