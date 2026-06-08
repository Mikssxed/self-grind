<template>
    <BaseBox>
        <BaseHeader tag="h3">Life Balance Overview</BaseHeader>
        <BaseChart
            v-if="hasData"
            type="radar"
            :data="chartData"
            :options="chartOptions"
            height="lg"
        />
        <BaseEmptyState
            v-else
            emoji="🧭"
            title="No balance to show"
            message="Spread XP across attributes to see your life balance."
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

    interface AnalyticsLifeBalanceProps {
        data: ChartData<'radar'>;
    }

    const props = defineProps<AnalyticsLifeBalanceProps>();

    const chartData = computed(() => props.data);
    const hasData = computed(() => hasChartData(props.data));

    const chartOptions: ChartOptions<'radar'> = {
        scales: {
            r: {
                beginAtZero: true,
                max: 100,
                ticks: {
                    display: false,
                },
                grid: {
                    color: 'rgba(55, 65, 81, 0.5)',
                },
                angleLines: {
                    color: 'rgba(55, 65, 81, 0.5)',
                },
                pointLabels: {
                    color: '#ffffff',
                    font: { size: 12 },
                },
            },
        },
        plugins: {
            legend: {
                display: false,
            },
        },
    };
</script>
