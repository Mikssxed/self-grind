<template>
    <div class="flex items-center justify-between px-4 py-3 border-b border-b-primary-900">
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
        <button
            class="p-2 rounded-lg hover:bg-primary-900 transition-colors"
            @click="$emit('openMenu')"
        >
            <BaseIcon
                name="menu"
                class="w-6 h-6 text-primary-400"
            />
        </button>
    </div>
</template>
<script setup lang="ts">
    import BaseHeader from '../base/BaseHeader.vue';
    import BaseIcon from '../base/BaseIcon.vue';
    import GradientIcon from '../GradientIcon.vue';

    defineEmits<{
        openMenu: [];
    }>();
</script>
