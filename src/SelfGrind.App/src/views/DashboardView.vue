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
    import { useUserStats } from '@/composables/useUserStats';

    const router = useRouter();
    const { open: openAddTask } = useAddTaskModal();
    const { open: openLogActivity } = useLogActivityModal();
    const { open: openDailyBoost } = useDailyBoostModal();
    const { dailySummary } = useDailyTasks();
    const { userStats } = useUserStats();

    const streakDays = computed(() => dailySummary.value?.streak ?? 0);
    const level = computed(() => userStats.value?.level ?? 1);
    const xpCurrent = computed(() => userStats.value?.exp ?? 0);
    const xpMax = computed(() => userStats.value?.requiredExp ?? 0);
    const attributeStats = computed(() => userStats.value?.attributeStats ?? []);

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
                :level="level"
                title="Productivity Warrior"
                :streakDays="streakDays"
                :xpCurrent="xpCurrent"
                :xpMax="xpMax"
                :achievementsCount="0"
                :achievementsTotal="0"
            />
            <DashboardCharacterStats :stats="attributeStats" />
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
