<template>
    <BaseBox>
        <BaseHeader tag="h3">Achievement Progress</BaseHeader>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
            <BaseAchievementCard
                v-for="achievement in achievements"
                :key="achievement.label"
                :emoji="achievement.emoji"
                :label="achievement.label"
                :subtitle="achievement.subtitle"
                :variant="achievement.variant"
                :locked="achievement.locked"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseAchievementCard from '@/components/base/BaseAchievementCard.vue';
    import type { AchievementCardVariant } from '@/components/base/BaseAchievementCard.vue';

    export interface AnalyticsAchievement {
        emoji: string;
        label: string;
        subtitle: string;
        variant: AchievementCardVariant;
        locked: boolean;
    }

    interface AnalyticsAchievementsProps {
        achievements: AnalyticsAchievement[];
    }

    defineProps<AnalyticsAchievementsProps>();
</script>
