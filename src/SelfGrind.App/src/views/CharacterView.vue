<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import CharacterEvolutionHero from '@/components/character/CharacterEvolutionHero.vue';
    import CharacterEvolutionTierList from '@/components/character/CharacterEvolutionTierList.vue';
    import CharacterSkillTrees from '@/components/character/CharacterSkillTrees.vue';
    import CharacterEquippedItems from '@/components/character/CharacterEquippedItems.vue';
    import CharacterItemInventory from '@/components/character/CharacterItemInventory.vue';
    import {
        useCharacterHero,
        useEquipItem,
        useEquippedItems,
        useEvolutionTiers,
        useInventory,
        useSkillTrees,
        useUnequipItem,
    } from '@/composables/useCharacter';
    import { computed } from 'vue';

    const { hero } = useCharacterHero();
    const { tiers } = useEvolutionTiers();
    const { skillCategories } = useSkillTrees();
    const { inventory } = useInventory();
    const { equippedItems } = useEquippedItems();
    const equipMutation = useEquipItem();
    const unequipMutation = useUnequipItem();

    const heroLevel = computed(() => hero.value?.level ?? 1);
    const heroStageName = computed(() => hero.value?.stageName ?? '');
    const heroNextEvolutionLabel = computed(() => hero.value?.nextEvolutionLabel ?? null);
    const heroExp = computed(() => hero.value?.exp ?? 0);
    const heroRequiredExp = computed(() => hero.value?.requiredExp ?? 0);
    const heroStats = computed(() => hero.value?.stats ?? []);

    const mutatingItemId = computed(() => {
        if (equipMutation.isPending.value) return equipMutation.variables.value ?? undefined;
        if (unequipMutation.isPending.value) return unequipMutation.variables.value ?? undefined;
        return undefined;
    });

    function handleEquip(userItemId: string) {
        equipMutation.mutate(userItemId);
    }

    function handleUnequip(userItemId: string) {
        unequipMutation.mutate(userItemId);
    }
</script>

<template>
    <PageLayout
        title="Character Upgrade ⚔️"
        subtitle="Evolve your character, unlock skills, and equip powerful items"
    >
        <!-- Character Evolution: Hero + Tier List -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <CharacterEvolutionHero
                class="md:col-span-2"
                :level="heroLevel"
                :stage-name="heroStageName"
                :next-evolution-label="heroNextEvolutionLabel"
                :xp-current="heroExp"
                :xp-max="heroRequiredExp"
                :stats="heroStats"
            />
            <CharacterEvolutionTierList :tiers="tiers" />
        </div>

        <!-- Skill Trees -->
        <CharacterSkillTrees :categories="skillCategories" />

        <!-- Equipment: Equipped Items + Inventory -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <CharacterEquippedItems :items="equippedItems" />
            <CharacterItemInventory
                class="md:col-span-2"
                :items="inventory"
                :mutating-item-id="mutatingItemId"
                @equip="handleEquip"
                @unequip="handleUnequip"
            />
        </div>
    </PageLayout>
</template>
