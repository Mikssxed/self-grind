<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseIcon from '@/components/base/BaseIcon.vue';
    import CommunityFriendItem from './CommunityFriendItem.vue';
    import type { Friend } from './CommunityFriendItem.vue';

    interface Props {
        friends: Friend[];
    }

    defineProps<Props>();
</script>

<template>
    <BaseBox class="h-full flex flex-col">
        <BaseHeader tag="h3">Your Friends</BaseHeader>

        <div class="flex flex-col gap-2 flex-1">
            <CommunityFriendItem
                v-for="friend in friends"
                :key="friend.name"
                :friend="friend"
            />
        </div>

        <BaseButton variant="primary" size="sm" class="w-full mt-2">
            <span class="flex items-center justify-center gap-2">
                <BaseIcon name="addUser" class="w-4 h-4" />
                Add Friends
            </span>
        </BaseButton>
    </BaseBox>
</template>
