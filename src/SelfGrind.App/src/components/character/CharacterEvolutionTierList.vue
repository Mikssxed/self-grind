<template>
    <BaseBox>
        <BaseHeader tag="h3">Evolution Path</BaseHeader>
        <div class="flex flex-col gap-3">
            <CharacterEvolutionTierItem
                v-for="tier in tiers"
                :key="tier.name"
                :name="tier.name"
                :level-range="tier.levelRange"
                :status="tier.status"
                :emoji="tier.emoji"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterEvolutionTierItem from './CharacterEvolutionTierItem.vue';
    import type { EvolutionTier } from './CharacterEvolutionTierItem.vue';

    interface CharacterEvolutionTierListProps {
        tiers: EvolutionTier[];
    }

    defineProps<CharacterEvolutionTierListProps>();
</script>
