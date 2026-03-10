<template>
    <div
        class="relative w-full"
        :class="heightClass"
    >
        <Chart
            class="absolute inset-0 h-full w-full"
            :type="type"
            :data="data"
            :options="mergedOptions"
        />
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import Chart from 'primevue/chart';
    import {
        Chart as ChartJS,
        CategoryScale,
        LinearScale,
        RadialLinearScale,
        BarElement,
        LineElement,
        PointElement,
        ArcElement,
        Legend,
        Tooltip,
        Filler,
    } from 'chart.js';
    import type { ChartData, ChartOptions } from 'chart.js';

    ChartJS.register(
        CategoryScale,
        LinearScale,
        RadialLinearScale,
        BarElement,
        LineElement,
        PointElement,
        ArcElement,
        Legend,
        Tooltip,
        Filler,
    );

    export type ChartType = 'bar' | 'line' | 'radar';
    export type ChartHeight = 'sm' | 'md' | 'lg';

    interface BaseChartProps {
        type: ChartType;
        data: ChartData;
        options?: ChartOptions;
        height?: ChartHeight;
    }

    const props = withDefaults(defineProps<BaseChartProps>(), {
        height: 'md',
    });

    const heightClasses: Record<ChartHeight, string> = {
        sm: 'h-48',
        md: 'h-72',
        lg: 'h-96',
    };

    const heightClass = computed(() => heightClasses[props.height]);

    const darkThemeDefaults: ChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                labels: {
                    color: '#ffffff',
                    usePointStyle: true,
                    pointStyle: 'rect',
                    padding: 16,
                },
            },
            tooltip: {
                backgroundColor: 'rgba(31, 41, 55, 0.95)',
                titleColor: '#ffffff',
                bodyColor: '#9ca3af',
                borderColor: 'rgba(55, 65, 81, 0.5)',
                borderWidth: 1,
            },
        },
        scales: {
            x: {
                ticks: { color: '#9ca3af' },
                grid: { color: 'rgba(55, 65, 81, 0.5)' },
            },
            y: {
                ticks: { color: '#9ca3af' },
                grid: { color: 'rgba(55, 65, 81, 0.5)' },
            },
        },
    };

    function deepMerge(target: Record<string, unknown>, source: Record<string, unknown>): Record<string, unknown> {
        const result = { ...target };
        for (const key of Object.keys(source)) {
            if (
                source[key] &&
                typeof source[key] === 'object' &&
                !Array.isArray(source[key]) &&
                target[key] &&
                typeof target[key] === 'object' &&
                !Array.isArray(target[key])
            ) {
                result[key] = deepMerge(
                    target[key] as Record<string, unknown>,
                    source[key] as Record<string, unknown>,
                );
            } else {
                result[key] = source[key];
            }
        }
        return result;
    }

    const mergedOptions = computed(() => {
        if (!props.options) return darkThemeDefaults;
        return deepMerge(
            darkThemeDefaults as unknown as Record<string, unknown>,
            props.options as unknown as Record<string, unknown>,
        ) as ChartOptions;
    });
</script>
