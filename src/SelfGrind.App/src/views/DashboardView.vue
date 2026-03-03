<script setup lang="ts">
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import DashboardCharacter from '@/components/dashboard/DashboardCharacter.vue';
    import DashboardCharacterStats from '@/components/dashboard/DashboardCharacterStats.vue';
    import DashboardDailyQuest from '@/components/dashboard/DashboardDailyQuest.vue';
    import DashboardRecentAchievements from '@/components/dashboard/DashboardRecentAchievements.vue';

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
        { emoji: '➕', label: 'Add Task', borderVariant: 'accent' as const },
        { emoji: '✅', label: 'Log Activity', borderVariant: 'info' as const },
        { emoji: '📊', label: 'View Stats', borderVariant: 'violet' as const },
        { emoji: '🎁', label: 'Daily Boost', borderVariant: 'warning' as const },
    ];
</script>

<template>
    <div class="flex flex-col gap-6 p-8 flex-1 max-h-screen overflow-y-auto">
        <!-- Page Header -->
        <div class="flex flex-col gap-1">
            <BaseHeader tag="h1">Welcome Back, Champion! 🎮</BaseHeader>
            <BaseText>Ready to level up today? Let's crush those goals!</BaseText>
        </div>

        <!-- Top Row: Character + Stats -->
        <div class="grid grid-cols-3 gap-6">
            <DashboardCharacter
                :level="42"
                title="Productivity Warrior"
                :streakDays="127"
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

        <DashboardRecentAchievements :achievements="achievements" />
    </div>
</template>
