import type { CreateHabitCommand, HabitDtoPagedResult, UpdateHabitCommand } from '@/api/apiClient/models';
import { keepPreviousData, useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import { computed, ref } from 'vue';
import { useApiClient } from './useApiClient';

const HABITS_PAGE_SIZE = 12;

export function useHabits() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const page = ref(1);

    const habitsQuery = useQuery({
        queryKey: ['habits', page],
        queryFn: (): Promise<HabitDtoPagedResult | undefined> =>
            apiClient.api.habits.get({
                queryParameters: { page: page.value, pageSize: HABITS_PAGE_SIZE },
            }),
        placeholderData: keepPreviousData,
    });

    const createMutation = useMutation({
        mutationFn: (command: CreateHabitCommand) => apiClient.api.habits.post(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['habits'] }),
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, command }: { id: string; command: UpdateHabitCommand }) =>
            apiClient.api.habits.byId(id).put(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['habits'] }),
    });

    const deleteMutation = useMutation({
        mutationFn: (id: string) => apiClient.api.habits.byId(id).delete(),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['habits'] }),
    });

    const logEntryMutation = useMutation({
        mutationFn: ({ id, value }: { id: string; value: number }) =>
            apiClient.api.habits.byId(id).entries.post({ value }),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['habits'] }),
    });

    const habits = computed(() => habitsQuery.data.value?.items ?? []);
    const totalPages = computed(() => habitsQuery.data.value?.totalPages ?? 1);

    const isLoading = computed(() => habitsQuery.isLoading.value);

    return {
        habits,
        page,
        totalPages,
        isLoading,
        createMutation,
        updateMutation,
        deleteMutation,
        logEntryMutation,
    };
}
