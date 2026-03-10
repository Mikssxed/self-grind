<script setup lang="ts">
    import { computed } from 'vue';
    import { twMerge } from 'tailwind-merge';
    import BaseAvatar from '@/components/base/BaseAvatar.vue';

    export interface LeaderboardEntry {
        rank: number;
        name: string;
        level: number;
        title: string;
        xp: number;
        isCurrentUser: boolean;
        avatarEmoji: string;
    }

    type RankVariant = 'gold' | 'silver' | 'bronze' | 'currentUser' | 'default';

    interface Props {
        entry: LeaderboardEntry;
    }

    const props = defineProps<Props>();

    const rankEmojis: Record<number, string> = {
        1: '🥇',
        2: '🥈',
        3: '🥉',
    };

    const rankDisplay = computed(() => rankEmojis[props.entry.rank] ?? props.entry.rank);
    const hasRankEmoji = computed(() => props.entry.rank in rankEmojis);

    const rankVariant = computed<RankVariant>(() => {
        if (props.entry.isCurrentUser) return 'currentUser';
        if (props.entry.rank === 1) return 'gold';
        if (props.entry.rank === 2) return 'silver';
        if (props.entry.rank === 3) return 'bronze';
        return 'default';
    });

    const variantClasses: Record<RankVariant, string> = {
        gold: 'border-l-4 border-l-warning-500 bg-warning-900/20',
        silver: 'border-l-4 border-l-primary-400 bg-primary-800/30',
        bronze: 'border-l-4 border-l-orange-500 bg-orange-900/20',
        currentUser: 'border-l-4 border-l-accent-500 bg-accent-700/15',
        default: 'bg-primary-800/40',
    };

    const containerClasses = computed(() =>
        twMerge(
            'flex items-center gap-3 md:gap-4 px-4 py-3 rounded-2xl transition-colors',
            variantClasses[rankVariant.value],
        ),
    );

    const formattedXp = computed(() => props.entry.xp.toLocaleString());

    const rankDisplayClass = computed(() =>
        hasRankEmoji.value ? 'text-base' : 'text-sm text-primary-400',
    );
</script>

<template>
    <div :class="containerClasses">
        <div class="flex items-center gap-3 shrink-0">
            <span
                class="font-bold w-6 text-center shrink-0"
                :class="rankDisplayClass"
            >
                {{ rankDisplay }}
            </span>

            <BaseAvatar :emoji="entry.avatarEmoji" />
        </div>

        <div class="flex flex-col min-w-0 flex-1">
            <span class="font-bold text-white text-sm md:text-base truncate">
                {{ entry.name }}
                <span v-if="entry.isCurrentUser" class="text-accent-500">(You)</span>
            </span>
            <span class="text-xs md:text-sm text-primary-400 truncate">
                Level {{ entry.level }} &bull; {{ entry.title }}
            </span>
        </div>

        <div class="flex flex-col items-end shrink-0">
            <span class="text-lg md:text-xl font-bold text-white">{{ formattedXp }}</span>
            <span class="text-[10px] text-primary-500 uppercase tracking-wide">XP</span>
        </div>
    </div>
</template>
