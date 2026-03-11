<template>
    <BaseDrawer
        :isOpen="isOpen"
        position="left"
        @update:isOpen="$emit('update:isOpen', $event)"
    >
        <template #header>
            <div class="flex gap-3 items-center">
                <GradientIcon
                    name="gamepad"
                    size="small"
                />
                <BaseHeader
                    variant="gradient"
                    tag="h4"
                >
                    SelfGrind
                </BaseHeader>
            </div>
        </template>
        <div class="flex flex-col h-full">
            <AppNavigation class="flex-1" />
            <MobileUserProfile />
        </div>
    </BaseDrawer>
</template>
<script setup lang="ts">
    import { watch } from 'vue';
    import { useRoute } from 'vue-router';
    import BaseDrawer from '../base/BaseDrawer.vue';
    import BaseHeader from '../base/BaseHeader.vue';
    import GradientIcon from '../GradientIcon.vue';
    import AppNavigation from './AppNavigation.vue';
    import MobileUserProfile from './MobileUserProfile.vue';

    interface SidebarDrawerProps {
        isOpen: boolean;
    }

    defineProps<SidebarDrawerProps>();

    const emit = defineEmits<{
        'update:isOpen': [value: boolean];
    }>();

    const route = useRoute();

    watch(() => route.name, () => {
        emit('update:isOpen', false);
    });
</script>
