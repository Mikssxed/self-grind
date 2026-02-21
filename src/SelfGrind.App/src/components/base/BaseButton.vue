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

    interface BaseButtonProps {
        variant?: 'primary' | 'secondary';
        disabled?: boolean;
    }

    const props = withDefaults(defineProps<BaseButtonProps>(), {
        variant: 'primary',
        disabled: false,
    });

    const classes = computed(() =>
        twMerge(
            'rounded-xl px-4 py-4 text-lg/7 font-bold hover:opacity-60 transition-opacity duration-300 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed',
            props.variant === 'primary' && 'button-primary'
        )
    );
</script>
