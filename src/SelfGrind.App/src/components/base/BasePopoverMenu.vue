<template>
    <Popover
        ref="popoverRef"
        :pt="passThroughStyles"
    >
        <div class="min-w-48 p-1.5">
            <button
                v-for="item in items"
                :key="item.label"
                :class="itemClasses(item)"
                @click="handleItemClick(item)"
            >
                <BaseIcon
                    v-if="item.icon"
                    :name="item.icon"
                    class="w-4 h-4"
                />
                {{ item.label }}
            </button>
        </div>
    </Popover>
</template>
<script setup lang="ts">
    import Popover from 'primevue/popover';
    import { ref } from 'vue';
    import { twMerge } from 'tailwind-merge';
    import BaseIcon from './BaseIcon.vue';
    import type { IconName } from '@/components/icons';

    export type PopoverMenuItemVariant = 'default' | 'danger';

    export interface PopoverMenuItem {
        label: string;
        icon?: IconName;
        variant?: PopoverMenuItemVariant;
        action: () => void;
    }

    interface BasePopoverMenuProps {
        items: PopoverMenuItem[];
    }

    defineProps<BasePopoverMenuProps>();

    const popoverRef = ref<InstanceType<typeof Popover> | null>(null);

    const passThroughStyles = {
        root: {
            class: 'rounded-xl bg-primary-900 border border-primary-800 shadow-xl shadow-black/40 overflow-hidden',
        },
        content: {
            class: '',
        },
    };

    const baseItemClass =
        'flex items-center gap-3 w-full px-3 py-2.5 text-sm rounded-lg transition-colors duration-150 cursor-pointer';

    const variantItemClasses: Record<PopoverMenuItemVariant, string> = {
        default: 'text-primary-400 hover:bg-primary-800 hover:text-white',
        danger: 'text-error-500 hover:bg-error-500/10 hover:text-error-500',
    };

    function itemClasses(item: PopoverMenuItem) {
        return twMerge(baseItemClass, variantItemClasses[item.variant ?? 'default']);
    }

    function toggle(event: Event) {
        popoverRef.value?.toggle(event);
    }

    function hide() {
        popoverRef.value?.hide();
    }

    function handleItemClick(item: PopoverMenuItem) {
        hide();
        item.action();
    }

    defineExpose({ toggle, hide });
</script>
