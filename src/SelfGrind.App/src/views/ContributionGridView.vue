<script setup lang="ts">
    import { ref, computed } from 'vue';
    import PageLayout from '@/components/layout/PageLayout.vue';
    import BaseAchievementGrid from '@/components/base/BaseAchievementGrid.vue';
    import ContributionGrid from '@/components/contribution-grid/ContributionGrid.vue';
    import BaseStatGrid from '@/components/base/BaseStatGrid.vue';
    import type { Achievement } from '@/components/base/BaseAchievementGrid.vue';
    import type { BorderedStat } from '@/components/base/BaseStatCardBordered.vue';
    import type { ContributionDay } from '@/components/contribution-grid/ContributionGrid.vue';
    import type { DayTask } from '@/components/contribution-grid/ContributionDayDetail.vue';
    import type { ActivityLevel } from '@/components/contribution-grid/ContributionGridCell.vue';

    const availableYears = [2024, 2025, 2026];
    const selectedYear = ref(new Date().getFullYear());

    const stats: BorderedStat[] = [
        {
            label: 'Current Streak',
            value: '15',
            unit: 'days',
            subtitle: 'Keep it up!',
            subtitleEmoji: '🔥',
            borderVariant: 'info',
        },
        {
            label: 'Longest Streak',
            value: '28',
            unit: 'days',
            subtitle: 'Personal best',
            subtitleEmoji: '🏆',
            borderVariant: 'accent',
        },
        {
            label: 'Total Days Active',
            value: '288',
            unit: 'days',
            subtitle: '79% this year',
            borderVariant: 'success',
        },
        {
            label: 'Completion Rate',
            value: '87',
            unit: '%',
            borderVariant: 'violet',
        },
    ];

    const taskPool: DayTask[] = [
        { title: 'Morning Workout', xp: 50, attribute: 'Strength', attributeEmoji: '💪', variant: 'error' },
        { title: 'Read 30 Pages', xp: 40, attribute: 'Knowledge', attributeEmoji: '📘', variant: 'info' },
        { title: 'Meditation Session', xp: 35, attribute: 'Discipline', attributeEmoji: '🔮', variant: 'violet' },
        { title: 'Code for 2 Hours', xp: 60, attribute: 'Focus', attributeEmoji: '🎯', variant: 'success' },
        { title: 'Journal Entry', xp: 25, attribute: 'Discipline', attributeEmoji: '🔮', variant: 'violet' },
        { title: 'Run 5K', xp: 55, attribute: 'Strength', attributeEmoji: '💪', variant: 'error' },
        { title: 'Study Session', xp: 45, attribute: 'Knowledge', attributeEmoji: '📘', variant: 'info' },
        { title: 'Yoga Practice', xp: 30, attribute: 'Energy', attributeEmoji: '⚡', variant: 'warning' },
    ];

    function seededRandom(seed: number): number {
        const x = Math.sin(seed) * 10000;
        return x - Math.floor(x);
    }

    function formatLocalDate(d: Date): string {
        const y = d.getFullYear();
        const m = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${y}-${m}-${day}`;
    }

    function generateDays(year: number): ContributionDay[] {
        const result: ContributionDay[] = [];
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        const startDate = new Date(year, 0, 1);
        const endDate = new Date(year, 11, 31);

        for (let d = new Date(startDate); d <= endDate; d.setDate(d.getDate() + 1)) {
            const dateStr = formatLocalDate(d);
            const isFuture = d > today;

            let level: ActivityLevel = 0;
            const tasks: DayTask[] = [];

            if (!isFuture) {
                const dayOfYear = Math.floor(
                    (d.getTime() - startDate.getTime()) / 86400000,
                );
                const rand = seededRandom(dayOfYear * 31 + 7 + year);
                if (rand < 0.25) level = 0;
                else if (rand < 0.45) level = 1;
                else if (rand < 0.65) level = 2;
                else if (rand < 0.85) level = 3;
                else level = 4;

                if (level > 0) {
                    const taskCount = level;
                    for (let t = 0; t < taskCount; t++) {
                        const taskIndex = Math.floor(
                            seededRandom(dayOfYear * 13 + t * 7 + year) * taskPool.length,
                        );
                        const task = taskPool[taskIndex];
                        if (task) tasks.push(task);
                    }
                }
            }

            result.push({ date: dateStr, level, tasks });
        }

        return result;
    }

    const days = computed(() => generateDays(selectedYear.value));

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
            @update:selected-year="selectedYear = $event"
        />

        <BaseAchievementGrid
            title="Streak Achievements"
            :achievements="achievements"
        />
    </PageLayout>
</template>
