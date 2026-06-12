<template>
    <section class="relative overflow-x-clip">
        <!-- ambient glow (clipped horizontally only, so it bleeds softly top/bottom with no hard seam) -->
        <div
            class="pointer-events-none absolute -top-40 -right-32 h-96 w-96 rounded-full bg-secondary-700/20 blur-3xl"
            aria-hidden="true"
        ></div>
        <div
            class="pointer-events-none absolute top-24 -left-40 h-96 w-96 rounded-full bg-accent-700/20 blur-3xl"
            aria-hidden="true"
        ></div>

        <div
            class="relative mx-auto grid max-w-6xl items-center gap-12 px-6 pt-16 pb-20 lg:grid-cols-2 lg:pt-24"
        >
            <div>
                <span
                    class="inline-flex items-center gap-2 rounded-full border border-accent-500/30 bg-accent-500/10 px-3 py-1 text-xs font-semibold text-accent-500"
                >
                    Gamified productivity
                </span>
                <h1 class="mt-6 text-4xl font-extrabold tracking-tight text-white sm:text-6xl">
                    Turn your tasks into
                    <span class="text-gradient-purple">XP</span>.
                </h1>
                <p class="mt-6 max-w-xl text-lg text-primary-200">
                    SelfGrind is a gamified task manager that turns your daily quests and habits
                    into experience, stats, and achievements — so doing the work actually feels
                    rewarding.
                </p>
                <div class="mt-9 flex flex-wrap items-center gap-4">
                    <LandingCtaButton
                        to="register"
                        variant="primary"
                        size="lg"
                    >
                        Start your grind — it's free
                    </LandingCtaButton>
                    <LandingCtaButton
                        to="login"
                        variant="secondary"
                        size="lg"
                    >
                        Sign in
                    </LandingCtaButton>
                </div>
                <p class="mt-5 text-sm text-primary-400">
                    No credit card. Build your character in minutes.
                </p>
            </div>

            <div class="flex justify-center lg:justify-end">
                <LandingHeroPreview />
            </div>
        </div>
    </section>
</template>
<script setup lang="ts">
    import LandingCtaButton from './LandingCtaButton.vue';
    import LandingHeroPreview from './LandingHeroPreview.vue';
</script>
