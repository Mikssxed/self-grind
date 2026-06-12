<script setup lang="ts">
    import PageLayout from '@/components/layout/PageLayout.vue';
    import BasePaginator from '@/components/base/BasePaginator.vue';
    import TasksList from '@/components/tasks/TasksList.vue';
    import { useTasksList } from '@/composables/useTasksList';

    const { tasks, page, totalPages } = useTasksList();
</script>

<template>
    <PageLayout
        title="All Tasks 📋"
        subtitle="Browse every task you've created"
    >
        <TasksList :tasks="tasks" />
        <BasePaginator
            v-model:page="page"
            :total-pages="totalPages"
        />
    </PageLayout>
</template>
