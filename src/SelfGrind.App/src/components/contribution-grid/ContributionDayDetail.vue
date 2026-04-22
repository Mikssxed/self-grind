<template>
    <div class="rounded-2xl bg-primary-900 p-4 flex flex-col gap-3 min-h-48">
        <template v-if="date">
            <span class="text-sm font-bold text-white">{{ formattedDate }}</span>
            <div
                v-if="tasks.length > 0"
                class="flex flex-col gap-2"
            >
                <div
                    v-for="task in tasks"
                    :key="task.title"
                    :class="taskContainerClass(task.variant)"
                >
                    <div :class="taskCheckmarkClass(task.variant)" />
                    <span class="text-sm font-bold text-white flex-1">{{ task.title }}</span>
                    <div class="flex items-center gap-1.5">
                        <span :class="taskXpBadgeClass(task.variant)">+{{ task.xp }} XP</span>
                        <span :class="taskAttributeBadgeClass(task.variant)">{{ task.attributeEmoji }} {{ task.attribute }}</span>
                    </div>
                </div>
            </div>
            <p
                v-else
                class="text-sm text-primary-400"
            >
                No tasks completed this day.
            </p>
            <div
                v-if="tasks.length > 0"
                class="mt-auto pt-2 border-t border-primary-800"
            >
                <span class="text-xs text-primary-400">{{ taskSummary }}</span>
            </div>
        </template>
        <div
            v-else
            class="flex items-center justify-center h-full"
        >
            <p class="text-sm text-primary-400">Click a day to see details</p>
        </div>
    </div>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    export type DayTaskVariant = 'error' | 'info' | 'violet' | 'success' | 'warning';

    export interface DayTask {
        title: string;
        xp: number;
        attribute: string;
        attributeEmoji: string;
        variant: DayTaskVariant;
    }

    interface ContributionDayDetailProps {
        date: string | null;
        tasks: DayTask[];
    }

    const props = defineProps<ContributionDayDetailProps>();

    const bgClasses: Record<DayTaskVariant, string> = {
        error: 'bg-error-900/40',
        info: 'bg-info-900/40',
        violet: 'bg-violet-900/40',
        success: 'bg-success-900/40',
        warning: 'bg-warning-900/40',
    };

    const borderClasses: Record<DayTaskVariant, string> = {
        error: 'border-error-500/30',
        info: 'border-info-500/30',
        violet: 'border-violet-500/30',
        success: 'border-success-500/30',
        warning: 'border-warning-500/30',
    };

    const xpBadgeClasses: Record<DayTaskVariant, string> = {
        error: 'bg-error-500/20 text-error-500',
        info: 'bg-info-500/20 text-info-500',
        violet: 'bg-violet-500/20 text-violet-500',
        success: 'bg-success-500/20 text-success-500',
        warning: 'bg-warning-500/20 text-warning-500',
    };

    const attributeBadgeClasses: Record<DayTaskVariant, string> = {
        error: 'bg-error-900/60 text-error-500',
        info: 'bg-info-900/60 text-info-500',
        violet: 'bg-violet-900/60 text-violet-500',
        success: 'bg-success-900/60 text-success-500',
        warning: 'bg-warning-900/60 text-warning-500',
    };

    const checkmarkClasses: Record<DayTaskVariant, string> = {
        error: 'bg-error-500',
        info: 'bg-info-500',
        violet: 'bg-violet-500',
        success: 'bg-success-500',
        warning: 'bg-warning-500',
    };

    function taskContainerClass(variant: DayTaskVariant) {
        return twMerge(
            'flex items-center gap-3 p-3 rounded-xl border',
            bgClasses[variant],
            borderClasses[variant]
        );
    }

    function taskXpBadgeClass(variant: DayTaskVariant) {
        return twMerge('text-xs font-bold px-2.5 py-0.5 rounded-full', xpBadgeClasses[variant]);
    }

    function taskAttributeBadgeClass(variant: DayTaskVariant) {
        return twMerge('text-xs font-bold px-2.5 py-0.5 rounded-full', attributeBadgeClasses[variant]);
    }

    function taskCheckmarkClass(variant: DayTaskVariant) {
        return twMerge('w-4 h-4 rounded border-2 flex-shrink-0', checkmarkClasses[variant]);
    }

    const formattedDate = computed(() => {
        if (!props.date) return '';
        return new Date(props.date).toLocaleDateString('en-US', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        });
    });

    const totalXp = computed(() => props.tasks.reduce((sum, task) => sum + task.xp, 0));

    const taskSummary = computed(() => {
        const suffix = props.tasks.length !== 1 ? 's' : '';
        return `${props.tasks.length} task${suffix} completed · +${totalXp.value} XP earned`;
    });
</script>
