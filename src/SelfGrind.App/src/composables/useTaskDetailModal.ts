import type { TaskItemDto } from '@/api/apiClient/models';
import { readonly, ref } from 'vue';

// Module-level singleton so any task row can open the shared detail modal mounted in SidebarLayout.
const isOpen = ref(false);
const selectedTask = ref<TaskItemDto | null>(null);

export function useTaskDetailModal() {
    const open = (task: TaskItemDto) => {
        selectedTask.value = task;
        isOpen.value = true;
    };

    const close = () => {
        isOpen.value = false;
    };

    return {
        isOpen: readonly(isOpen),
        selectedTask,
        open,
        close,
    };
}
