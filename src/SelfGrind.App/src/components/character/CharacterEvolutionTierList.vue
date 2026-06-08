<template>
    <BaseBox>
        <BaseHeader tag="h3">Evolution Path</BaseHeader>
        <div v-if="hasTiers" class="flex flex-col gap-3">
            <CharacterEvolutionTierItem
                v-for="tier in tiers"
                :key="tier.name"
                :name="tier.name"
                :level-range="tier.levelRange"
                :status="tier.status"
                :emoji="tier.emoji"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🪜"
            title="No evolution path defined"
            message="Tiers will appear once configured."
            size="sm"
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterEvolutionTierItem from './CharacterEvolutionTierItem.vue';
    import type { EvolutionTierDto } from '@/api/apiClient/models';

    interface CharacterEvolutionTierListProps {
        tiers: EvolutionTierDto[];
    }

    const props = defineProps<CharacterEvolutionTierListProps>();
    const hasTiers = computed(() => props.tiers.length > 0);
</script>
