<template>
    <Dialog
        :visible="isOpen"
        :header="header"
        modal
        dismissableMask
        :pt="passThroughStyles"
        @update:visible="$emit('update:isOpen', $event)"
    >
        <slot />
        <template
            v-if="$slots.footer"
            #footer
        >
            <slot name="footer" />
        </template>
    </Dialog>
</template>
<script setup lang="ts">
    import Dialog from 'primevue/dialog';

    interface BaseModalProps {
        isOpen: boolean;
        header?: string;
    }

    defineProps<BaseModalProps>();

    defineEmits<{
        'update:isOpen': [value: boolean];
    }>();

    const passThroughStyles = {
        root: {
            class: 'bg-primary-900 border border-primary-800 rounded-2xl shadow-xl w-full max-w-lg mx-4 max-h-[80vh] flex flex-col',
        },
        header: {
            class: 'flex items-center justify-between p-6 pb-4 shrink-0',
        },
        title: {
            class: 'text-white font-semibold text-xl',
        },
        content: {
            class: 'px-6 pb-6 overflow-y-auto',
        },
        footer: {
            class: 'px-6 pb-6 pt-2 flex gap-3',
        },
        pcCloseButton: {
            root: {
                class: 'text-primary-400 hover:text-white transition-colors p-2 rounded-lg hover:bg-primary-800 cursor-pointer',
            },
        },
        mask: {
            class: 'fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50',
        },
    };
</script>
