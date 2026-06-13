<template>
    <div class="w-full flex flex-col items-center pt-10 px-4">
        <RouterLink
            :to="{ name: 'landing' }"
            class="flex items-center gap-2.5 transition-opacity hover:opacity-80"
        >
            <img
                src="/favicon.svg"
                alt="SelfGrind logo"
                class="h-8 w-8"
            />
            <span class="text-lg font-bold text-white">SelfGrind</span>
        </RouterLink>
        <div class="flex flex-col gap-8 w-full max-w-md pt-15">
            <div class="flex flex-col gap-4 p-2">
                <GradientIcon :name="icon" />
                <div class="flex flex-col gap-2">
                    <BaseHeader
                        variant="gradient"
                        class="text-center"
                    >
                        {{ title }}
                    </BaseHeader>
                    <BaseText class="text-center">{{ subtitle }}</BaseText>
                </div>
            </div>
            <slot />
            <slot name="footer" />
        </div>
    </div>
</template>
<script setup lang="ts">
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import GradientIcon from '@/components/GradientIcon.vue';
    import type { IconName } from '@/components/icons';
    import { RouterLink } from 'vue-router';

    interface AuthLayoutProps {
        icon: IconName;
        title: string;
        subtitle: string;
    }

    defineProps<AuthLayoutProps>();
</script>
