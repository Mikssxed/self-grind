import type { DailySummaryDto, TodayTaskItemDto } from '@/api/apiClient/models';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

export function useDailyTasks() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const todayTasksQuery = useQuery({
        queryKey: ['tasks', 'today'],
        queryFn: (): Promise<TodayTaskItemDto[] | null | undefined> =>
            apiClient.api.tasks.today.get(),
    });

    const dailySummaryQuery = useQuery({
        queryKey: ['tasks', 'daily-summary'],
        queryFn: (): Promise<DailySummaryDto | null | undefined> =>
            apiClient.api.tasks.dailySummary.get(),
    });

    const completeMutation = useMutation({
        mutationFn: (occurrenceId: string) =>
            apiClient.api.occurrences.byId(occurrenceId).complete.post(),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['tasks'] });
        },
    });

    const undoMutation = useMutation({
        mutationFn: (occurrenceId: string) =>
            apiClient.api.occurrences.byId(occurrenceId).undo.post(),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['tasks'] });
        },
    });

    function toggleOccurrence(occurrenceId: string, currentlyCompleted: boolean) {
        if (currentlyCompleted) {
            undoMutation.mutate(occurrenceId);
        } else {
            completeMutation.mutate(occurrenceId);
        }
    }

    return {
        todayTasks: computed(() => todayTasksQuery.data.value ?? []),
        dailySummary: computed(() => dailySummaryQuery.data.value ?? undefined),
        isLoading: computed(
            () => todayTasksQuery.isLoading.value || dailySummaryQuery.isLoading.value
        ),
        toggleOccurrence,
    };
}
