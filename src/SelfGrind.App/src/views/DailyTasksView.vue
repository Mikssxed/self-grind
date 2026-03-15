<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import DailyTasksHabitTracker from '@/components/daily-tasks/DailyTasksHabitTracker.vue';
    import DailyTasksQuests from '@/components/daily-tasks/DailyTasksQuests.vue';
    import DailyTasksStats from '@/components/daily-tasks/DailyTasksStats.vue';

    const stats = [
        { emoji: '✅', value: '12', label: 'Completed Today' },
        { emoji: '🔥', value: '127', label: 'Day Streak' },
        { emoji: '⭐', value: '+850', label: 'XP Earned Today' },
    ];

    const quests = [
        {
            title: 'Morning Workout',
            description: '30 minutes cardio',
            xp: 50,
            attribute: 'Strength',
            attributeEmoji: '💪',
            variant: 'error' as const,
            completed: true,
        },
        {
            title: 'Read 30 Pages',
            description: 'Personal development book',
            xp: 40,
            attribute: 'Knowledge',
            attributeEmoji: '📘',
            variant: 'info' as const,
            completed: false,
        },
        {
            title: 'Meditation Session',
            description: '15 minutes mindfulness',
            xp: 35,
            attribute: 'Discipline',
            attributeEmoji: '🛡️',
            variant: 'violet' as const,
            completed: false,
        },
        {
            title: 'Code for 2 Hours',
            description: 'Work on side project',
            xp: 60,
            attribute: 'Focus',
            attributeEmoji: '👁️',
            variant: 'success' as const,
            completed: false,
        },
    ];

    const habits = [
        { emoji: '💧', label: 'Water', value: '6/8', variant: 'info' as const },
        { emoji: '🛏️', label: 'Sleep', value: '7.5h', variant: 'success' as const },
        { emoji: '🍽️', label: 'Meals', value: '3/3', variant: 'warning' as const },
        { emoji: '🏃', label: 'Steps', value: '8.2k', variant: 'violet' as const },
    ];
</script>

<template>
    <PageLayout
        title="Daily Tasks & Habits ✅"
        subtitle="Complete tasks to earn XP and level up your character"
    >
        <DailyTasksStats :stats="stats" />

        <DailyTasksQuests :quests="quests" />

        <DailyTasksHabitTracker :habits="habits" />
    </PageLayout>
</template>
