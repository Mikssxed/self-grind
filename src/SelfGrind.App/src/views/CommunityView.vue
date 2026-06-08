<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import CommunityLeaderboard from '@/components/community/CommunityLeaderboard.vue';
    import CommunityFriends from '@/components/community/CommunityFriends.vue';
    import CommunityPartyQuests from '@/components/community/CommunityPartyQuests.vue';
    import type { Friend } from '@/components/community/CommunityFriendItem.vue';
    import type { PartyQuest } from '@/components/community/CommunityPartyQuestCard.vue';
    import { useLeaderboard } from '@/composables/useCommunity';

    const { entries: leaderboardEntries } = useLeaderboard();

    const friendsEnabled = false;
    const partyQuestsEnabled = false;

    const friends: Friend[] = [];
    const partyQuests: PartyQuest[] = [];
</script>

<template>
    <PageLayout
        title="Community"
        subtitle="Connect with fellow grinders and compete together"
    >
        <div class="grid grid-cols-1 lg:grid-cols-[2fr_1fr] gap-6 items-stretch">
            <CommunityLeaderboard :entries="leaderboardEntries" />
            <CommunityFriends v-if="friendsEnabled" :friends="friends" />
        </div>

        <CommunityPartyQuests v-if="partyQuestsEnabled" :quests="partyQuests" />
    </PageLayout>
</template>
