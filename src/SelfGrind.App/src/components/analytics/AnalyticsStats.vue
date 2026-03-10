<template>
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <BaseStatCardBordered
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
    import BaseStatCardBordered from '@/components/base/BaseStatCardBordered.vue';
    import type { BorderedStat } from '@/components/base/BaseStatCardBordered.vue';

    interface AnalyticsStatsProps {
        stats: BorderedStat[];
    }

    defineProps<AnalyticsStatsProps>();
</script>
