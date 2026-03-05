<template>
    <div class="grid grid-cols-4 gap-4">
        <ContributionStatCard
            v-for="stat in stats"
            :key="stat.label"
            :label="stat.label"
            :value="stat.value"
            :unit="stat.unit"
            :subtitle="stat.subtitle"
            :subtitle-emoji="stat.subtitleEmoji"
            :border-variant="stat.borderVariant"
        />
    </div>
</template>
<script setup lang="ts">
    import ContributionStatCard from './ContributionStatCard.vue';
    import type { ContributionStat } from './ContributionStatCard.vue';

    interface ContributionStatsProps {
        stats: ContributionStat[];
    }

    defineProps<ContributionStatsProps>();
</script>
