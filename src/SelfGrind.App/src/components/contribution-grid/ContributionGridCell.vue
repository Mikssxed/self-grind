<template>
    <div
        class="rounded-sm cursor-pointer transition-all"
        style="width: var(--cell-size); height: var(--cell-size);"
        :class="[levelClass, selectedClass]"
        @click="$emit('select')"
    />
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    export type ActivityLevel = 0 | 1 | 2 | 3 | 4;

    interface ContributionGridCellProps {
        level: ActivityLevel;
        isSelected: boolean;
    }

    const props = defineProps<ContributionGridCellProps>();

    defineEmits<{
        select: [];
    }>();

    const levelClasses: Record<ActivityLevel, string> = {
        0: 'bg-primary-800',
        1: 'bg-secondary-500/25',
        2: 'bg-secondary-500/50',
        3: 'bg-secondary-500/75',
        4: 'bg-secondary-500',
    };

    const levelClass = computed(() => levelClasses[props.level]);
    const selectedClass = computed(() => (props.isSelected ? 'ring-2 ring-white' : ''));
</script>
