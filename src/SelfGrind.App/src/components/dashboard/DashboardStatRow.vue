<template>
    <div class="flex flex-col gap-1">
        <div class="flex justify-between items-center">
            <span class="text-sm text-primary-400">{{ emoji }} {{ label }}</span>
            <span
                class="text-sm font-bold"
                :class="textClass"
            >{{ value }}/100</span>
        </div>
        <BaseProgressBar
            :percentage="value"
            :variant="variant"
        />
    </div>
</template>
<script setup lang="ts">
    import BaseProgressBar from '@/components/base/BaseProgressBar.vue';
    import type { ProgressBarVariant } from '@/components/base/BaseProgressBar.vue';
    import { computed } from 'vue';

    interface DashboardStatRowProps {
        label: string;
        emoji: string;
        value: number;
        variant: ProgressBarVariant;
    }

    const props = defineProps<DashboardStatRowProps>();

    const variantTextClasses: Record<ProgressBarVariant, string> = {
        gradient: 'text-accent-500',
        error: 'text-error-500',
        info: 'text-info-500',
        violet: 'text-violet-500',
        warning: 'text-warning-500',
        success: 'text-success-500',
    };

    const textClass = computed(() => variantTextClasses[props.variant]);
</script>
