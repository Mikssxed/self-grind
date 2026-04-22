import type { CreateHabitCommand, HabitDto, UpdateHabitCommand } from '@/api/apiClient/models';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

export function useHabits() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const habitsQuery = useQuery({
        queryKey: ['habits'],
        queryFn: (): Promise<HabitDto[] | undefined> => apiClient.api.habits.get(),
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

    const habits = computed(() => habitsQuery.data.value ?? []);

    return {
        habits,
        isLoading: computed(() => habitsQuery.isLoading.value),
        createMutation,
        updateMutation,
        deleteMutation,
        logEntryMutation,
    };
}
