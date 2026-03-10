<template>
    <BaseBox>
        <BaseHeader tag="h3">Stat Growth Over Time</BaseHeader>
        <BaseChart
            type="line"
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

    interface AnalyticsStatGrowthProps {
        data: ChartData<'line'>;
    }

    const props = defineProps<AnalyticsStatGrowthProps>();

    const chartData = computed(() => props.data);

    const chartOptions: ChartOptions<'line'> = {
        plugins: {
            legend: {
                position: 'bottom',
                labels: {
                    color: '#ffffff',
                    usePointStyle: true,
                    pointStyle: 'circle',
                    padding: 16,
                },
            },
        },
    };
</script>
