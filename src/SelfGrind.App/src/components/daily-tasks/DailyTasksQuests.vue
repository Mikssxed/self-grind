<script lang="ts" setup>
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import DailyTasksQuestItem, { type QuestItemVariant } from './DailyTasksQuestItem.vue';

    export interface Quest {
        title: string;
        description: string;
        xp: number;
        attribute: string;
        attributeEmoji: string;
        variant: QuestItemVariant;
        completed: boolean;
    }

    interface DailyTasksQuestsProps {
        quests: Quest[];
    }

    defineProps<DailyTasksQuestsProps>();
</script>

<template>
    <BaseBox>
        <div class="flex items-center justify-between">
            <BaseHeader tag="h3">Today's Quests</BaseHeader>
            <BaseButton size="sm">+ Add Task</BaseButton>
        </div>
        <div class="flex flex-col gap-3">
            <DailyTasksQuestItem
                v-for="quest in quests"
                :key="quest.title"
                :title="quest.title"
                :description="quest.description"
                :xp="quest.xp"
                :attribute="quest.attribute"
                :attributeEmoji="quest.attributeEmoji"
                :variant="quest.variant"
                :completed="quest.completed"
            />
        </div>
    </BaseBox>
</template>
