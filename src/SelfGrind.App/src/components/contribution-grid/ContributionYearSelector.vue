<template>
    <div class="flex flex-col gap-2">
        <button
            v-for="year in years"
            :key="year"
            :class="year === selectedYear ? selectedClass : defaultClass"
            @click="$emit('update:selectedYear', year)"
        >
            {{ year }}
        </button>
    </div>
</template>
<script setup lang="ts">
    const selectedClass = 'px-3 py-1.5 rounded-lg bg-gradient-purple text-white text-sm font-bold';
    const defaultClass =
        'px-3 py-1.5 rounded-lg text-primary-400 text-sm font-medium hover:text-white transition-colors';

    interface ContributionYearSelectorProps {
        years: number[];
        selectedYear: number;
    }

    defineProps<ContributionYearSelectorProps>();

    defineEmits<{
        'update:selectedYear': [year: number];
    }>();
</script>
