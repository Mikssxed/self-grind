<script setup lang="ts">
    import BaseIcon from '@/components/base/BaseIcon.vue';
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';
    import type { IconName } from './icons';

    export interface GradientIconProps {
        name: IconName;
        size?: 'small' | 'medium' | 'large';
    }

    const props = withDefaults(defineProps<GradientIconProps>(), {
        size: 'medium',
    });

    const sizeClasses = {
        container: {
            small: 'w-16 h-16',
            medium: 'w-20 h-20',
            large: 'w-24 h-24',
        },
        icon: {
            small: 'w-8 h-8',
            medium: 'w-10 h-10',
            large: 'w-12 h-12',
        },
    };

    const containerClasses = computed(() => {
        return twMerge(
            'bg-gradient-purple rounded-3xl flex justify-center items-center',
            sizeClasses.container[props.size]
        );
    });

    const iconSizeClass = computed(() => {
        return sizeClasses.icon[props.size];
    });
</script>

<template>
    <div :class="containerClasses">
        <BaseIcon
            :name="name"
            :class="iconSizeClass"
        />
    </div>
</template>
