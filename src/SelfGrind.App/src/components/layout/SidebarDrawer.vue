<template>
    <BaseDrawer
        :isOpen="isOpen"
        header="SelfGrind"
        position="left"
        @update:isOpen="$emit('update:isOpen', $event)"
    >
        <AppNavigation />
    </BaseDrawer>
</template>
<script setup lang="ts">
    import { watch } from 'vue';
    import { useRoute } from 'vue-router';
    import BaseDrawer from '../base/BaseDrawer.vue';
    import AppNavigation from './AppNavigation.vue';

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
