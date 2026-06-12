<template>
    <details
        class="group rounded-2xl border border-primary-800 bg-primary-900/50 px-5 py-4 transition-colors duration-300 open:border-accent-500/40"
    >
        <summary
            class="flex cursor-pointer list-none items-center justify-between gap-4 font-semibold text-white [&::-webkit-details-marker]:hidden"
        >
            {{ question }}
            <span
                class="text-xl leading-none text-accent-500 transition-transform duration-200 group-open:rotate-45"
                aria-hidden="true"
            >
                +
            </span>
        </summary>
        <p class="mt-3 text-sm/6 text-primary-200">{{ answer }}</p>
    </details>
</template>
<script setup lang="ts">
    interface LandingFaqItemProps {
        question: string;
        answer: string;
    }

    defineProps<LandingFaqItemProps>();
</script>
