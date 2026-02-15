<script setup lang="ts">
    import BaseIcon from '@/components/base/BaseIcon.vue';
    import type { IconName } from './icons';

    export interface GradientIconProps {
        name: IconName;
    }

    defineProps<GradientIconProps>();
</script>

<template>
    <div class="bg-gradient-purple w-20 h-20 rounded-3xl flex justify-center items-center">
        <BaseIcon
            :name="name"
            class="w-11"
        />
    </div>
</template>
