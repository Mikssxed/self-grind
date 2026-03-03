<template>
    <BaseBox>
        <div class="flex justify-between items-center">
            <BaseHeader tag="h3">📋 Daily Quest</BaseHeader>
            <BaseBadge>+{{ xpReward }} XP</BaseBadge>
        </div>

        <BaseQuoteBlock
            :quote="quote"
            :author="author"
        />

        <div class="grid grid-cols-4 gap-4">
            <DashboardActionCard
                v-for="action in actions"
                :key="action.label"
                :emoji="action.emoji"
                :label="action.label"
                :borderVariant="action.borderVariant"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBadge from '@/components/base/BaseBadge.vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseQuoteBlock from '@/components/base/BaseQuoteBlock.vue';
    import DashboardActionCard from './DashboardActionCard.vue';
    import type { ActionCardBorderVariant } from './DashboardActionCard.vue';

    export interface DailyQuestAction {
        emoji: string;
        label: string;
        borderVariant?: ActionCardBorderVariant;
    }

    interface DashboardDailyQuestProps {
        quote: string;
        author: string;
        xpReward: number;
        actions: DailyQuestAction[];
    }

    defineProps<DashboardDailyQuestProps>();
</script>
