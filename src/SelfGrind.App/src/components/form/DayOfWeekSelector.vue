<template>
    <div class="flex flex-col gap-2">
        <InputLabel :name="name">
            {{ label }}
        </InputLabel>
        <div class="flex gap-2">
            <button
                v-for="day in days"
                :key="day.value"
                type="button"
                :class="dayClass(day.value)"
                @click="toggleDay(day.value)"
            >
                {{ day.label }}
            </button>
        </div>
        <p
            v-if="errorMessage"
            class="text-xs text-error-500"
        >
            {{ errorMessage }}
        </p>
    </div>
</template>
<script setup lang="ts">
    import { useField } from 'vee-validate';
    import InputLabel from './InputLabel.vue';

    export interface DayOfWeekSelectorProps {
        name: string;
        label?: string;
    }

    const props = withDefaults(defineProps<DayOfWeekSelectorProps>(), {
        label: 'Repeat on',
    });

    const { value, errorMessage } = useField<number[]>(() => props.name);

    const days: { label: string; value: number }[] = [
        { label: 'Mon', value: 1 },
        { label: 'Tue', value: 2 },
        { label: 'Wed', value: 3 },
        { label: 'Thu', value: 4 },
        { label: 'Fri', value: 5 },
        { label: 'Sat', value: 6 },
        { label: 'Sun', value: 0 },
    ];

    const baseClasses = 'flex-1 py-2 rounded-xl border-2 text-sm font-semibold cursor-pointer transition-all text-center';
    const activeClasses = 'border-accent-500 bg-accent-500/10 text-white';
    const inactiveClasses = 'border-primary-800 bg-primary-900 text-primary-400 hover:border-primary-600';

    function dayClass(dayValue: number) {
        const isActive = value.value?.includes(dayValue) ?? false;
        return [baseClasses, isActive ? activeClasses : inactiveClasses];
    }

    function toggleDay(dayValue: number) {
        const current = value.value ?? [];
        if (current.includes(dayValue)) {
            value.value = current.filter((d) => d !== dayValue);
        } else {
            value.value = [...current, dayValue];
        }
    }
</script>
