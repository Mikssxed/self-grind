<template>
    <BaseBox>
        <BaseHeader tag="h3">Character Stats</BaseHeader>
        <div class="flex flex-col gap-4">
            <DashboardStatRow
                v-for="stat in stats"
                :key="stat.label"
                :label="stat.label"
                :emoji="stat.emoji"
                :value="stat.value"
                :variant="stat.variant"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import type { ProgressBarVariant } from '@/components/base/BaseProgressBar.vue';
    import DashboardStatRow from './DashboardStatRow.vue';

    export interface CharacterStat {
        label: string;
        emoji: string;
        value: number;
        variant: ProgressBarVariant;
    }

    interface DashboardCharacterStatsProps {
        stats: CharacterStat[];
    }

    defineProps<DashboardCharacterStatsProps>();
</script>
