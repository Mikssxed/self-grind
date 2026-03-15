<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import BaseStatGrid from '@/components/base/BaseStatGrid.vue';
    import AnalyticsWeeklyActivity from '@/components/analytics/AnalyticsWeeklyActivity.vue';
    import AnalyticsLifeBalance from '@/components/analytics/AnalyticsLifeBalance.vue';
    import AnalyticsStatGrowth from '@/components/analytics/AnalyticsStatGrowth.vue';
    import AnalyticsXpPerWeek from '@/components/analytics/AnalyticsXpPerWeek.vue';
    import BaseAchievementGrid from '@/components/base/BaseAchievementGrid.vue';
    import type { Achievement } from '@/components/base/BaseAchievementGrid.vue';
    import { useChartColors } from '@/composables/useChartColors';
    import type { BorderedStat } from '@/components/base/BaseStatCardBordered.vue';
    import type { ChartData } from 'chart.js';

    const colors = useChartColors();

    const stats: BorderedStat[] = [
        {
            label: 'Total XP',
            value: '45,230',
            subtitle: '+12.5% this week',
            subtitleEmoji: '📈',
            borderVariant: 'info',
        },
        {
            label: 'Achievements',
            value: '24',
            unit: '/ 50',
            subtitle: '48% completed',
            borderVariant: 'accent',
        },
        {
            label: 'Tasks Done',
            value: '342',
            subtitle: '+28 this week',
            subtitleEmoji: '✅',
            borderVariant: 'success',
        },
        {
            label: 'Best Streak',
            value: '28',
            unit: 'days',
            subtitle: 'Personal record!',
            subtitleEmoji: '🔥',
            borderVariant: 'violet',
        },
    ];

    const weeklyActivityData: ChartData<'bar'> = {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        datasets: [
            {
                label: 'Learning',
                data: [4, 6, 5, 8, 7, 10, 6],
                backgroundColor: colors.success,
                borderRadius: 4,
            },
            {
                label: 'Mental',
                data: [6, 5, 7, 4, 6, 5, 8],
                backgroundColor: colors.info,
                borderRadius: 4,
            },
            {
                label: 'Physical',
                data: [8, 10, 6, 8, 5, 8, 6],
                backgroundColor: colors.error,
                borderRadius: 4,
            },
            {
                label: 'Wellness',
                data: [6, 5, 8, 6, 8, 4, 5],
                backgroundColor: colors.warning,
                borderRadius: 4,
            },
        ],
    };

    const accentWithAlpha = `${colors.accent}33`;

    const lifeBalanceData: ChartData<'radar'> = {
        labels: ['Physical', 'Mental', 'Learning', 'Wellness', 'Productivity'],
        datasets: [
            {
                label: 'Balance',
                data: [78, 65, 82, 55, 70],
                backgroundColor: accentWithAlpha,
                borderColor: colors.accent,
                borderWidth: 2,
                pointBackgroundColor: colors.accent,
                pointBorderColor: colors.accent,
                pointRadius: 4,
            },
        ],
    };

    const statGrowthData: ChartData<'line'> = {
        labels: ['W1', 'W2', 'W3', 'W4', 'W5', 'W6'],
        datasets: [
            {
                label: 'Energy',
                data: [50, 55, 60, 58, 72, 80],
                borderColor: colors.warning,
                backgroundColor: colors.warning,
                tension: 0.4,
                borderWidth: 2,
                pointRadius: 4,
                fill: false,
            },
            {
                label: 'Focus',
                data: [45, 52, 48, 65, 70, 78],
                borderColor: colors.success,
                backgroundColor: colors.success,
                tension: 0.4,
                borderWidth: 2,
                pointRadius: 4,
                fill: false,
            },
            {
                label: 'Knowledge',
                data: [55, 58, 62, 70, 75, 85],
                borderColor: colors.info,
                backgroundColor: colors.info,
                tension: 0.4,
                borderWidth: 2,
                pointRadius: 4,
                fill: false,
            },
            {
                label: 'Strength',
                data: [48, 50, 55, 60, 68, 75],
                borderColor: colors.error,
                backgroundColor: colors.error,
                tension: 0.4,
                borderWidth: 2,
                pointRadius: 4,
                fill: false,
            },
        ],
    };

    const xpPerWeekData: ChartData<'bar'> = {
        labels: ['W1', 'W2', 'W3', 'W4', 'W5', 'W6'],
        datasets: [
            {
                label: 'XP',
                data: [420, 580, 350, 720, 850, 940],
                backgroundColor: colors.accent,
                borderRadius: 6,
            },
        ],
    };

    const achievements: Achievement[] = [
        {
            emoji: '🏃',
            label: 'First Steps',
            subtitle: 'Complete your first task',
            variant: 'orange',
            locked: false,
        },
        {
            emoji: '⚔️',
            label: 'Week Warrior',
            subtitle: 'Complete all daily tasks for a week',
            variant: 'blue',
            locked: false,
        },
        {
            emoji: '📚',
            label: 'Knowledge Seeker',
            subtitle: 'Read 10 books',
            variant: 'green',
            locked: false,
        },
        {
            emoji: '🧘',
            label: 'Zen Master',
            subtitle: 'Meditate for 30 days straight',
            variant: 'purple',
            locked: true,
        },
        {
            emoji: '💪',
            label: 'Iron Will',
            subtitle: 'Reach level 50',
            variant: 'crimson',
            locked: true,
        },
        {
            emoji: '👑',
            label: 'Perfect Month',
            subtitle: '100% completion rate for a month',
            variant: 'orange',
            locked: true,
        },
    ];
</script>

<template>
    <PageLayout
        title="Progress Analytics"
        subtitle="Detailed insights into your growth journey"
    >
        <BaseStatGrid :stats="stats" />

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <AnalyticsWeeklyActivity :data="weeklyActivityData" />
            <AnalyticsLifeBalance :data="lifeBalanceData" />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <AnalyticsStatGrowth :data="statGrowthData" />
            <AnalyticsXpPerWeek :data="xpPerWeekData" />
        </div>

        <BaseAchievementGrid
            title="Achievement Progress"
            :achievements="achievements"
            :columns="3"
        />
    </PageLayout>
</template>
