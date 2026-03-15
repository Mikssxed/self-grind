<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import CommunityLeaderboard from '@/components/community/CommunityLeaderboard.vue';
    import CommunityFriends from '@/components/community/CommunityFriends.vue';
    import CommunityPartyQuests from '@/components/community/CommunityPartyQuests.vue';
    import type { LeaderboardEntry } from '@/components/community/CommunityLeaderboardEntry.vue';
    import type { Friend } from '@/components/community/CommunityFriendItem.vue';
    import type { PartyQuest } from '@/components/community/CommunityPartyQuestCard.vue';

    const leaderboardEntries: LeaderboardEntry[] = [
        {
            rank: 1,
            name: 'Sarah Johnson',
            level: 48,
            title: 'Legendary Champion',
            xp: 12847,
            isCurrentUser: false,
            avatarEmoji: '👩',
        },
        {
            rank: 2,
            name: 'Mike Chen',
            level: 45,
            title: 'Elite Warrior',
            xp: 11523,
            isCurrentUser: false,
            avatarEmoji: '👨',
        },
        {
            rank: 3,
            name: 'Emma Davis',
            level: 44,
            title: 'Elite Warrior',
            xp: 10891,
            isCurrentUser: false,
            avatarEmoji: '👩',
        },
        {
            rank: 4,
            name: 'Alex Chen',
            level: 42,
            title: 'Elite Warrior',
            xp: 9847,
            isCurrentUser: true,
            avatarEmoji: '🧑',
        },
        {
            rank: 5,
            name: 'James Wilson',
            level: 41,
            title: 'Advanced Warrior',
            xp: 9234,
            isCurrentUser: false,
            avatarEmoji: '👨',
        },
    ];

    const friends: Friend[] = [
        {
            name: 'Sarah Johnson',
            level: 48,
            status: 'online',
            avatarEmoji: '👩',
        },
        {
            name: 'Mike Chen',
            level: 45,
            status: 'offline',
            lastSeen: '2h ago',
            avatarEmoji: '👨',
        },
        {
            name: 'Emma Davis',
            level: 44,
            status: 'online',
            avatarEmoji: '👩',
        },
        {
            name: 'James Wilson',
            level: 41,
            status: 'offline',
            lastSeen: '1d ago',
            avatarEmoji: '👨',
        },
    ];

    const partyQuests: PartyQuest[] = [
        {
            title: '30-Day Fitness Challenge',
            description: 'Complete 30 consecutive days of workouts with your party',
            memberEmojis: ['👩', '👨', '🧑'],
            extraMembers: 2,
            progress: 18,
            total: 30,
            unit: 'Days',
            reward: '+1000 XP, Exclusive Badge',
            variant: 'purple',
        },
        {
            title: 'Reading Marathon',
            description: 'Read 10 books collectively as a team this month',
            memberEmojis: ['👩', '👨', '🧑'],
            extraMembers: 0,
            progress: 7,
            total: 10,
            unit: 'Books',
            reward: '+800 XP, Knowledge Boost',
            variant: 'blue',
        },
    ];
</script>

<template>
    <PageLayout
        title="Community"
        subtitle="Connect with fellow grinders and compete together"
    >
        <div class="grid grid-cols-1 lg:grid-cols-[2fr_1fr] gap-6 items-stretch">
            <CommunityLeaderboard :entries="leaderboardEntries" />
            <CommunityFriends :friends="friends" />
        </div>

        <CommunityPartyQuests :quests="partyQuests" />
    </PageLayout>
</template>
