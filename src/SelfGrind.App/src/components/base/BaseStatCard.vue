<template>
    <div
        class="rounded-2xl p-4"
        :class="variantClass"
    >
        <span class="text-xs text-primary-400">{{ emoji }} {{ label }}</span>
        <p class="text-2xl font-bold text-white mt-1">{{ value }}</p>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type StatCardVariant = 'default' | 'info' | 'violet';

    interface BaseStatCardProps {
        emoji: string;
        label: string;
        value: string;
        variant?: StatCardVariant;
    }

    const props = withDefaults(defineProps<BaseStatCardProps>(), {
        variant: 'default',
    });

    const variantClasses: Record<StatCardVariant, string> = {
        default: 'bg-primary-900',
        info: 'bg-info-500/30',
        violet: 'bg-violet-500/30',
    };

    const variantClass = computed(() => variantClasses[props.variant]);
</script>
