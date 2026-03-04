<script lang="ts" setup>
    import BaseBox from '@/components/base/BaseBox.vue';

    interface DailyTasksStatCardProps {
        emoji: string;
        value: string;
        label: string;
    }

    defineProps<DailyTasksStatCardProps>();
</script>

<template>
    <BaseBox class="flex flex-col items-center gap-2 py-4">
        <span class="text-2xl">{{ emoji }}</span>
        <span class="text-3xl font-bold text-white">{{ value }}</span>
        <span class="text-sm text-primary-400">{{ label }}</span>
    </BaseBox>
</template>
