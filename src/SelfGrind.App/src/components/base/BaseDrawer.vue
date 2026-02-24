<template>
    <Drawer
        v-model="isOpen"
        :header="header"
    >
        <slot />
    </Drawer>
</template>
<script setup lang="ts">
    interface BaseDrawerProps {
        isOpen: boolean;
        header?: string;
    }

    defineProps<BaseDrawerProps>();
</script>
