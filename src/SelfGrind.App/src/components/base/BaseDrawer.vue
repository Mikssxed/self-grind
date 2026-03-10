<template>
    <Drawer
        :visible="isOpen"
        :header="header"
        :position="position"
        :pt="passThroughStyles"
        @update:visible="$emit('update:isOpen', $event)"
    >
        <slot />
    </Drawer>
</template>
<script setup lang="ts">
    import Drawer from 'primevue/drawer';

    export type DrawerPosition = 'left' | 'right' | 'top' | 'bottom';

    interface BaseDrawerProps {
        isOpen: boolean;
        header?: string;
        position?: DrawerPosition;
    }

    withDefaults(defineProps<BaseDrawerProps>(), {
        position: 'left',
    });

    defineEmits<{
        'update:isOpen': [value: boolean];
    }>();

    const passThroughStyles = {
        root: {
            class: 'fixed top-0 left-0 h-full w-72 bg-primary-900 border-r border-r-primary-800 shadow-xl z-50 flex flex-col',
        },
        header: {
            class: 'flex items-center justify-between p-4 border-b border-b-primary-800',
        },
        title: {
            class: 'text-white font-semibold text-lg',
        },
        content: {
            class: 'flex-1 overflow-y-auto',
        },
        pcCloseButton: {
            root: {
                class: 'text-primary-400 hover:text-white transition-colors p-2 rounded-lg hover:bg-primary-800 cursor-pointer',
            },
        },
        mask: {
            class: 'fixed inset-0 bg-black/50 z-40',
        },
    };
</script>
