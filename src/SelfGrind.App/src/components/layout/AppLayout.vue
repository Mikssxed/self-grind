<template>
    <div>
        <SidebarLayout v-if="hasSidebar" />
        <RouterView v-else />
    </div>
</template>
<script setup lang="ts">
    import { LayoutType } from '@/types';
    import { computed } from 'vue';
    import { useRouter } from 'vue-router';
    import SidebarLayout from './SidebarLayout.vue';

    const router = useRouter();

    const hasSidebar = computed(() => {
        const currentRoute = router.currentRoute.value;
        return currentRoute.meta.layoutType === LayoutType.WITH_SIDEBAR;
    });
</script>
