<template>
    <BaseBox>
        <BaseHeader tag="h3">XP Earned Per Week</BaseHeader>
        <BaseChart
            v-if="hasData"
            type="bar"
            :data="chartData"
            :options="chartOptions"
            height="lg"
        />
        <BaseEmptyState
            v-else
            emoji="⚡"
            title="No XP earned yet"
            message="Complete tasks to start building your weekly XP."
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseChart from '@/components/base/BaseChart.vue';
    import { hasChartData } from '@/composables/useChartEmpty';
    import type { ChartData, ChartOptions } from 'chart.js';

    interface AnalyticsXpPerWeekProps {
        data: ChartData<'bar'>;
    }

    const props = defineProps<AnalyticsXpPerWeekProps>();

    const chartData = computed(() => props.data);
    const hasData = computed(() => hasChartData(props.data));

    const chartOptions: ChartOptions<'bar'> = {
        plugins: {
            legend: {
                display: false,
            },
        },
    };
</script>
