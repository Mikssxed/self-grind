<script lang="ts" setup>
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import DailyTasksHabitCard, { type HabitCardVariant } from './DailyTasksHabitCard.vue';

    export interface Habit {
        emoji: string;
        label: string;
        value: string;
        variant: HabitCardVariant;
    }

    interface DailyTasksHabitTrackerProps {
        habits: Habit[];
    }

    defineProps<DailyTasksHabitTrackerProps>();
</script>

<template>
    <BaseBox>
        <BaseHeader tag="h3">Habit Tracker</BaseHeader>
        <div class="grid grid-cols-4 gap-4">
            <DailyTasksHabitCard
                v-for="habit in habits"
                :key="habit.label"
                :emoji="habit.emoji"
                :label="habit.label"
                :value="habit.value"
                :variant="habit.variant"
            />
        </div>
    </BaseBox>
</template>
