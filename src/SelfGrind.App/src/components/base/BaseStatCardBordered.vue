<template>
    <div
        class="rounded-2xl p-4 border-l-4 bg-gradient-midnight"
        :class="borderClass"
    >
        <span class="text-xs text-primary-400">{{ label }}</span>
        <p class="text-2xl font-bold text-white mt-1">
            {{ value }}
            <span
                v-if="unit"
                class="text-sm font-normal text-primary-400"
            >
                {{ unit }}
            </span>
        </p>
        <span
            v-if="subtitle"
            class="text-xs mt-1"
            :class="subtitleClass"
        >
            {{ subtitleEmoji }} {{ subtitle }}
        </span>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type StatBorderVariant = 'info' | 'accent' | 'success' | 'violet';

    export interface BorderedStat {
        label: string;
        value: string;
        unit?: string;
        subtitle?: string;
        subtitleEmoji?: string;
        borderVariant: StatBorderVariant;
    }

    interface BaseStatCardBorderedProps {
        label: string;
        value: string;
        unit?: string;
        subtitle?: string;
        subtitleEmoji?: string;
        borderVariant: StatBorderVariant;
    }

    const props = defineProps<BaseStatCardBorderedProps>();

    const borderClasses: Record<StatBorderVariant, string> = {
        info: 'border-l-info-500',
        accent: 'border-l-accent-500',
        success: 'border-l-success-500',
        violet: 'border-l-violet-500',
    };

    const subtitleClasses: Record<StatBorderVariant, string> = {
        info: 'text-info-500',
        accent: 'text-accent-500',
        success: 'text-success-500',
        violet: 'text-violet-500',
    };

    const borderClass = computed(() => borderClasses[props.borderVariant]);
    const subtitleClass = computed(() => subtitleClasses[props.borderVariant]);
</script>
