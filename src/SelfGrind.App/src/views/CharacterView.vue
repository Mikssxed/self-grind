<script setup lang="ts">
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import CharacterEvolutionHero from '@/components/character/CharacterEvolutionHero.vue';
    import CharacterEvolutionTierList from '@/components/character/CharacterEvolutionTierList.vue';
    import CharacterSkillTrees from '@/components/character/CharacterSkillTrees.vue';
    import CharacterEquippedItems from '@/components/character/CharacterEquippedItems.vue';
    import CharacterItemInventory from '@/components/character/CharacterItemInventory.vue';
    import type { HeroStat } from '@/components/character/CharacterEvolutionHero.vue';
    import type { EvolutionTier } from '@/components/character/CharacterEvolutionTierItem.vue';
    import type { SkillCategory } from '@/components/character/CharacterSkillCategory.vue';
    import type { EquippedItem } from '@/components/character/CharacterEquippedItemRow.vue';
    import type { InventoryItem } from '@/components/character/CharacterInventoryCard.vue';

    const heroStats: HeroStat[] = [
        { emoji: '🛡️', label: 'Defense', value: '+45', variant: 'info' },
        { emoji: '⚔️', label: 'Attack', value: '+38', variant: 'default' },
        { emoji: '🧠', label: 'Intelligence', value: '+52', variant: 'violet' },
        { emoji: '💚', label: 'Vitality', value: '+41', variant: 'info' },
    ];

    const evolutionTiers: EvolutionTier[] = [
        { name: 'Novice', levelRange: 'Level 1-10', status: 'completed', emoji: '🌱' },
        { name: 'Apprentice', levelRange: 'Level 11-20', status: 'completed', emoji: '📖' },
        { name: 'Adept', levelRange: 'Level 21-30', status: 'completed', emoji: '⚡' },
        { name: 'Expert', levelRange: 'Level 31-40', status: 'current', emoji: '🔥' },
        { name: 'Master', levelRange: 'Level 41-50', status: 'locked', emoji: '👑' },
    ];

    const skillCategories: SkillCategory[] = [
        {
            name: 'Strength',
            emoji: '💪',
            variant: 'error',
            skills: [
                { name: 'Warrior I', status: 'unlocked', emoji: '⚔️', description: 'Complete 10 workouts' },
                { name: 'Warrior II', status: 'unlocked', emoji: '🗡️', description: 'Complete 50 workouts' },
                { name: 'Warrior III', status: 'locked', emoji: '🛡️', description: 'Complete 100 workouts' },
                { name: 'Titan', status: 'locked', emoji: '💥', description: 'Complete 250 workouts' },
            ],
        },
        {
            name: 'Knowledge',
            emoji: '📘',
            variant: 'info',
            skills: [
                { name: 'Scholar I', status: 'unlocked', emoji: '📖', description: 'Read 5 books' },
                { name: 'Scholar II', status: 'unlocked', emoji: '📚', description: 'Read 20 books' },
                { name: 'Scholar III', status: 'locked', emoji: '🎓', description: 'Read 50 books' },
                { name: 'Sage', status: 'locked', emoji: '🧙', description: 'Read 100 books' },
            ],
        },
        {
            name: 'Focus',
            emoji: '👁️',
            variant: 'success',
            skills: [
                { name: 'Monk I', status: 'unlocked', emoji: '🧘', description: 'Meditate for 7 days' },
                { name: 'Monk II', status: 'locked', emoji: '🌿', description: 'Meditate for 30 days' },
                { name: 'Monk III', status: 'locked', emoji: '☯️', description: 'Meditate for 90 days' },
                { name: 'Zen Master', status: 'locked', emoji: '✨', description: 'Meditate for 365 days' },
            ],
        },
    ];

    const equippedItems: EquippedItem[] = [
        { name: 'Power Gauntlets', type: 'Gloves', bonus: '+20 Strength', emoji: '🧤', variant: 'error' },
        { name: 'Wisdom Scroll', type: 'Artifact', bonus: '+15 Knowledge XP', emoji: '📜', variant: 'info' },
        { name: 'Focus Amulet', type: 'Artifact', bonus: '+25 Focus XP', emoji: '📿', variant: 'violet' },
    ];

    const inventoryItems: InventoryItem[] = [
        { name: 'Health Potion', bonus: '+10% Energy', emoji: '🧪', rarity: 'common', isEquipped: false },
        { name: 'Wisdom Scroll', bonus: '+15 Knowledge XP', emoji: '📜', rarity: 'uncommon', isEquipped: true },
        { name: 'Focus Amulet', bonus: '+25 Focus XP', emoji: '📿', rarity: 'rare', isEquipped: true },
        { name: 'Power Band', bonus: '+20% Strength', emoji: '💎', rarity: 'epic', isEquipped: false },
        { name: 'Power Gauntlets', bonus: '+20 Strength', emoji: '🧤', rarity: 'epic', isEquipped: true },
        { name: 'Life Amulet', bonus: '+30 Vitality', emoji: '💚', rarity: 'rare', isEquipped: false },
        { name: 'XP Booster', bonus: '+50% XP Gain', emoji: '⭐', rarity: 'legendary', isEquipped: false },
        { name: 'Energy Crystal', bonus: '+22 Energy', emoji: '⚡', rarity: 'uncommon', isEquipped: false },
    ];
</script>

<template>
    <div class="flex flex-col gap-6 p-4 md:p-8 flex-1 max-h-screen overflow-y-auto">
        <!-- Page Header -->
        <div class="flex flex-col gap-1">
            <BaseHeader tag="h1">Character Upgrade ⚔️</BaseHeader>
            <BaseText>Evolve your character, unlock skills, and equip powerful items</BaseText>
        </div>

        <!-- Character Evolution: Hero + Tier List -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <CharacterEvolutionHero
                class="md:col-span-2"
                :level="42"
                stage-name="Elite Warrior"
                next-evolution="Level 50: Legendary Champion"
                :xp-current="8450"
                :xp-max="10000"
                :stats="heroStats"
            />
            <CharacterEvolutionTierList :tiers="evolutionTiers" />
        </div>

        <!-- Skill Trees -->
        <CharacterSkillTrees :categories="skillCategories" />

        <!-- Equipment: Equipped Items + Inventory -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <CharacterEquippedItems :items="equippedItems" />
            <CharacterItemInventory
                class="md:col-span-2"
                :items="inventoryItems"
            />
        </div>
    </div>
</template>
