<template>
    <section
        id="how-it-works"
        class="mx-auto max-w-6xl scroll-mt-24 px-6 py-20"
    >
        <div class="mx-auto max-w-2xl text-center">
            <h2 class="text-3xl font-bold text-white sm:text-4xl">How it works</h2>
            <p class="mt-4 text-primary-200">Three steps from to-do list to level-up.</p>
        </div>
        <div class="mt-16 grid gap-6 sm:grid-cols-3">
            <LandingStep
                v-for="item in steps"
                :key="item.step"
                :step="item.step"
                :icon="item.icon"
                :title="item.title"
                :description="item.description"
            />
        </div>
    </section>
</template>
<script setup lang="ts">
    import type { UIconName } from '@/components/base/BaseIcon.vue';
    import LandingStep from './LandingStep.vue';

    interface HowItWorksStep {
        step: number;
        icon: UIconName;
        title: string;
        description: string;
    }

    const steps: HowItWorksStep[] = [
        {
            step: 1,
            icon: 'dailyTasks',
            title: 'Add tasks & habits',
            description: 'Capture what you want to get done and the habits you want to build.',
        },
        {
            step: 2,
            icon: 'gamepad',
            title: 'Complete to earn XP',
            description: 'Check things off to gain experience and grow your attributes.',
        },
        {
            step: 3,
            icon: 'character',
            title: 'Level up your life',
            description: 'Watch your character, stats, and streaks climb as you stay consistent.',
        },
    ];
</script>
