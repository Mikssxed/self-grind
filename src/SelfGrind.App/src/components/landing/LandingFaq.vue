<template>
    <section
        id="faq"
        class="mx-auto max-w-3xl scroll-mt-24 px-6 py-20"
    >
        <div class="text-center">
            <h2 class="text-3xl font-bold text-white sm:text-4xl">Frequently asked questions</h2>
            <p class="mt-4 text-primary-200">
                Everything you need to know before starting your grind.
            </p>
        </div>
        <div class="mt-10 space-y-3">
            <LandingFaqItem
                v-for="faq in faqs"
                :key="faq.question"
                :question="faq.question"
                :answer="faq.answer"
            />
        </div>
    </section>
</template>
<script setup lang="ts">
    import LandingFaqItem from './LandingFaqItem.vue';

    interface LandingFaqEntry {
        question: string;
        answer: string;
    }

    // NOTE: must stay in sync with the FAQPage JSON-LD in index.html (Google requires the
    // structured data to match visible page content).
    const faqs: LandingFaqEntry[] = [
        {
            question: 'What is SelfGrind?',
            answer: 'SelfGrind is a free gamified task manager and habit tracker. Every task you complete grants XP that levels up your character and grows attributes like Strength and Knowledge.',
        },
        {
            question: 'Is SelfGrind free to use?',
            answer: 'Yes. SelfGrind is completely free — create an account and start earning XP in minutes. No credit card required.',
        },
        {
            question: 'How does the XP and leveling system work?',
            answer: 'Each task and habit is worth XP and is tied to an attribute. Completing it raises both your overall level and that attribute, so your character mirrors your real-life effort.',
        },
        {
            question: 'Can I track habits as well as tasks?',
            answer: 'Yes. Daily and recurring habits get a GitHub-style contribution grid, so you can see your streaks and consistency at a glance.',
        },
        {
            question: 'How is SelfGrind different from a normal to-do app?',
            answer: 'Instead of a plain checklist, SelfGrind gives you the feedback loops of a game: XP, levels, stats, achievements, and a leaderboard that make finishing things genuinely satisfying.',
        },
    ];
</script>
