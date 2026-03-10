<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CommunityPartyQuestCard from './CommunityPartyQuestCard.vue';
    import type { PartyQuest } from './CommunityPartyQuestCard.vue';

    interface Props {
        quests: PartyQuest[];
    }

    defineProps<Props>();
</script>

<template>
    <BaseBox>
        <BaseHeader tag="h3">Party Quests</BaseHeader>

        <div class="flex flex-col gap-4">
            <CommunityPartyQuestCard
                v-for="quest in quests"
                :key="quest.title"
                :quest="quest"
            />
        </div>
    </BaseBox>
</template>
