<script lang="ts" setup>
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    export type HabitCardVariant = 'info' | 'success' | 'warning' | 'violet';

    interface DailyTasksHabitCardProps {
        emoji: string;
        label: string;
        value: string;
        variant: HabitCardVariant;
    }

    const props = defineProps<DailyTasksHabitCardProps>();

    const emit = defineEmits<{
        edit: [];
    }>();

    const bgClasses: Record<HabitCardVariant, string> = {
        info: 'bg-info-900/40',
        success: 'bg-success-900/40',
        warning: 'bg-warning-900/40',
        violet: 'bg-violet-900/40',
    };

    const borderClasses: Record<HabitCardVariant, string> = {
        info: 'border-b-info-500',
        success: 'border-b-success-500',
        warning: 'border-b-warning-500',
        violet: 'border-b-violet-500',
    };

    const valueClasses: Record<HabitCardVariant, string> = {
        info: 'text-info-500',
        success: 'text-success-500',
        warning: 'text-warning-500',
        violet: 'text-violet-500',
    };

    const containerClasses = computed(() => {
        return twMerge(
            'flex flex-col items-center gap-2 p-6 rounded-xl border-b-4',
            bgClasses[props.variant],
            borderClasses[props.variant]
        );
    });
</script>

<template>
    <div :class="containerClasses" class="cursor-pointer transition-all hover:scale-105" @click="emit('edit')">
        <span class="text-2xl">{{ emoji }}</span>
        <span class="text-sm text-primary-400">{{ label }}</span>
        <span :class="twMerge('text-2xl font-bold', valueClasses[variant])">{{ value }}</span>
    </div>
</template>
