<template>
    <div :class="containerClass">
        <span class="text-4xl" aria-hidden="true">{{ emoji }}</span>
        <div class="flex flex-col items-center gap-1">
            <span class="text-sm font-bold text-white text-center">{{ title }}</span>
            <p v-if="message" class="text-xs text-primary-400 text-center max-w-xs">{{ message }}</p>
        </div>
        <div v-if="hasAction" class="pt-1">
            <slot name="action" />
        </div>
    </div>
</template>
<script setup lang="ts">
    import { computed, useSlots } from 'vue';
    import { twMerge } from 'tailwind-merge';

    type EmptyStateSize = 'sm' | 'md' | 'lg';

    interface BaseEmptyStateProps {
        emoji: string;
        title: string;
        message?: string;
        size?: EmptyStateSize;
    }

    const props = withDefaults(defineProps<BaseEmptyStateProps>(), {
        size: 'md',
    });

    const slots = useSlots();
    const hasAction = computed(() => !!slots.action);

    const sizeClasses: Record<EmptyStateSize, string> = {
        sm: 'py-6 gap-2',
        md: 'py-10 gap-3',
        lg: 'py-14 gap-4',
    };

    const containerClass = computed(() =>
        twMerge(
            'flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-primary-800 bg-primary-900/40 px-6',
            sizeClasses[props.size]
        )
    );
</script>
