<script setup lang="ts">
    import { computed, ref } from 'vue';
    import type { ChartData } from 'chart.js';
    import PageLayout from '@/components/layout/PageLayout.vue';
    import BaseStatGrid from '@/components/base/BaseStatGrid.vue';
    import BaseAchievementGrid from '@/components/base/BaseAchievementGrid.vue';
    import type { BorderedStat } from '@/components/base/BaseStatCardBordered.vue';
    import { mapAchievementsToCards } from '@/utils';
    import AnalyticsWeeklyActivity from '@/components/analytics/AnalyticsWeeklyActivity.vue';
    import AnalyticsLifeBalance from '@/components/analytics/AnalyticsLifeBalance.vue';
    import AnalyticsStatGrowth from '@/components/analytics/AnalyticsStatGrowth.vue';
    import AnalyticsXpPerWeek from '@/components/analytics/AnalyticsXpPerWeek.vue';
    import { useChartColors } from '@/composables/useChartColors';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import {
        useAnalyticsOverview,
        useWeeklyActivity,
        useXpPerWeek,
        useStatGrowth,
        useLifeBalance,
        useAchievements,
    } from '@/composables/useAnalytics';
    import { BaseAttributeObject, type BaseAttribute } from '@/api/apiClient/models';

    const colors = useChartColors();
    const weeks = ref(6);

    const { overview } = useAnalyticsOverview();
    const { weeklyActivity } = useWeeklyActivity();
    const { xpPerWeek } = useXpPerWeek(weeks);
    const { statGrowth } = useStatGrowth(weeks);
    const { lifeBalance } = useLifeBalance();
    const { achievements: achievementsData } = useAchievements();

    const attributeOrder: BaseAttribute[] = [
        BaseAttributeObject.Strength,
        BaseAttributeObject.Knowledge,
        BaseAttributeObject.Health,
        BaseAttributeObject.Discipline,
        BaseAttributeObject.Focus,
        BaseAttributeObject.Energy,
    ];

    type ChartColorKey = 'success' | 'info' | 'error' | 'warning' | 'accent' | 'violet';
    const variantToColor: Record<string, ChartColorKey> = {
        success: 'success',
        info: 'info',
        error: 'error',
        warning: 'warning',
        violet: 'violet',
    };

    function attributeColor(attribute: BaseAttribute): string {
        const variant = getAttributeDisplay(attribute).variant;
        return colors[variantToColor[variant] ?? 'info'];
    }

    function attributeLabel(attribute: BaseAttribute): string {
        return getAttributeDisplay(attribute).label;
    }

    const dayLabels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

    const stats = computed<BorderedStat[]>(() => {
        const o = overview.value;
        return [
            {
                label: 'Total XP',
                value: (o?.totalXp ?? 0).toLocaleString(),
                subtitle: 'Lifetime earned',
                subtitleEmoji: '📈',
                borderVariant: 'info',
            },
            {
                label: 'Achievements',
                value: String(o?.achievementsUnlocked ?? 0),
                unit: `/ ${o?.achievementsTotal ?? 0}`,
                subtitle: achievementPercent.value,
                borderVariant: 'accent',
            },
            {
                label: 'Tasks Done',
                value: String(o?.tasksDone ?? 0),
                subtitle: 'Completed all time',
                subtitleEmoji: '✅',
                borderVariant: 'success',
            },
            {
                label: 'Best Streak',
                value: String(o?.bestStreak ?? 0),
                unit: 'days',
                subtitle: bestStreakSubtitle.value,
                subtitleEmoji: '🔥',
                borderVariant: 'violet',
            },
        ];
    });

    const achievementPercent = computed(() => {
        const total = overview.value?.achievementsTotal ?? 0;
        const unlocked = overview.value?.achievementsUnlocked ?? 0;
        if (total === 0) return '0% completed';
        return `${Math.round((unlocked / total) * 100)}% completed`;
    });

    const bestStreakSubtitle = computed(() => {
        const best = overview.value?.bestStreak ?? 0;
        return best > 0 ? 'Personal record!' : 'Get started!';
    });

    const weeklyActivityChartData = computed<ChartData<'bar'>>(() => {
        const days = weeklyActivity.value?.days ?? [];
        const datasets = attributeOrder.map((attr) => ({
            label: attributeLabel(attr),
            data: days.map((day) =>
                day.counts?.find((c) => c.attribute === attr)?.count ?? 0,
            ),
            backgroundColor: attributeColor(attr),
            borderRadius: 4,
        }));
        return { labels: dayLabels, datasets };
    });

    const xpPerWeekChartData = computed<ChartData<'bar'>>(() => {
        const entries = xpPerWeek.value?.weeks ?? [];
        return {
            labels: entries.map((e) => `W${e.weekNumber ?? 0}`),
            datasets: [
                {
                    label: 'XP',
                    data: entries.map((e) => e.xp ?? 0),
                    backgroundColor: colors.accent,
                    borderRadius: 6,
                },
            ],
        };
    });

    const statGrowthChartData = computed<ChartData<'line'>>(() => {
        const data = statGrowth.value;
        const series = data?.series ?? [];
        const weekCount = data?.weekStarts?.length ?? 0;
        const labels = Array.from({ length: weekCount }, (_, i) => `W${i + 1}`);
        const datasets = series.map((s) => {
            const color = s.attribute ? attributeColor(s.attribute) : colors.info;
            return {
                label: s.attribute ? attributeLabel(s.attribute) : 'Unknown',
                data: (s.levels ?? []).map((l) => l ?? 0),
                borderColor: color,
                backgroundColor: color,
                tension: 0.4,
                borderWidth: 2,
                pointRadius: 4,
                fill: false,
            };
        });
        return { labels, datasets };
    });

    const lifeBalanceChartData = computed<ChartData<'radar'>>(() => {
        const entries = lifeBalance.value?.attributes ?? [];
        const accentWithAlpha = `${colors.accent}33`;
        return {
            labels: entries.map((e) => (e.attribute ? attributeLabel(e.attribute) : 'Unknown')),
            datasets: [
                {
                    label: 'Balance',
                    data: entries.map((e) => e.score ?? 0),
                    backgroundColor: accentWithAlpha,
                    borderColor: colors.accent,
                    borderWidth: 2,
                    pointBackgroundColor: colors.accent,
                    pointBorderColor: colors.accent,
                    pointRadius: 4,
                },
            ],
        };
    });

    const achievements = computed(() =>
        mapAchievementsToCards(achievementsData.value?.achievements)
    );
</script>

<template>
    <PageLayout
        title="Progress Analytics"
        subtitle="Detailed insights into your growth journey"
    >
        <BaseStatGrid :stats="stats" />

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <AnalyticsWeeklyActivity :data="weeklyActivityChartData" />
            <AnalyticsLifeBalance :data="lifeBalanceChartData" />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <AnalyticsStatGrowth :data="statGrowthChartData" />
            <AnalyticsXpPerWeek :data="xpPerWeekChartData" />
        </div>

        <BaseAchievementGrid
            title="Achievement Progress"
            :achievements="achievements"
            :columns="3"
        />
    </PageLayout>
</template>
