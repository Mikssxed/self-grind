<template>
    <div class="flex flex-col md:flex-row w-screen h-screen overflow-auto">
        <SideBar class="hidden md:flex" />
        <MobileHeader
            class="md:hidden"
            @open-menu="drawerOpen = true"
        />
        <SidebarDrawer
            :isOpen="drawerOpen"
            @update:isOpen="drawerOpen = $event"
        />
        <RouterView />
    </div>
</template>
<script setup lang="ts">
    import { ref } from 'vue';
    import MobileHeader from './MobileHeader.vue';
    import SideBar from './SideBar.vue';
    import SidebarDrawer from './SidebarDrawer.vue';

    const drawerOpen = ref(false);
</script>
