<template>
    <BaseBox>
        <BaseHeader tag="h3">🥇 Recent Achievements</BaseHeader>
        <div class="grid grid-cols-5 gap-4">
            <DashboardAchievementCard
                v-for="achievement in achievements"
                :key="achievement.label"
                :emoji="achievement.emoji"
                :label="achievement.label"
                :variant="achievement.variant"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import DashboardAchievementCard from './DashboardAchievementCard.vue';
    import type { AchievementCardVariant } from './DashboardAchievementCard.vue';

    export interface Achievement {
        emoji: string;
        label: string;
        variant: AchievementCardVariant;
    }

    interface DashboardRecentAchievementsProps {
        achievements: Achievement[];
    }

    defineProps<DashboardRecentAchievementsProps>();
</script>
