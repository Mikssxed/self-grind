<script lang="ts" setup>
    import type { HabitDto } from '@/api/apiClient/models';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import DailyTasksHabitCard from './DailyTasksHabitCard.vue';
    import DailyTasksHabitModal from './DailyTasksHabitModal.vue';
    import { useHabitModal } from '@/composables/useHabitModal';

    interface Props {
        habits: (HabitDto & { id: string })[];
    }

    defineProps<Props>();

    const { openAdd, openEdit } = useHabitModal();
</script>

<template>
    <BaseBox>
        <BaseHeader tag="h3">Habit Tracker</BaseHeader>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
            <DailyTasksHabitCard
                v-for="habit in habits"
                :key="habit.id ?? ''"
                :habit="habit"
                @edit="openEdit(habit)"
            />
            <button
                class="flex flex-col items-center justify-center gap-2 p-6 rounded-xl border-2 border-dashed border-primary-800 text-primary-400 cursor-pointer transition-all hover:border-accent-500 hover:text-accent-500 hover:bg-accent-500/5"
                @click="openAdd()"
            >
                <span class="text-2xl">+</span>
                <span class="text-sm">Add Habit</span>
            </button>
        </div>
    </BaseBox>
    <DailyTasksHabitModal />
</template>
