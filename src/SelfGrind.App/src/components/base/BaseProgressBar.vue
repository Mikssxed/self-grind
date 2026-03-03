<template>
    <div class="w-full h-2 rounded-full bg-primary-800">
        <div
            class="h-full rounded-full transition-all duration-500"
            :class="variantBarClass"
            :style="{ width: `${percentage}%` }"
        ></div>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type ProgressBarVariant = 'gradient' | 'error' | 'info' | 'violet' | 'warning' | 'success';

    interface BaseProgressBarProps {
        percentage: number;
        variant?: ProgressBarVariant;
    }

    const props = withDefaults(defineProps<BaseProgressBarProps>(), {
        variant: 'gradient',
    });

    const variantClasses: Record<ProgressBarVariant, string> = {
        gradient: 'bg-gradient-purple',
        error: 'bg-error-500',
        info: 'bg-info-500',
        violet: 'bg-violet-500',
        warning: 'bg-warning-500',
        success: 'bg-success-500',
    };

    const variantBarClass = computed(() => variantClasses[props.variant]);
</script>
