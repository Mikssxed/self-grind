<template>
    <BaseButton
        variant="secondary"
        class="flex flex-col items-center gap-2 px-4 py-4 bg-primary-900 text-sm rounded-2xl border"
        :class="borderClass"
    >
        <span class="text-2xl">{{ emoji }}</span>
        <span class="font-bold text-white">{{ label }}</span>
    </BaseButton>
</template>
<script setup lang="ts">
    import BaseButton from '@/components/base/BaseButton.vue';
    import { computed } from 'vue';

    export type ActionCardBorderVariant = 'accent' | 'info' | 'violet' | 'warning';

    interface DashboardActionCardProps {
        emoji: string;
        label: string;
        borderVariant?: ActionCardBorderVariant;
    }

    const props = withDefaults(defineProps<DashboardActionCardProps>(), {
        borderVariant: 'accent',
    });

    const borderClasses: Record<ActionCardBorderVariant, string> = {
        accent: 'border-accent-700/30',
        info: 'border-info-500/30',
        violet: 'border-violet-500/30',
        warning: 'border-warning-500/30',
    };

    const borderClass = computed(() => borderClasses[props.borderVariant]);
</script>
