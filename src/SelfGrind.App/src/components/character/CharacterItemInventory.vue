<template>
    <BaseBox>
        <BaseHeader tag="h3">Item Inventory</BaseHeader>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
            <CharacterInventoryCard
                v-for="item in items"
                :key="item.name"
                :name="item.name"
                :bonus="item.bonus"
                :emoji="item.emoji"
                :rarity="item.rarity"
                :is-equipped="item.isEquipped"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterInventoryCard from './CharacterInventoryCard.vue';
    import type { InventoryItem } from './CharacterInventoryCard.vue';

    interface CharacterItemInventoryProps {
        items: InventoryItem[];
    }

    defineProps<CharacterItemInventoryProps>();
</script>
