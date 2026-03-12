<template>
    <div class="flex flex-col gap-2">
        <InputLabel :name="name">
            {{ displayLabel }}
        </InputLabel>
        <input
            type="range"
            v-model.number="value"
            :min="min"
            :max="max"
            :step="step"
            :name="name"
            :style="trackStyle"
            class="range-slider w-full h-6 appearance-none cursor-pointer bg-transparent"
        />
        <div class="flex justify-between text-sm text-primary-400">
            <span>{{ min }} {{ unit }}</span>
            <span>{{ max }} {{ unit }}</span>
        </div>
    </div>
</template>
<script setup lang="ts">
    import { useField } from 'vee-validate';
    import { computed } from 'vue';
    import InputLabel from './InputLabel.vue';

    export interface RangeSliderFieldProps {
        name: string;
        label: string;
        min?: number;
        max?: number;
        step?: number;
        unit?: string;
    }

    const props = withDefaults(defineProps<RangeSliderFieldProps>(), {
        min: 0,
        max: 100,
        step: 1,
        unit: '',
    });

    const { value } = useField<number>(() => props.name);

    const displayLabel = computed(() => {
        const currentValue = value.value ?? props.min;
        return `${props.label}: ${currentValue}`;
    });

    const progressPercent = computed(() => {
        const currentValue = value.value ?? props.min;
        return ((currentValue - props.min) / (props.max - props.min)) * 100;
    });

    const trackStyle = computed(() => ({
        '--range-progress': `${progressPercent.value}%`,
    }));
</script>
