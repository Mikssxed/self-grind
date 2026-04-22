<script lang="ts" setup>
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import DailyTasksQuestItem from './DailyTasksQuestItem.vue';
    import { useAddTaskModal } from '@/composables/useAddTaskModal';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import { TaskOccurrenceStatusObject, type TodayTaskItemDto } from '@/api/apiClient/models';

    const { open: openAddTask } = useAddTaskModal();

    const props = defineProps<{ items: TodayTaskItemDto[] }>();
    const emit = defineEmits<{ toggle: [occurrenceId: string] }>();

    const displayItems = computed(() =>
        props.items.map(item => {
            const { label, emoji, variant } = getAttributeDisplay(item.attribute);
            return {
                occurrenceId: item.occurrenceId,
                title: item.title,
                description: item.description,
                xp: item.exp,
                attribute: label,
                attributeEmoji: emoji,
                variant,
                completed: item.occurrenceStatus === TaskOccurrenceStatusObject.Done,
            };
        })
    );
</script>

<template>
    <BaseBox>
        <div class="flex items-center justify-between">
            <BaseHeader tag="h3">Today's Quests</BaseHeader>
            <BaseButton
                size="sm"
                @click="openAddTask"
            >
                + Add Task
            </BaseButton>
        </div>
        <div class="flex flex-col gap-3">
            <DailyTasksQuestItem
                v-for="item in displayItems"
                :key="item.occurrenceId"
                :title="item.title"
                :description="item.description"
                :xp="item.xp"
                :attribute="item.attribute"
                :attributeEmoji="item.attributeEmoji"
                :variant="item.variant"
                :completed="item.completed"
                @toggle="emit('toggle', item.occurrenceId)"
            />
        </div>
    </BaseBox>
</template>
