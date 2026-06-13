<script lang="ts" setup>
    import { computed } from 'vue';
    import type { TaskItemDto } from '@/api/apiClient/models';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import TasksListItem from './TasksListItem.vue';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import { useTaskDetailModal } from '@/composables/useTaskDetailModal';

    const props = defineProps<{ tasks: TaskItemDto[] }>();

    const { open: openTaskDetail } = useTaskDetailModal();

    function onSelect(id: string) {
        const task = props.tasks.find((t) => t.id === id);
        if (task) {
            openTaskDetail(task);
        }
    }

    const displayItems = computed(() =>
        props.tasks
            .filter((task): task is TaskItemDto & { id: string } => !!task.id)
            .map(task => {
                const { label, emoji, variant } = getAttributeDisplay(task.attribute);
                return {
                    id: task.id,
                    title: task.title,
                    description: task.description,
                    xp: task.exp,
                    attribute: label,
                    attributeEmoji: emoji,
                    variant,
                };
            })
    );

    const hasItems = computed(() => displayItems.value.length > 0);
</script>

<template>
    <BaseBox>
        <BaseHeader tag="h3">All Tasks</BaseHeader>
        <div v-if="hasItems" class="flex flex-col gap-3">
            <TasksListItem
                v-for="item in displayItems"
                :key="item.id"
                :id="item.id"
                :title="item.title"
                :description="item.description"
                :xp="item.xp"
                :attribute="item.attribute"
                :attributeEmoji="item.attributeEmoji"
                :variant="item.variant"
                @select="onSelect"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="📭"
            title="No tasks yet"
            message="Create a task to see it listed here."
        />
    </BaseBox>
</template>
