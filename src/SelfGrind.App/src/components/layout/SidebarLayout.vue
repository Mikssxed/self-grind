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

        <AddTaskModal />
        <LogActivityModal />
        <DailyBoostModal />
    </div>
</template>
<script setup lang="ts">
    import { ref } from 'vue';
    import MobileHeader from './MobileHeader.vue';
    import SideBar from './SideBar.vue';
    import SidebarDrawer from './SidebarDrawer.vue';
    import AddTaskModal from '@/components/tasks/AddTaskModal.vue';
    import LogActivityModal from '@/components/tasks/LogActivityModal.vue';
    import DailyBoostModal from '@/components/tasks/DailyBoostModal.vue';

    const drawerOpen = ref(false);
</script>
