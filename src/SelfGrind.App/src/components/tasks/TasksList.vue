<script lang="ts" setup>
    import { computed } from 'vue';
    import type { TaskItemDto } from '@/api/apiClient/models';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import TasksListItem from './TasksListItem.vue';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';

    const props = defineProps<{ tasks: TaskItemDto[] }>();

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
                :title="item.title"
                :description="item.description"
                :xp="item.xp"
                :attribute="item.attribute"
                :attributeEmoji="item.attributeEmoji"
                :variant="item.variant"
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
