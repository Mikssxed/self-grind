<template>
    <div
        class="rounded-2xl border border-primary-800 bg-primary-900/60 p-6 transition-colors duration-300 hover:border-accent-500/50"
    >
        <div :class="iconWrapClasses">
            <BaseIcon
                :name="icon"
                class="h-6 w-6"
            />
        </div>
        <h3 class="text-lg font-bold text-white">{{ title }}</h3>
        <p class="mt-2 text-sm/6 text-primary-200">{{ description }}</p>
    </div>
</template>
<script setup lang="ts">
    import BaseIcon, { type UIconName } from '@/components/base/BaseIcon.vue';
    import { computed } from 'vue';

    export type FeatureAccent = 'violet' | 'accent';

    interface LandingFeatureCardProps {
        icon: UIconName;
        title: string;
        description: string;
        accent?: FeatureAccent;
    }

    const props = withDefaults(defineProps<LandingFeatureCardProps>(), {
        accent: 'violet',
    });

    const iconWrapAccentClasses: Record<FeatureAccent, string> = {
        violet: 'bg-violet-500/15 text-violet-500',
        accent: 'bg-accent-500/15 text-accent-500',
    };

    const iconWrapClasses = computed(
        () =>
            `mb-4 inline-flex h-12 w-12 items-center justify-center rounded-xl ${iconWrapAccentClasses[props.accent]}`
    );
</script>
