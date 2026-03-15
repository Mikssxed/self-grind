<template>
    <div class="w-full flex justify-center items-center pt-25 px-4">
        <div class="flex flex-col gap-8 w-full max-w-md">
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
        </div>
    </div>
</template>
<script setup lang="ts">
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import GradientIcon from '@/components/GradientIcon.vue';
    import type { IconName } from '@/components/icons';

    interface AuthLayoutProps {
        icon: IconName;
        title: string;
        subtitle: string;
    }

    defineProps<AuthLayoutProps>();
</script>
