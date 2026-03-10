<template>
    <div
        class="flex flex-col items-center gap-2 p-4 rounded-2xl"
        :class="[variantClass, { 'opacity-50 grayscale': locked }]"
    >
        <span class="text-3xl">{{ emoji }}</span>
        <span class="text-xs font-bold text-white text-center">{{ label }}</span>
        <span
            v-if="subtitle"
            class="text-xs text-primary-400 text-center"
        >
            {{ subtitle }}
        </span>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type AchievementCardVariant = 'orange' | 'blue' | 'green' | 'purple' | 'crimson';

    interface BaseAchievementCardProps {
        emoji: string;
        label: string;
        variant: AchievementCardVariant;
        subtitle?: string;
        locked?: boolean;
    }

    const props = withDefaults(defineProps<BaseAchievementCardProps>(), {
        locked: false,
    });

    const variantClasses: Record<AchievementCardVariant, string> = {
        orange: 'bg-orange-900/24',
        blue: 'bg-blue-900/24',
        green: 'bg-green-900/24',
        purple: 'bg-purple-900/24',
        crimson: 'bg-crimson-900/24',
    };

    const variantClass = computed(() => variantClasses[props.variant]);
</script>
