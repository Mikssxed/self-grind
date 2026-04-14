import type { CreateTaskCommand } from '@/api/apiClient/models';
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { useApiClient } from './useApiClient';

export function useTasks() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    const createMutation = useMutation({
        mutationFn: (command: CreateTaskCommand) => apiClient.api.tasks.post(command),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    });

    return { createMutation };
}
