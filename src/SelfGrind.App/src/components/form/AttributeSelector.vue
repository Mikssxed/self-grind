<template>
    <div class="flex flex-col gap-2">
        <InputLabel :name="name">
            {{ label }}
        </InputLabel>
        <div class="grid grid-cols-3 gap-3">
            <button
                v-for="attr in attributes"
                :key="attr.value"
                type="button"
                :class="attributeClass(attr.value)"
                @click="selectAttribute(attr.value)"
            >
                <span class="text-xl">{{ attr.emoji }}</span>
                <span class="text-xs font-semibold">{{ attr.label }}</span>
            </button>
        </div>
        <p class="text-xs text-primary-500">
            Tasks with attributes increase your character stats
        </p>
    </div>
</template>
<script setup lang="ts">
    import type { BaseAttribute } from '@/api/apiClient/models';
    import { useField } from 'vee-validate';
    import InputLabel from './InputLabel.vue';

    export interface AttributeSelectorProps {
        name: string;
        label?: string;
    }

    const props = withDefaults(defineProps<AttributeSelectorProps>(), {
        label: 'Character Attribute (optional)',
    });

    const { value } = useField<BaseAttribute | undefined>(() => props.name);

    const attributes: { label: string; emoji: string; value: BaseAttribute }[] = [
        { label: 'Strength', emoji: '💪', value: 'Strength' },
        { label: 'Knowledge', emoji: '📘', value: 'Knowledge' },
        { label: 'Health', emoji: '❤️', value: 'Health' },
        { label: 'Discipline', emoji: '🛡️', value: 'Discipline' },
        { label: 'Focus', emoji: '🎯', value: 'Focus' },
        { label: 'Energy', emoji: '⚡', value: 'Energy' },
    ];

    const baseClasses = 'flex flex-col items-center gap-1 p-3 rounded-xl border-2 cursor-pointer transition-all';
    const activeClasses = 'border-accent-500 bg-accent-500/10 text-white';
    const inactiveClasses = 'border-primary-800 bg-primary-900 text-primary-400 hover:border-primary-600';

    function attributeClass(attrValue: BaseAttribute) {
        const isActive = value.value === attrValue;
        return [baseClasses, isActive ? activeClasses : inactiveClasses];
    }

    function selectAttribute(attrValue: BaseAttribute) {
        value.value = value.value === attrValue ? undefined : attrValue;
    }
</script>
