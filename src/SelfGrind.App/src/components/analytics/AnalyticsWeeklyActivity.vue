<template>
    <BaseBox>
        <BaseHeader tag="h3">Weekly Activity by Category</BaseHeader>
        <BaseChart
            v-if="hasData"
            type="bar"
            :data="chartData"
            :options="chartOptions"
            height="lg"
        />
        <BaseEmptyState
            v-else
            emoji="📅"
            title="No activity this week"
            message="Complete a few tasks and your weekly chart will fill in."
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

    interface AnalyticsWeeklyActivityProps {
        data: ChartData<'bar'>;
    }

    const props = defineProps<AnalyticsWeeklyActivityProps>();

    const chartData = computed(() => props.data);
    const hasData = computed(() => hasChartData(props.data));

    const chartOptions: ChartOptions<'bar'> = {
        scales: {
            x: {
                stacked: true,
                ticks: { color: '#9ca3af' },
                grid: { color: 'rgba(55, 65, 81, 0.5)' },
            },
            y: {
                stacked: true,
                ticks: { color: '#9ca3af' },
                grid: { color: 'rgba(55, 65, 81, 0.5)' },
            },
        },
        plugins: {
            legend: {
                position: 'bottom',
                labels: {
                    color: '#ffffff',
                    usePointStyle: true,
                    pointStyle: 'rect',
                    padding: 16,
                },
            },
        },
    };
</script>
