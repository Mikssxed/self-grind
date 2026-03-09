<template>
    <div
        class="flex items-center gap-4 p-4 rounded-2xl border transition-all duration-300"
        :class="containerClass"
    >
        <div
            class="flex items-center justify-center w-10 h-10 rounded-xl text-xl"
            :class="iconBgClass"
        >
            {{ emoji }}
        </div>
        <div class="flex flex-col flex-1">
            <span class="text-sm font-bold text-white">{{ name }}</span>
            <span class="text-xs text-primary-400">{{ levelRange }}</span>
        </div>
        <span class="text-lg">{{ statusIcon }}</span>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type TierStatus = 'completed' | 'current' | 'locked';

    export interface EvolutionTier {
        name: string;
        levelRange: string;
        status: TierStatus;
        emoji: string;
    }

    interface CharacterEvolutionTierItemProps {
        name: string;
        levelRange: string;
        status: TierStatus;
        emoji: string;
    }

    const props = defineProps<CharacterEvolutionTierItemProps>();

    const containerClasses: Record<TierStatus, string> = {
        completed: 'bg-success-500/20 border-success-500/30',
        current: 'bg-accent-500/20 border-accent-500/30',
        locked: 'bg-primary-900 border-primary-800 opacity-50',
    };

    const iconBgClasses: Record<TierStatus, string> = {
        completed: 'bg-success-500/30',
        current: 'bg-accent-500/30',
        locked: 'bg-primary-800',
    };

    const statusIcons: Record<TierStatus, string> = {
        completed: '✅',
        current: '⭐',
        locked: '🔒',
    };

    const containerClass = computed(() => containerClasses[props.status]);
    const iconBgClass = computed(() => iconBgClasses[props.status]);
    const statusIcon = computed(() => statusIcons[props.status]);
</script>
