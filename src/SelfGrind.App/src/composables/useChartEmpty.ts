import type { ChartData, ChartType } from 'chart.js';

export function hasChartData(data: ChartData<ChartType>): boolean {
    const datasets = data.datasets ?? [];
    if (datasets.length === 0) return false;

    return datasets.some(dataset => {
        const values = (dataset.data ?? []) as Array<number | null | undefined>;
        return values.some(value => typeof value === 'number' && value !== 0);
    });
}
