import type { CreateTaskCommand, LogActivityCommand } from '@/api/apiClient/models';
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { useApiClient } from './useApiClient';

export function useTasks() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const createMutation = useMutation({
        mutationFn: (command: CreateTaskCommand) => apiClient.api.tasks.post(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    });

    const logActivityMutation = useMutation({
        mutationFn: (command: LogActivityCommand) => apiClient.api.tasks.logActivity.post(command),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['tasks'] });
            queryClient.invalidateQueries({ queryKey: ['user', 'stats'] });
            queryClient.invalidateQueries({ queryKey: ['character'] });
            queryClient.invalidateQueries({ queryKey: ['analytics'] });
            queryClient.invalidateQueries({ queryKey: ['contribution-grid'] });
        },
    });

    return { createMutation, logActivityMutation };
}
