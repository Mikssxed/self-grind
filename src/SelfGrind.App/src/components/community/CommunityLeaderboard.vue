<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CommunityLeaderboardEntry from './CommunityLeaderboardEntry.vue';
    import type { LeaderboardEntryDto } from '@/api/apiClient/models';

    interface Props {
        entries: LeaderboardEntryDto[];
    }

    const props = defineProps<Props>();
    const hasEntries = computed(() => props.entries.length > 0);
</script>

<template>
    <BaseBox class="h-full">
        <div class="flex items-center justify-between">
            <BaseHeader tag="h3">Leaderboard</BaseHeader>
            <span class="text-sm font-semibold text-primary-200 bg-primary-800 px-4 py-2 rounded-xl border border-primary-600/30">
                This Week
            </span>
        </div>

        <div v-if="hasEntries" class="flex flex-col gap-2">
            <CommunityLeaderboardEntry
                v-for="entry in entries"
                :key="entry.rank"
                :entry="entry"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🏁"
            title="No rankings yet"
            message="Complete tasks this week to appear on the leaderboard."
        />
    </BaseBox>
</template>
