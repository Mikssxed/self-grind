<template>
    <div class="flex flex-col gap-6 p-4 md:p-8 flex-1 max-h-screen overflow-y-auto">
        <div class="flex flex-col gap-1">
            <BaseHeader tag="h1">{{ title }}</BaseHeader>
            <BaseText v-if="subtitle">{{ subtitle }}</BaseText>
        </div>
        <slot />
    </div>
</template>
<script setup lang="ts">
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';

    interface PageLayoutProps {
        title: string;
        subtitle?: string;
    }

    defineProps<PageLayoutProps>();
</script>
