import type {
    AchievementsDto,
    AnalyticsOverviewDto,
    LifeBalanceDto,
    StatGrowthDto,
    WeeklyActivityDto,
    XpPerWeekDto,
} from '@/api/apiClient/models';
import { useQuery } from '@tanstack/vue-query';
import { computed, type Ref } from 'vue';
import { useApiClient } from './useApiClient';

export function useAnalyticsOverview() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['analytics', 'overview'] as const,
        queryFn: (): Promise<AnalyticsOverviewDto | undefined> =>
            apiClient.api.analytics.overview.get(),
    });

    return {
        overview: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useWeeklyActivity() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['analytics', 'weekly-activity'] as const,
        queryFn: (): Promise<WeeklyActivityDto | undefined> =>
            apiClient.api.analytics.weeklyActivity.get(),
    });

    return {
        weeklyActivity: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useXpPerWeek(weeks: Ref<number>) {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: computed(() => ['analytics', 'xp-per-week', weeks.value] as const),
        queryFn: (): Promise<XpPerWeekDto | undefined> =>
            apiClient.api.analytics.xpPerWeek.get({
                queryParameters: { weeks: weeks.value },
            }),
    });

    return {
        xpPerWeek: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useStatGrowth(weeks: Ref<number>) {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: computed(() => ['analytics', 'stat-growth', weeks.value] as const),
        queryFn: (): Promise<StatGrowthDto | undefined> =>
            apiClient.api.analytics.statGrowth.get({
                queryParameters: { weeks: weeks.value },
            }),
    });

    return {
        statGrowth: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useLifeBalance() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['analytics', 'life-balance'] as const,
        queryFn: (): Promise<LifeBalanceDto | undefined> =>
            apiClient.api.analytics.lifeBalance.get(),
    });

    return {
        lifeBalance: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useAchievements() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['analytics', 'achievements'] as const,
        queryFn: (): Promise<AchievementsDto | undefined> =>
            apiClient.api.analytics.achievements.get(),
    });

    return {
        achievements: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}
