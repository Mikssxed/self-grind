import { ref, readonly } from 'vue';
import type { Habit } from '@/components/daily-tasks/DailyTasksHabitTracker.vue';

const isOpen = ref(false);
const editingHabit = ref<Habit | null>(null);

export function useHabitModal() {
    const openAdd = () => {
        editingHabit.value = null;
        isOpen.value = true;
    };

    const openEdit = (habit: Habit) => {
        editingHabit.value = habit;
        isOpen.value = true;
    };

    const close = () => {
        isOpen.value = false;
        editingHabit.value = null;
    };

    return {
        isOpen: readonly(isOpen),
        editingHabit: readonly(editingHabit),
        openAdd,
        openEdit,
        close,
    };
}
