<script lang="ts" setup>
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    export type QuestItemVariant = 'error' | 'info' | 'violet' | 'success' | 'warning';

    interface DailyTasksQuestItemProps {
        title: string;
        description: string;
        xp: number;
        attribute: string;
        attributeEmoji: string;
        variant: QuestItemVariant;
        completed?: boolean;
    }

    const props = withDefaults(defineProps<DailyTasksQuestItemProps>(), {
        completed: false,
    });

    const bgClasses: Record<QuestItemVariant, string> = {
        error: 'bg-error-900/40',
        info: 'bg-info-900/40',
        violet: 'bg-violet-900/40',
        success: 'bg-success-900/40',
        warning: 'bg-warning-900/40',
    };

    const borderClasses: Record<QuestItemVariant, string> = {
        error: 'border-error-500/30',
        info: 'border-info-500/30',
        violet: 'border-violet-500/30',
        success: 'border-success-500/30',
        warning: 'border-warning-500/30',
    };

    const xpBadgeClasses: Record<QuestItemVariant, string> = {
        error: 'bg-error-500/20 text-error-500',
        info: 'bg-info-500/20 text-info-500',
        violet: 'bg-violet-500/20 text-violet-500',
        success: 'bg-success-500/20 text-success-500',
        warning: 'bg-warning-500/20 text-warning-500',
    };

    const attributeBadgeClasses: Record<QuestItemVariant, string> = {
        error: 'bg-error-900/60 text-error-500',
        info: 'bg-info-900/60 text-info-500',
        violet: 'bg-violet-900/60 text-violet-500',
        success: 'bg-success-900/60 text-success-500',
        warning: 'bg-warning-900/60 text-warning-500',
    };

    const checkboxClasses: Record<QuestItemVariant, string> = {
        error: 'border-error-500',
        info: 'border-info-500',
        violet: 'border-violet-500',
        success: 'border-success-500',
        warning: 'border-warning-500',
    };

    const checkboxFilledClasses: Record<QuestItemVariant, string> = {
        error: 'bg-error-500',
        info: 'bg-info-500',
        violet: 'bg-violet-500',
        success: 'bg-success-500',
        warning: 'bg-warning-500',
    };

    const containerClasses = computed(() => {
        return twMerge(
            'flex items-center gap-4 p-4 rounded-xl border',
            bgClasses[props.variant],
            borderClasses[props.variant]
        );
    });

    const checkboxClass = computed(() => {
        return twMerge(
            'w-5 h-5 rounded border-2 flex-shrink-0',
            checkboxClasses[props.variant],
            props.completed && checkboxFilledClasses[props.variant]
        );
    });

    const xpLabel = computed(() => `+${props.xp} XP`);

    const xpBadgeClass = computed(() => {
        return twMerge('text-xs font-bold px-3 py-1 rounded-full', xpBadgeClasses[props.variant]);
    });

    const attributeBadgeClass = computed(() => {
        return twMerge('text-xs font-bold px-3 py-1 rounded-full', attributeBadgeClasses[props.variant]);
    });
</script>

<template>
    <div :class="containerClasses">
        <div :class="checkboxClass" />
        <div class="flex flex-col gap-0.5 flex-1">
            <span class="font-bold text-white">{{ title }}</span>
            <span class="text-sm text-primary-400">{{ description }}</span>
        </div>
        <div class="flex items-center gap-2">
            <span :class="xpBadgeClass">
                {{ xpLabel }}
            </span>
            <span :class="attributeBadgeClass">
                {{ attributeEmoji }} {{ attribute }}
            </span>
        </div>
    </div>
</template>
