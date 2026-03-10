<template>
    <BaseBox>
        <BaseHeader tag="h3">Weekly Activity by Category</BaseHeader>
        <BaseChart
            type="bar"
            :data="chartData"
            :options="chartOptions"
            height="lg"
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseChart from '@/components/base/BaseChart.vue';
    import type { ChartData, ChartOptions } from 'chart.js';

    interface AnalyticsWeeklyActivityProps {
        data: ChartData<'bar'>;
    }

    const props = defineProps<AnalyticsWeeklyActivityProps>();

    const chartData = computed(() => props.data);

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
