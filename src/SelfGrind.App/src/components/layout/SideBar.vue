<template>
    <div class="flex flex-col py-6 border-r border-r-primary-900 w-3xs">
        <div class="flex gap-3 items-center pb-6 border-b border-b-primary-900 px-4">
            <GradientIcon
                name="gamepad"
                size="small"
            />
            <div>
                <BaseHeader
                    variant="gradient"
                    tag="h4"
                >
                    SelfGrind
                </BaseHeader>
                <BaseText class="text-xs">Level Up Your Life</BaseText>
            </div>
        </div>
        <AppNavigation class="flex-1" />
        <SidebarUserProfile />
    </div>
</template>
<script setup lang="ts">
    import BaseHeader from '../base/BaseHeader.vue';
    import BaseText from '../base/BaseText.vue';
    import GradientIcon from '../GradientIcon.vue';
    import AppNavigation from './AppNavigation.vue';
    import SidebarUserProfile from './SidebarUserProfile.vue';
</script>
