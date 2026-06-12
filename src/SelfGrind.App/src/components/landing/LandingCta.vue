<template>
    <section class="mx-auto max-w-6xl px-6 py-20">
        <div
            class="bg-gradient-purple relative overflow-hidden rounded-3xl px-8 py-16 text-center"
        >
            <div
                class="pointer-events-none absolute inset-0 bg-primary-900/10"
                aria-hidden="true"
            ></div>
            <div class="relative">
                <h2 class="text-3xl font-bold text-white sm:text-4xl">Ready to level up?</h2>
                <p class="mx-auto mt-4 max-w-xl text-white/80">
                    Join SelfGrind and turn the things you have to do into the game you want to
                    play.
                </p>
                <div class="mt-8 flex justify-center">
                    <LandingCtaButton
                        to="register"
                        variant="secondary"
                        size="lg"
                    >
                        Create your free account
                    </LandingCtaButton>
                </div>
            </div>
        </div>
    </section>
</template>
<script setup lang="ts">
    import LandingCtaButton from './LandingCtaButton.vue';
</script>
