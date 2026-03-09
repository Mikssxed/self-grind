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

    export type EquippedItemVariant = 'error' | 'info' | 'violet' | 'warning' | 'success';

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

    const borderClasses: Record<EquippedItemVariant, string> = {
        error: 'border-l-error-500',
        info: 'border-l-info-500',
        violet: 'border-l-violet-500',
        warning: 'border-l-warning-500',
        success: 'border-l-success-500',
    };

    const bonusTextClasses: Record<EquippedItemVariant, string> = {
        error: 'text-error-500',
        info: 'text-info-500',
        violet: 'text-violet-500',
        warning: 'text-warning-500',
        success: 'text-success-500',
    };

    const borderClass = computed(() => borderClasses[props.variant]);
    const bonusTextClass = computed(() => bonusTextClasses[props.variant]);
</script>
