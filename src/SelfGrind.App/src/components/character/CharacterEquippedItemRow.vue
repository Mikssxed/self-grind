<template>
    <div
        class="flex items-center gap-4 p-4 rounded-2xl bg-primary-900 border-l-4"
        :class="borderClass"
    >
        <div class="flex items-center justify-center w-10 h-10 rounded-xl bg-primary-800 text-xl">
            {{ emoji }}
        </div>
        <div class="flex flex-col flex-1">
            <span class="text-sm font-bold text-white">{{ name }}</span>
            <span class="text-xs text-primary-400">{{ type }}</span>
        </div>
        <span
            class="text-sm font-bold"
            :class="bonusTextClass"
        >
            {{ bonus }}
        </span>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import { borderLeftClasses, textClasses } from '@/composables/useColorVariant';
    import type { ColorVariant } from '@/composables/useColorVariant';

    export type EquippedItemVariant = ColorVariant;

    export interface EquippedItem {
        name: string;
        type: string;
        bonus: string;
        emoji: string;
        variant: EquippedItemVariant;
    }

    interface CharacterEquippedItemRowProps {
        name: string;
        type: string;
        bonus: string;
        emoji: string;
        variant: EquippedItemVariant;
    }

    const props = defineProps<CharacterEquippedItemRowProps>();

    const borderClass = computed(() => borderLeftClasses[props.variant]);
    const bonusTextClass = computed(() => textClasses[props.variant]);
</script>
