<template>
    <BaseBox>
        <div class="flex items-center justify-between">
            <BaseHeader tag="h3">{{ selectedYear }} Activity Map</BaseHeader>
            <ContributionGridLegend />
        </div>
        <div class="flex gap-6">
            <ContributionYearSelector
                :years="years"
                :selected-year="selectedYear"
                @update:selected-year="$emit('update:selectedYear', $event)"
            />
            <div class="contribution-grid flex flex-col gap-2 flex-1 overflow-x-auto">
                <div
                    class="grid grid-rows-7 grid-flow-col"
                    style="gap: var(--grid-gap); grid-auto-columns: var(--cell-size);"
                >
                    <div
                        v-for="n in startDayOffset"
                        :key="'pad-' + n"
                    />
                    <ContributionGridCell
                        v-for="day in days"
                        :key="day.date"
                        :level="day.level"
                        :is-selected="selectedDate === day.date"
                        @select="selectedDate = day.date"
                    />
                </div>
                <div
                    class="grid text-xs text-primary-400"
                    :style="{
                        gap: 'var(--grid-gap)',
                        gridTemplateColumns: `repeat(${totalColumns}, var(--cell-size))`,
                    }"
                >
                    <span
                        v-for="month in monthLabels"
                        :key="month.column"
                        :style="{ gridColumn: month.column }"
                    >
                        {{ month.name }}
                    </span>
                </div>
            </div>
        </div>
        <ContributionDayDetail
            :date="selectedDate"
            :tasks="selectedDayTasks"
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed, ref } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import ContributionGridCell from './ContributionGridCell.vue';
    import ContributionGridLegend from './ContributionGridLegend.vue';
    import ContributionDayDetail from './ContributionDayDetail.vue';
    import ContributionYearSelector from './ContributionYearSelector.vue';
    import type { ActivityLevel } from './ContributionGridCell.vue';
    import type { DayTask } from './ContributionDayDetail.vue';

    export interface ContributionDay {
        date: string;
        level: ActivityLevel;
        tasks: DayTask[];
    }

    interface ContributionGridProps {
        days: ContributionDay[];
        years: number[];
        selectedYear: number;
    }

    const props = defineProps<ContributionGridProps>();

    defineEmits<{
        'update:selectedYear': [year: number];
    }>();

    const selectedDate = ref<string | null>(null);

    function parseLocalDate(dateStr: string): Date {
        const [y, m, d] = dateStr.split('-').map(Number);
        return new Date(y!, m! - 1, d!);
    }

    const selectedDayTasks = computed(() => {
        if (!selectedDate.value) return [];
        const day = props.days.find((d: ContributionDay) => d.date === selectedDate.value);
        return day?.tasks ?? [];
    });

    const startDayOffset = computed(() => {
        const firstDay = props.days[0];
        if (!firstDay) return 0;
        const date = parseLocalDate(firstDay.date);
        return (date.getDay() + 6) % 7; // Monday-start: Mon=0, Sun=6
    });

    const totalColumns = computed(() => {
        return Math.ceil((props.days.length + startDayOffset.value) / 7);
    });

    const monthLabels = computed(() => {
        const labels: { name: string; column: number }[] = [];
        let lastMonth = -1;

        for (let i = 0; i < props.days.length; i++) {
            const day = props.days[i];
            if (!day) continue;
            const date = parseLocalDate(day.date);
            const month = date.getMonth();

            if (month !== lastMonth) {
                const col = Math.floor((i + startDayOffset.value) / 7) + 1;
                labels.push({
                    name: date.toLocaleDateString('en-US', { month: 'short' }),
                    column: col,
                });
                lastMonth = month;
            }
        }

        return labels;
    });
</script>
<style scoped>
    .contribution-grid {
        --cell-size: 12px;
        --grid-gap: 4px;
    }

    @media (min-width: 1280px) {
        .contribution-grid {
            --cell-size: 14px;
            --grid-gap: 4px;
        }
    }

    @media (min-width: 1536px) {
        .contribution-grid {
            --cell-size: 16px;
            --grid-gap: 5px;
        }
    }
</style>
