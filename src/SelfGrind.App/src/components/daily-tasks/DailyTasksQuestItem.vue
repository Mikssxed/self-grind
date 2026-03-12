<script lang="ts" setup>
    import { twMerge } from 'tailwind-merge';
    import { computed, ref, watch } from 'vue';

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

    const isAnimating = ref(false);

    watch(() => props.completed, (newVal, oldVal) => {
        if (newVal && !oldVal) {
            isAnimating.value = true;
            setTimeout(() => { isAnimating.value = false; }, 700);
        }
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
            'flex items-center gap-4 p-4 rounded-xl border transition-all duration-500',
            bgClasses[props.variant],
            borderClasses[props.variant],
            props.completed && 'opacity-60',
            isAnimating.value && 'animate-complete'
        );
    });

    const checkboxClass = computed(() => {
        return twMerge(
            'w-5 h-5 rounded-full border-2 flex-shrink-0 flex items-center justify-center transition-all duration-300',
            checkboxClasses[props.variant],
            props.completed && checkboxFilledClasses[props.variant]
        );
    });

    const titleClass = computed(() => {
        return twMerge(
            'font-bold transition-all duration-300',
            props.completed ? 'line-through text-primary-300' : 'text-white'
        );
    });

    const xpLabel = computed(() => `+${props.xp} XP`);

    const attributeLabel = computed(() => `${props.attributeEmoji} ${props.attribute}`);

    const xpBadgeClass = computed(() => {
        return twMerge('text-xs font-bold px-3 py-1 rounded-full', xpBadgeClasses[props.variant]);
    });

    const attributeBadgeClass = computed(() => {
        return twMerge('text-xs font-bold px-3 py-1 rounded-full', attributeBadgeClasses[props.variant]);
    });
</script>

<template>
    <div :class="containerClasses">
        <div :class="checkboxClass">
            <svg
                v-if="completed"
                class="w-3 h-3 text-white animate-check"
                viewBox="0 0 12 10"
                fill="none"
            >
                <path d="M1 5l3.5 3.5L11 1" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        </div>
        <div class="flex flex-col gap-0.5 flex-1">
            <span :class="titleClass">{{ title }}</span>
            <span class="text-sm text-primary-400">{{ description }}</span>
        </div>
        <div class="flex items-center gap-2">
            <span :class="xpBadgeClass">
                {{ xpLabel }}
            </span>
            <span :class="attributeBadgeClass">
                {{ attributeLabel }}
            </span>
        </div>
    </div>
</template>
