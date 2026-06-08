import type { LeaderboardDto } from '@/api/apiClient/models';
import { useQuery } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

export function useLeaderboard() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['community', 'leaderboard'] as const,
        queryFn: (): Promise<LeaderboardDto | undefined> =>
            apiClient.api.community.leaderboard.get(),
    });

    return {
        leaderboard: computed(() => query.data.value ?? undefined),
        entries: computed(() => query.data.value?.entries ?? []),
        isLoading: computed(() => query.isLoading.value),
    };
}
