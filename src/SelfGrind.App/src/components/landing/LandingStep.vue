<template>
    <div class="relative rounded-2xl border border-primary-800 bg-primary-900/40 p-6">
        <span
            class="bg-gradient-purple absolute -top-4 left-6 inline-flex h-9 w-9 items-center justify-center rounded-full text-sm font-bold text-white"
        >
            {{ step }}
        </span>
        <div class="mt-4 mb-3 inline-flex h-11 w-11 items-center justify-center rounded-xl bg-accent-500/15 text-accent-500">
            <BaseIcon
                :name="icon"
                class="h-5 w-5"
            />
        </div>
        <h3 class="text-base font-bold text-white">{{ title }}</h3>
        <p class="mt-1.5 text-sm/6 text-primary-200">{{ description }}</p>
    </div>
</template>
<script setup lang="ts">
    import BaseIcon, { type UIconName } from '@/components/base/BaseIcon.vue';

    interface LandingStepProps {
        step: number;
        icon: UIconName;
        title: string;
        description: string;
    }

    defineProps<LandingStepProps>();
</script>
