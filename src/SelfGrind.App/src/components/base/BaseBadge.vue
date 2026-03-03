<template>
    <span :class="classes">
        <slot />
    </span>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    type BadgeVariant = 'gradient' | 'accent';
    type BadgeSize = 'small' | 'medium' | 'large';

    interface BaseBadgeProps {
        variant?: BadgeVariant;
        size?: BadgeSize;
    }

    const props = withDefaults(defineProps<BaseBadgeProps>(), {
        variant: 'gradient',
        size: 'small',
    });

    const variantClasses: Record<BadgeVariant, string> = {
        gradient: 'bg-gradient-purple',
        accent: 'bg-orange-500',
    };

    const sizeClasses: Record<BadgeSize, string> = {
        small: 'text-xs px-3 py-1',
        medium: 'text-sm px-4 py-1.5',
        large: 'text-base px-5 py-2',
    };

    const classes = computed(() =>
        twMerge(
            'text-white font-bold rounded-full',
            sizeClasses[props.size],
            variantClasses[props.variant]
        )
    );
</script>
