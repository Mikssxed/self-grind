<template>
    <button
        :class="classes"
        :disabled="disabled"
    >
        <slot />
    </button>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    export type ButtonSize = 'sm' | 'md';

    interface BaseButtonProps {
        variant?: 'primary' | 'secondary';
        size?: ButtonSize;
        disabled?: boolean;
    }

    const props = withDefaults(defineProps<BaseButtonProps>(), {
        variant: 'primary',
        size: 'md',
        disabled: false,
    });

    const sizeClasses: Record<ButtonSize, string> = {
        sm: 'px-3 py-2 text-sm rounded-lg',
        md: 'px-4 py-4 text-lg/7 rounded-xl',
    };

    const classes = computed(() =>
        twMerge(
            'font-bold hover:opacity-60 transition-opacity duration-300 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed',
            sizeClasses[props.size],
            props.variant === 'primary' && 'button-primary'
        )
    );
</script>
