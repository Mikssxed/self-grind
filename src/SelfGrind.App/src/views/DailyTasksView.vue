<script setup lang="ts">
    import { computed } from 'vue';
    import PageLayout from '@/components/layout/PageLayout.vue';
    import DailyTasksHabitTracker from '@/components/daily-tasks/DailyTasksHabitTracker.vue';
    import DailyTasksQuests from '@/components/daily-tasks/DailyTasksQuests.vue';
    import DailyTasksStats from '@/components/daily-tasks/DailyTasksStats.vue';
    import { useDailyTasks } from '@/composables/useDailyTasks';
    import { TaskOccurrenceStatusObject } from '@/api/apiClient/models';

    const { todayTasks, dailySummary, toggleOccurrence } = useDailyTasks();

    const stats = computed(() => [
        { emoji: '✅', value: String(dailySummary.value?.completedCount ?? 0), label: 'Completed Today' },
        { emoji: '🔥', value: String(dailySummary.value?.streak ?? 0), label: 'Day Streak' },
        { emoji: '⭐', value: `+${dailySummary.value?.totalExpEarnedToday ?? 0}`, label: 'XP Earned Today' },
    ]);

    const habits = [
        { emoji: '💧', label: 'Water', value: '6/8', variant: 'info' as const },
        { emoji: '🛏️', label: 'Sleep', value: '7.5h', variant: 'success' as const },
        { emoji: '🍽️', label: 'Meals', value: '3/3', variant: 'warning' as const },
        { emoji: '🏃', label: 'Steps', value: '8.2k', variant: 'violet' as const },
    ];

    function handleToggle(occurrenceId: string) {
        const task = todayTasks.value.find(t => t.occurrenceId === occurrenceId);
        toggleOccurrence(occurrenceId, task?.occurrenceStatus === TaskOccurrenceStatusObject.Done);
    }
</script>

<template>
    <PageLayout
        title="Daily Tasks & Habits ✅"
        subtitle="Complete tasks to earn XP and level up your character"
    >
        <DailyTasksStats :stats="stats" />

        <DailyTasksQuests
            :items="todayTasks"
            @toggle="handleToggle"
        />

        <DailyTasksHabitTracker :habits="habits" />
    </PageLayout>
</template>
