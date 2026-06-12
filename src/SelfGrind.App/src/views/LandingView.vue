<template>
    <div class="min-h-screen">
        <LandingNav />
        <main>
            <LandingHero />
            <LandingFeatures />
            <LandingHowItWorks />
            <LandingFaq />
            <LandingCta />
        </main>
        <LandingFooter />
    </div>
</template>
<script setup lang="ts">
    import LandingCta from '@/components/landing/LandingCta.vue';
    import LandingFaq from '@/components/landing/LandingFaq.vue';
    import LandingFeatures from '@/components/landing/LandingFeatures.vue';
    import LandingFooter from '@/components/landing/LandingFooter.vue';
    import LandingHero from '@/components/landing/LandingHero.vue';
    import LandingHowItWorks from '@/components/landing/LandingHowItWorks.vue';
    import LandingNav from '@/components/landing/LandingNav.vue';
</script>
