<template>
    <BaseBox>
        <BaseHeader tag="h3">Streak Achievements</BaseHeader>
        <div class="grid grid-cols-4 gap-4">
            <BaseAchievementCard
                v-for="achievement in achievements"
                :key="achievement.label"
                :emoji="achievement.emoji"
                :label="achievement.label"
                :variant="achievement.variant"
                :subtitle="achievement.subtitle"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseAchievementCard from '@/components/base/BaseAchievementCard.vue';
    import type { AchievementCardVariant } from '@/components/base/BaseAchievementCard.vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';

    export interface ContributionAchievement {
        emoji: string;
        label: string;
        subtitle: string;
        variant: AchievementCardVariant;
    }

    interface ContributionAchievementsProps {
        achievements: ContributionAchievement[];
    }

    defineProps<ContributionAchievementsProps>();
</script>
