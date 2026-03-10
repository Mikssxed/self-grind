<script setup lang="ts">
    import { computed } from 'vue';
    import BaseAvatar from '@/components/base/BaseAvatar.vue';

    export type FriendStatus = 'online' | 'offline';

    export interface Friend {
        name: string;
        level: number;
        status: FriendStatus;
        lastSeen?: string;
        avatarEmoji: string;
    }

    interface Props {
        friend: Friend;
    }

    const props = defineProps<Props>();

    const isOnline = computed(() => props.friend.status === 'online');

    const statusText = computed(() =>
        isOnline.value ? 'Online' : props.friend.lastSeen ?? 'Offline',
    );

    const statusClass = computed(() =>
        isOnline.value ? 'text-xs font-bold text-success-500' : 'text-xs text-primary-400',
    );
</script>

<template>
    <div class="flex items-center gap-3 px-4 py-3 rounded-2xl bg-primary-800/40">
        <BaseAvatar :emoji="friend.avatarEmoji" />

        <div class="flex flex-col min-w-0 flex-1">
            <span class="font-bold text-white text-sm truncate">{{ friend.name }}</span>
            <span class="text-xs text-primary-400">Level {{ friend.level }}</span>
        </div>

        <span :class="statusClass">{{ statusText }}</span>
    </div>
</template>
