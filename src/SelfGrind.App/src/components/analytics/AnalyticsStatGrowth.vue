<template>
    <BaseBox>
        <BaseHeader tag="h3">Stat Growth Over Time</BaseHeader>
        <BaseChart
            v-if="hasData"
            type="line"
            :data="chartData"
            :options="chartOptions"
            height="lg"
        />
        <BaseEmptyState
            v-else
            emoji="🌱"
            title="No growth yet"
            message="Level up attributes to track their growth here."
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

    interface AnalyticsStatGrowthProps {
        data: ChartData<'line'>;
    }

    const props = defineProps<AnalyticsStatGrowthProps>();

    const chartData = computed(() => props.data);
    const hasData = computed(() => hasChartData(props.data));

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
