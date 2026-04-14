import { ref, readonly } from 'vue';
import type { HabitDto } from '@/api/apiClient/models';

const isOpen = ref(false);
const editingHabit = ref<HabitDto | null>(null);

export function useHabitModal() {
    const openAdd = () => {
        editingHabit.value = null;
        isOpen.value = true;
    };

    const openEdit = (habit: HabitDto) => {
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
