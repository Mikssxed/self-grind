<template>
    <div class="bg-gradient-quote rounded-2xl p-6">
        <p class="text-white italic text-lg">"{{ quote }}"</p>
        <p class="text-primary-400 text-sm mt-2">— {{ author }}</p>
    </div>
</template>
<script setup lang="ts">
    interface BaseQuoteBlockProps {
        quote: string;
        author: string;
    }

    defineProps<BaseQuoteBlockProps>();
</script>
