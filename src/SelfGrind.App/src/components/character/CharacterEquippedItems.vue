<template>
    <BaseBox>
        <BaseHeader tag="h3">Equipped Items</BaseHeader>
        <div class="flex flex-col gap-3">
            <CharacterEquippedItemRow
                v-for="item in items"
                :key="item.name"
                :name="item.name"
                :type="item.type"
                :bonus="item.bonus"
                :emoji="item.emoji"
                :variant="item.variant"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterEquippedItemRow from './CharacterEquippedItemRow.vue';
    import type { EquippedItem } from './CharacterEquippedItemRow.vue';

    interface CharacterEquippedItemsProps {
        items: EquippedItem[];
    }

    defineProps<CharacterEquippedItemsProps>();
</script>
