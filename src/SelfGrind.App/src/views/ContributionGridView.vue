<script setup lang="ts">
    import { ref, computed } from 'vue';
    import PageLayout from '@/components/layout/PageLayout.vue';
    import BaseAchievementGrid from '@/components/base/BaseAchievementGrid.vue';
    import ContributionGrid from '@/components/contribution-grid/ContributionGrid.vue';
    import BaseStatGrid from '@/components/base/BaseStatGrid.vue';
    import type { Achievement } from '@/components/base/BaseAchievementGrid.vue';
    import type { BorderedStat } from '@/components/base/BaseStatCardBordered.vue';
    import type { ContributionDay } from '@/components/contribution-grid/ContributionGrid.vue';
    import type { ActivityLevel } from '@/components/contribution-grid/ContributionGridCell.vue';
    import type { DayTask, DayTaskVariant } from '@/components/contribution-grid/ContributionDayDetail.vue';
    import { useContributionGrid, useDayActivity } from '@/composables/useContributionGrid';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';

    const selectedYear = ref(new Date().getFullYear());
    const selectedDate = ref<string | null>(null);

    const { gridData } = useContributionGrid(selectedYear);
    const { dayActivity } = useDayActivity(selectedDate);

    const availableYears = computed(() => {
        const years = gridData.value?.availableYears ?? [];
        const currentYear = new Date().getFullYear();
        if (years.length === 0) return [currentYear];
        return years;
    });

    const days = computed<ContributionDay[]>(() => {
        if (!gridData.value?.days) return [];
        return gridData.value.days.map(d => ({
            date: d.date?.toString() ?? '',
            level: (d.level ?? 0) as ActivityLevel,
        }));
    });

    const stats = computed<BorderedStat[]>(() => {
        if (!gridData.value) return [];
        return [
            {
                label: 'Current Streak',
                value: String(gridData.value.currentStreak ?? 0),
                unit: 'days',
                subtitle: (gridData.value.currentStreak ?? 0) > 0 ? 'Keep it up!' : 'Start today!',
                subtitleEmoji: '🔥',
                borderVariant: 'info',
            },
            {
                label: 'Longest Streak',
                value: String(gridData.value.longestStreak ?? 0),
                unit: 'days',
                subtitle: 'Personal best',
                subtitleEmoji: '🏆',
                borderVariant: 'accent',
            },
            {
                label: 'Total Days Active',
                value: String(gridData.value.totalDaysActive ?? 0),
                unit: 'days',
                subtitle: `${Math.round(gridData.value.activePercentage ?? 0)}% this year`,
                borderVariant: 'success',
            },
            {
                label: 'Completion Rate',
                value: String(Math.round(gridData.value.completionRate ?? 0)),
                unit: '%',
                borderVariant: 'violet',
            },
        ];
    });

    const selectedDayTasks = computed<DayTask[]>(() => {
        if (!dayActivity.value?.tasks) return [];
        return dayActivity.value.tasks.map(t => {
            const display = getAttributeDisplay(t.attribute);
            return {
                title: t.title ?? '',
                xp: t.xp ?? 0,
                attribute: display.label,
                attributeEmoji: display.emoji,
                variant: display.variant as DayTaskVariant,
            };
        });
    });

    const achievements: Achievement[] = [
        {
            emoji: '🔥',
            label: 'Streak Master',
            subtitle: '30 day streak achieved',
            variant: 'orange',
        },
        {
            emoji: '🧘',
            label: 'Mindful Warrior',
            subtitle: '30 days of meditation',
            variant: 'blue',
        },
        {
            emoji: '📚',
            label: 'Knowledge Seeker',
            subtitle: 'Read 10 books',
            variant: 'green',
        },
        {
            emoji: '👑',
            label: 'Consistency King',
            subtitle: 'No missed days in a month',
            variant: 'purple',
        },
    ];
</script>

<template>
    <PageLayout
        title="Contribution Grid"
        subtitle="Track your daily progress and maintain your streak"
    >
        <BaseStatGrid :stats="stats" />

        <ContributionGrid
            :days="days"
            :years="availableYears"
            :selected-year="selectedYear"
            :selected-date="selectedDate"
            :day-tasks="selectedDayTasks"
            @update:selected-year="selectedYear = $event"
            @update:selected-date="selectedDate = $event"
        />

        <BaseAchievementGrid
            title="Streak Achievements"
            :achievements="achievements"
        />
    </PageLayout>
</template>
