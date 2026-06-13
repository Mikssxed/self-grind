import type { CreateTaskCommand, LogActivityCommand, UpdateTaskCommand } from '@/api/apiClient/models';
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { useApiClient } from './useApiClient';

export function useTasks() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const createMutation = useMutation({
        mutationFn: (command: CreateTaskCommand) => apiClient.api.tasks.post(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, command }: { id: string; command: UpdateTaskCommand }) =>
            apiClient.api.tasks.byId(id).put(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    });

    const deleteMutation = useMutation({
        mutationFn: (id: string) => apiClient.api.tasks.byId(id).delete(),
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

    return { createMutation, updateMutation, deleteMutation, logActivityMutation };
}
