<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CommunityLeaderboardEntry from './CommunityLeaderboardEntry.vue';
    import type { LeaderboardEntry } from './CommunityLeaderboardEntry.vue';

    interface Props {
        entries: LeaderboardEntry[];
    }

    defineProps<Props>();
</script>

<template>
    <BaseBox class="h-full">
        <div class="flex items-center justify-between">
            <BaseHeader tag="h3">Leaderboard</BaseHeader>
            <span class="text-sm font-semibold text-primary-200 bg-primary-800 px-4 py-2 rounded-xl border border-primary-600/30">
                This Week
            </span>
        </div>

        <div class="flex flex-col gap-2">
            <CommunityLeaderboardEntry
                v-for="entry in entries"
                :key="entry.rank"
                :entry="entry"
            />
        </div>
    </BaseBox>
</template>
