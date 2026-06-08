<template>
    <BaseBox>
        <BaseHeader tag="h3">{{ title }}</BaseHeader>
        <div
            v-if="hasAchievements"
            class="grid grid-cols-2 gap-4"
            :class="columnClass"
        >
            <BaseAchievementCard
                v-for="achievement in achievements"
                :key="achievement.label"
                :emoji="achievement.emoji"
                :label="achievement.label"
                :variant="achievement.variant"
                :subtitle="achievement.subtitle"
                :locked="achievement.locked"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🏅"
            title="No achievements yet"
            message="Keep completing quests to unlock your first badge."
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseAchievementCard from '@/components/base/BaseAchievementCard.vue';
    import type { AchievementCardVariant } from '@/components/base/BaseAchievementCard.vue';

    export interface Achievement {
        emoji: string;
        label: string;
        variant: AchievementCardVariant;
        subtitle?: string;
        locked?: boolean;
    }

    type ColumnCount = 3 | 4 | 5;

    interface BaseAchievementGridProps {
        title: string;
        achievements: Achievement[];
        columns?: ColumnCount;
    }

    const props = withDefaults(defineProps<BaseAchievementGridProps>(), {
        columns: 4,
    });

    const columnClasses: Record<ColumnCount, string> = {
        3: 'md:grid-cols-3',
        4: 'md:grid-cols-4',
        5: 'md:grid-cols-5',
    };

    const columnClass = computed(() => columnClasses[props.columns]);
    const hasAchievements = computed(() => props.achievements.length > 0);
</script>
