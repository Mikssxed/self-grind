<template>
    <div class="flex flex-col gap-2">
        <InputLabel
            v-if="label"
            :name="name"
        >
            {{ label }}
        </InputLabel>
        <div class="flex gap-2">
            <button
                v-for="option in options"
                :key="option.value"
                type="button"
                :class="optionClass(option.value)"
                @click="value = option.value"
            >
                {{ option.label }}
            </button>
        </div>
    </div>
</template>
<script setup lang="ts">
    import { useField } from 'vee-validate';
    import InputLabel from './InputLabel.vue';

    export interface ToggleOption {
        label: string;
        value: string;
    }

    export interface ToggleButtonGroupProps {
        name: string;
        options: ToggleOption[];
        label?: string;
    }

    const props = defineProps<ToggleButtonGroupProps>();

    const { value } = useField<string>(() => props.name);

    const baseClasses = 'px-6 py-2 rounded-full text-sm font-semibold transition-all cursor-pointer';
    const activeClasses = 'button-primary text-white';
    const inactiveClasses = 'bg-primary-800 text-primary-400 hover:text-white';

    function optionClass(optionValue: string) {
        const isActive = value.value === optionValue;
        return [baseClasses, isActive ? activeClasses : inactiveClasses];
    }
</script>
