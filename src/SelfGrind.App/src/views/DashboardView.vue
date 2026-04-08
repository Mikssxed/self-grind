<script setup lang="ts">
    import { computed } from 'vue';
    import { useRouter } from 'vue-router';
    import PageLayout from '@/components/layout/PageLayout.vue';
    import DashboardCharacter from '@/components/dashboard/DashboardCharacter.vue';
    import DashboardCharacterStats from '@/components/dashboard/DashboardCharacterStats.vue';
    import DashboardDailyQuest from '@/components/dashboard/DashboardDailyQuest.vue';
    import BaseAchievementGrid from '@/components/base/BaseAchievementGrid.vue';
    import { useAddTaskModal } from '@/composables/useAddTaskModal';
    import { useLogActivityModal } from '@/composables/useLogActivityModal';
    import { useDailyBoostModal } from '@/composables/useDailyBoostModal';
    import { useDailyTasks } from '@/composables/useDailyTasks';

    const router = useRouter();
    const { open: openAddTask } = useAddTaskModal();
    const { open: openLogActivity } = useLogActivityModal();
    const { open: openDailyBoost } = useDailyBoostModal();
    const { dailySummary } = useDailyTasks();
    const streakDays = computed(() => dailySummary.value?.streak ?? 0);

    const stats = [
        { label: 'Strength', emoji: '💪', value: 78, variant: 'error' as const },
        { label: 'Knowledge', emoji: '📘', value: 85, variant: 'info' as const },
        { label: 'Discipline', emoji: '🛡️', value: 92, variant: 'violet' as const },
        { label: 'Energy', emoji: '⚡', value: 67, variant: 'warning' as const },
        { label: 'Focus', emoji: '👁️', value: 81, variant: 'success' as const },
    ];

    const achievements = [
        { label: '100 Day Streak', emoji: '🔥', variant: 'orange' as const },
        { label: 'Bookworm', emoji: '📚', variant: 'blue' as const },
        { label: 'Zen Master', emoji: '🌿', variant: 'green' as const },
        { label: 'Marathon', emoji: '🏃', variant: 'purple' as const },
        { label: 'Champion', emoji: '👑', variant: 'crimson' as const },
    ];

    const questActions = [
        { emoji: '➕', label: 'Add Task', borderVariant: 'accent' as const, action: openAddTask },
        { emoji: '✅', label: 'Log Activity', borderVariant: 'info' as const, action: openLogActivity },
        { emoji: '📊', label: 'View Stats', borderVariant: 'violet' as const, action: () => router.push('/contribution-grid') },
        { emoji: '🎁', label: 'Daily Boost', borderVariant: 'warning' as const, action: openDailyBoost },
    ];
</script>

<template>
    <PageLayout
        title="Welcome Back, Champion! 🎮"
        subtitle="Ready to level up today? Let's crush those goals!"
    >
        <!-- Top Row: Character + Stats -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <DashboardCharacter
                :level="42"
                title="Productivity Warrior"
                :streakDays="streakDays"
                :xpCurrent="8450"
                :xpMax="10000"
                :achievementsCount="47"
                :achievementsTotal="100"
            />
            <DashboardCharacterStats :stats="stats" />
        </div>

        <DashboardDailyQuest
            quote="The only way to do great work is to love what you do."
            author="Steve Jobs"
            :xpReward="500"
            :actions="questActions"
        />

        <BaseAchievementGrid
            title="🥇 Recent Achievements"
            :achievements="achievements"
            :columns="5"
        />
    </PageLayout>
</template>
