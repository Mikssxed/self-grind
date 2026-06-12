<template>
    <header
        class="sticky top-0 z-20 border-b border-primary-800/60 bg-primary-900/40 backdrop-blur-md"
    >
        <nav class="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
            <RouterLink
                :to="{ name: 'landing' }"
                class="flex items-center gap-2.5"
            >
                <img
                    src="/favicon.svg"
                    alt="SelfGrind logo"
                    class="h-8 w-8"
                />
                <span class="text-lg font-bold text-white">SelfGrind</span>
            </RouterLink>
            <div class="hidden items-center gap-7 md:flex">
                <a
                    v-for="link in sectionLinks"
                    :key="link.href"
                    :href="link.href"
                    class="text-sm font-medium text-primary-200 transition-colors hover:text-white"
                >
                    {{ link.label }}
                </a>
            </div>
            <div class="flex items-center gap-3">
                <LandingCtaButton
                    to="login"
                    variant="secondary"
                    size="md"
                >
                    Sign in
                </LandingCtaButton>
                <LandingCtaButton
                    to="register"
                    variant="primary"
                    size="md"
                >
                    Get started
                </LandingCtaButton>
            </div>
        </nav>
    </header>
</template>
<script setup lang="ts">
    import LandingCtaButton from './LandingCtaButton.vue';

    interface SectionLink {
        href: string;
        label: string;
    }

    const sectionLinks: SectionLink[] = [
        { href: '#features', label: 'Features' },
        { href: '#how-it-works', label: 'How it works' },
        { href: '#faq', label: 'FAQ' },
    ];
</script>
