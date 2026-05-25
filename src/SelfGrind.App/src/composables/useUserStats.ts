import type { UserStatsDto } from '@/api/apiClient/models';
import { useQuery } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

export const userStatsQueryKey = ['user', 'stats'] as const;

export function useUserStats() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: userStatsQueryKey,
        queryFn: (): Promise<UserStatsDto | null | undefined> =>
            apiClient.api.user.stats.get(),
    });

    return {
        userStats: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}
