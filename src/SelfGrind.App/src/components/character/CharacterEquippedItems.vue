<template>
    <BaseBox>
        <BaseHeader tag="h3">Equipped Items</BaseHeader>
        <div v-if="hasItems" class="flex flex-col gap-3">
            <CharacterEquippedItemRow
                v-for="item in items"
                :key="item.userItemId"
                :name="item.name"
                :type="item.type"
                :bonus="item.bonus"
                :emoji="item.emoji"
                :variant="item.variant"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🎽"
            title="Nothing equipped yet"
            message="Equip items from your inventory to power up your character."
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterEquippedItemRow from './CharacterEquippedItemRow.vue';
    import type { EquippedItemDto } from '@/api/apiClient/models';

    interface CharacterEquippedItemsProps {
        items: EquippedItemDto[];
    }

    const props = defineProps<CharacterEquippedItemsProps>();
    const hasItems = computed(() => props.items.length > 0);
</script>
