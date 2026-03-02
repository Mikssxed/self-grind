<template>
    <div class="flex w-screen h-screen overflow-auto">
        <SideBar />
        <RouterView />
    </div>
</template>
<script setup lang="ts">
    import SideBar from './SideBar.vue';
</script>
