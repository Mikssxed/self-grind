import type { TaskItemDtoPagedResult } from '@/api/apiClient/models';
import { keepPreviousData, useQuery } from '@tanstack/vue-query';
import { computed, ref } from 'vue';
import { useApiClient } from './useApiClient';

const TASKS_PAGE_SIZE = 10;

export function useTasksList() {
    const apiClient = useApiClient();

    const page = ref(1);

    const tasksQuery = useQuery({
        queryKey: ['tasks', 'list', page],
        queryFn: (): Promise<TaskItemDtoPagedResult | undefined> =>
            apiClient.api.tasks.get({
                queryParameters: { page: page.value, pageSize: TASKS_PAGE_SIZE },
            }),
        placeholderData: keepPreviousData,
    });

    const tasks = computed(() => tasksQuery.data.value?.items ?? []);
    const totalPages = computed(() => tasksQuery.data.value?.totalPages ?? 1);
    const isLoading = computed(() => tasksQuery.isLoading.value);

    return {
        tasks,
        page,
        totalPages,
        isLoading,
    };
}
