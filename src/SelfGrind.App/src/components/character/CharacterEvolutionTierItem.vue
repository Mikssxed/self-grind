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
    import type { EvolutionTierStatus } from '@/api/apiClient/models';
    import { EvolutionTierStatusObject } from '@/api/apiClient/models';

    interface CharacterEvolutionTierItemProps {
        name: string;
        levelRange: string;
        status: EvolutionTierStatus;
        emoji: string;
    }

    const props = defineProps<CharacterEvolutionTierItemProps>();

    const containerClasses: Record<EvolutionTierStatus, string> = {
        [EvolutionTierStatusObject.Completed]: 'bg-success-500/20 border-success-500/30',
        [EvolutionTierStatusObject.Current]: 'bg-accent-500/20 border-accent-500/30',
        [EvolutionTierStatusObject.Locked]: 'bg-primary-900 border-primary-800 opacity-50',
    };

    const iconBgClasses: Record<EvolutionTierStatus, string> = {
        [EvolutionTierStatusObject.Completed]: 'bg-success-500/30',
        [EvolutionTierStatusObject.Current]: 'bg-accent-500/30',
        [EvolutionTierStatusObject.Locked]: 'bg-primary-800',
    };

    const statusIcons: Record<EvolutionTierStatus, string> = {
        [EvolutionTierStatusObject.Completed]: '✅',
        [EvolutionTierStatusObject.Current]: '⭐',
        [EvolutionTierStatusObject.Locked]: '🔒',
    };

    const containerClass = computed(() => containerClasses[props.status]);
    const iconBgClass = computed(() => iconBgClasses[props.status]);
    const statusIcon = computed(() => statusIcons[props.status]);
</script>
