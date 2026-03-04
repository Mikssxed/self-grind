<script lang="ts" setup>
    import DailyTasksStatCard from './DailyTasksStatCard.vue';

    export interface DailyTasksStat {
        emoji: string;
        value: string;
        label: string;
    }

    interface DailyTasksStatsProps {
        stats: DailyTasksStat[];
    }

    defineProps<DailyTasksStatsProps>();
</script>

<template>
    <div class="grid grid-cols-3 gap-4">
        <DailyTasksStatCard
            v-for="stat in stats"
            :key="stat.label"
            :emoji="stat.emoji"
            :value="stat.value"
            :label="stat.label"
        />
    </div>
</template>
