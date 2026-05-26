import type { ContributionGridDto, DayActivityDto } from '@/api/apiClient/models';
import { useQuery } from '@tanstack/vue-query';
import { computed, type Ref } from 'vue';
import { useApiClient } from './useApiClient';
import { DateOnly } from '@microsoft/kiota-abstractions';

export function useContributionGrid(selectedYear: Ref<number>) {
    const apiClient = useApiClient();

    const gridQuery = useQuery({
        queryKey: computed(() => ['tasks', 'contribution-grid', selectedYear.value] as const),
        queryFn: (): Promise<ContributionGridDto | undefined> =>
            apiClient.api.tasks.contributionGrid.get({
                queryParameters: { year: selectedYear.value },
            }),
    });

    return {
        gridData: computed(() => gridQuery.data.value ?? undefined),
        isLoading: computed(() => gridQuery.isLoading.value),
    };
}

export function useDayActivity(selectedDate: Ref<string | null>) {
    const apiClient = useApiClient();

    const dayQuery = useQuery({
        queryKey: computed(() => ['tasks', 'day-activity', selectedDate.value] as const),
        queryFn: (): Promise<DayActivityDto | undefined> => {
            if (!selectedDate.value) return Promise.resolve(undefined);
            return apiClient.api.tasks.dayActivity.get({
                queryParameters: { date: DateOnly.parse(selectedDate.value) },
            });
        },
        enabled: computed(() => !!selectedDate.value),
    });

    return {
        dayActivity: computed(() => dayQuery.data.value ?? undefined),
        isDayLoading: computed(() => dayQuery.isLoading.value),
    };
}
