<template>
    <BaseBox>
        <BaseHeader tag="h3">XP Earned Per Week</BaseHeader>
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

    interface AnalyticsXpPerWeekProps {
        data: ChartData<'bar'>;
    }

    const props = defineProps<AnalyticsXpPerWeekProps>();

    const chartData = computed(() => props.data);

    const chartOptions: ChartOptions<'bar'> = {
        plugins: {
            legend: {
                display: false,
            },
        },
    };
</script>
