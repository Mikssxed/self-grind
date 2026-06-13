import type { CurrentUserDto } from '@/api/apiClient/models';
import { useQuery } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

export const currentUserQueryKey = ['user', 'current'] as const;

export function useCurrentUser() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: currentUserQueryKey,
        queryFn: (): Promise<CurrentUserDto | null | undefined> =>
            apiClient.api.identity.user.get(),
    });

    return {
        currentUser: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}
