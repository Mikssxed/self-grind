<template>
    <div
        class="flex flex-col items-center gap-3 p-5 rounded-2xl bg-primary-900 border-2 transition-all duration-300"
        :class="borderClass"
    >
        <div
            class="flex items-center justify-center w-14 h-14 rounded-xl text-3xl"
            :class="iconBgClass"
        >
            {{ emoji }}
        </div>
        <div class="flex flex-col items-center gap-1">
            <span class="text-sm font-bold text-white text-center">{{ name }}</span>
            <span
                class="text-xs font-bold"
                :class="bonusTextClass"
            >
                {{ bonus }}
            </span>
        </div>
        <BaseButton
            :variant="equipButtonVariant"
            size="sm"
            :disabled="disableEquipAction"
            class="w-full text-center"
            @click="toggleEquip"
        >
            {{ equipButtonLabel }}
        </BaseButton>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import type { ItemRarity } from '@/api/apiClient/models';
    import { ItemRarityObject } from '@/api/apiClient/models';

    interface CharacterInventoryCardProps {
        name: string;
        bonus: string;
        emoji: string;
        rarity: ItemRarity;
        isEquipped: boolean;
        isMutating?: boolean;
    }

    const props = withDefaults(defineProps<CharacterInventoryCardProps>(), {
        isMutating: false,
    });

    const emit = defineEmits<{
        equip: [];
        unequip: [];
    }>();

    const borderClasses: Record<ItemRarity, string> = {
        [ItemRarityObject.Common]: 'border-primary-600',
        [ItemRarityObject.Uncommon]: 'border-success-500/50',
        [ItemRarityObject.Rare]: 'border-info-500/50',
        [ItemRarityObject.Epic]: 'border-violet-500/50',
        [ItemRarityObject.Legendary]: 'border-warning-500/50',
    };

    const iconBgClasses: Record<ItemRarity, string> = {
        [ItemRarityObject.Common]: 'bg-primary-800',
        [ItemRarityObject.Uncommon]: 'bg-success-500/20',
        [ItemRarityObject.Rare]: 'bg-info-500/20',
        [ItemRarityObject.Epic]: 'bg-violet-500/20',
        [ItemRarityObject.Legendary]: 'bg-warning-500/20',
    };

    const bonusTextClasses: Record<ItemRarity, string> = {
        [ItemRarityObject.Common]: 'text-primary-400',
        [ItemRarityObject.Uncommon]: 'text-success-500',
        [ItemRarityObject.Rare]: 'text-info-500',
        [ItemRarityObject.Epic]: 'text-violet-500',
        [ItemRarityObject.Legendary]: 'text-warning-500',
    };

    const borderClass = computed(() => borderClasses[props.rarity]);
    const iconBgClass = computed(() => iconBgClasses[props.rarity]);
    const bonusTextClass = computed(() => bonusTextClasses[props.rarity]);
    const equipButtonVariant = computed(() => (props.isEquipped ? 'secondary' : 'primary'));
    const equipButtonLabel = computed(() => (props.isEquipped ? 'Unequip' : 'Equip'));
    const disableEquipAction = computed(() => props.isMutating);

    function toggleEquip() {
        if (props.isEquipped) {
            emit('unequip');
        } else {
            emit('equip');
        }
    }
</script>
