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
            :variant="isEquipped ? 'secondary' : 'primary'"
            size="sm"
            :disabled="isEquipped"
            class="w-full text-center"
        >
            {{ isEquipped ? 'Equipped' : 'Equip' }}
        </BaseButton>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseButton from '@/components/base/BaseButton.vue';

    export type InventoryItemRarity = 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';

    export interface InventoryItem {
        name: string;
        bonus: string;
        emoji: string;
        rarity: InventoryItemRarity;
        isEquipped: boolean;
    }

    interface CharacterInventoryCardProps {
        name: string;
        bonus: string;
        emoji: string;
        rarity: InventoryItemRarity;
        isEquipped: boolean;
    }

    const props = defineProps<CharacterInventoryCardProps>();

    const borderClasses: Record<InventoryItemRarity, string> = {
        common: 'border-primary-600',
        uncommon: 'border-success-500/50',
        rare: 'border-info-500/50',
        epic: 'border-violet-500/50',
        legendary: 'border-warning-500/50',
    };

    const iconBgClasses: Record<InventoryItemRarity, string> = {
        common: 'bg-primary-800',
        uncommon: 'bg-success-500/20',
        rare: 'bg-info-500/20',
        epic: 'bg-violet-500/20',
        legendary: 'bg-warning-500/20',
    };

    const bonusTextClasses: Record<InventoryItemRarity, string> = {
        common: 'text-primary-400',
        uncommon: 'text-success-500',
        rare: 'text-info-500',
        epic: 'text-violet-500',
        legendary: 'text-warning-500',
    };

    const borderClass = computed(() => borderClasses[props.rarity]);
    const iconBgClass = computed(() => iconBgClasses[props.rarity]);
    const bonusTextClass = computed(() => bonusTextClasses[props.rarity]);
</script>
