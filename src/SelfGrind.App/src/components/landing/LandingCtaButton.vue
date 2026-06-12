<template>
    <RouterLink
        :to="{ name: to }"
        :class="classes"
    >
        <slot />
    </RouterLink>
</template>
<script setup lang="ts">
    import type { AppRouteNames } from '@/router';
    import { computed } from 'vue';

    export type CtaVariant = 'primary' | 'secondary';
    export type CtaSize = 'md' | 'lg';

    interface LandingCtaButtonProps {
        to: AppRouteNames;
        variant?: CtaVariant;
        size?: CtaSize;
    }

    const props = withDefaults(defineProps<LandingCtaButtonProps>(), {
        variant: 'primary',
        size: 'md',
    });

    const variantClasses: Record<CtaVariant, string> = {
        primary: 'button-primary text-white shadow-lg shadow-secondary-700/30 hover:opacity-90',
        secondary:
            'border border-primary-800 bg-primary-900/60 text-white hover:border-accent-500/60',
    };

    const sizeClasses: Record<CtaSize, string> = {
        md: 'px-5 py-2.5 text-sm rounded-lg',
        lg: 'px-7 py-3.5 text-base rounded-xl',
    };

    const classes = computed(
        () =>
            `inline-flex items-center justify-center gap-2 font-bold transition-all duration-300 outline-none focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-accent-500 ${variantClasses[props.variant]} ${sizeClasses[props.size]}`
    );
</script>
