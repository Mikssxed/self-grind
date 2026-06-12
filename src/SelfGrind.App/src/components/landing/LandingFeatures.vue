<template>
    <section
        id="features"
        class="mx-auto max-w-6xl scroll-mt-24 px-6 py-20"
    >
        <div class="mx-auto max-w-2xl text-center">
            <h2 class="text-3xl font-bold text-white sm:text-4xl">
                Everything you do becomes progress
            </h2>
            <p class="mt-4 text-primary-200">
                SelfGrind turns the boring parts of productivity into a game you actually want to
                play.
            </p>
        </div>
        <div class="mt-14 grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
            <LandingFeatureCard
                v-for="feature in features"
                :key="feature.title"
                :icon="feature.icon"
                :title="feature.title"
                :description="feature.description"
                :accent="feature.accent"
            />
        </div>
    </section>
</template>
<script setup lang="ts">
    import type { UIconName } from '@/components/base/BaseIcon.vue';
    import LandingFeatureCard, { type FeatureAccent } from './LandingFeatureCard.vue';

    interface LandingFeature {
        icon: UIconName;
        title: string;
        description: string;
        accent: FeatureAccent;
    }

    const features: LandingFeature[] = [
        {
            icon: 'dailyTasks',
            title: 'Daily quests',
            description:
                'Your tasks become quests. Complete them to earn XP and keep your streak alive.',
            accent: 'violet',
        },
        {
            icon: 'contributionGrid',
            title: 'Habit tracking',
            description:
                'Build habits with a GitHub-style contribution grid that makes your consistency visible.',
            accent: 'accent',
        },
        {
            icon: 'character',
            title: 'Character & stats',
            description:
                'Grow a character across core attributes like Strength and Knowledge as you do real work.',
            accent: 'violet',
        },
        {
            icon: 'analytics',
            title: 'Insightful analytics',
            description:
                'Track XP per week, life balance, and stat growth with clear, motivating charts.',
            accent: 'accent',
        },
        {
            icon: 'gamepad',
            title: 'Achievements & loot',
            description:
                'Unlock achievements and items as you level up. Your effort, finally rewarded.',
            accent: 'violet',
        },
        {
            icon: 'community',
            title: 'Community & leaderboard',
            description:
                'Climb the leaderboard and grind alongside others chasing their own goals.',
            accent: 'accent',
        },
    ];
</script>
