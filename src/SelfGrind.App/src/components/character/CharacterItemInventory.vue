<template>
    <BaseBox>
        <BaseHeader tag="h3">Item Inventory</BaseHeader>
        <div v-if="hasItems" class="grid grid-cols-2 md:grid-cols-4 gap-4">
            <CharacterInventoryCard
                v-for="item in items"
                :key="item.userItemId"
                :name="item.name"
                :bonus="item.bonus"
                :emoji="item.emoji"
                :rarity="item.rarity"
                :is-equipped="item.isEquipped"
                :is-mutating="mutatingItemId === item.userItemId"
                @equip="onEquip(item.userItemId)"
                @unequip="onUnequip(item.userItemId)"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🎒"
            title="Your inventory is empty"
            message="Complete tasks and level up to unlock new items."
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterInventoryCard from './CharacterInventoryCard.vue';
    import type { InventoryItemDto } from '@/api/apiClient/models';

    interface CharacterItemInventoryProps {
        items: InventoryItemDto[];
        mutatingItemId?: string;
    }

    const props = defineProps<CharacterItemInventoryProps>();
    const hasItems = computed(() => props.items.length > 0);

    const emit = defineEmits<{
        equip: [userItemId: string];
        unequip: [userItemId: string];
    }>();

    function onEquip(userItemId: string) {
        emit('equip', userItemId);
    }

    function onUnequip(userItemId: string) {
        emit('unequip', userItemId);
    }
</script>
