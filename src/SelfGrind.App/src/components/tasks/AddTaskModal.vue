<template>
    <BaseModal
        :isOpen="isOpen"
        header="Add New Task"
        @update:isOpen="handleVisibilityChange"
    >
        <TaskForm
            :initial-values="emptyTaskFormValues()"
            submit-label="Add Task"
            @submit="onSubmit"
            @cancel="close"
        />
    </BaseModal>
</template>
<script setup lang="ts">
    import BaseModal from '@/components/base/BaseModal.vue';
    import TaskForm from './TaskForm.vue';
    import { useAddTaskModal } from '@/composables/useAddTaskModal';
    import { useTasks } from '@/composables/useTasks';
    import { buildTaskCommandFields, emptyTaskFormValues, type TaskFormValues } from '@/utils';

    const { isOpen, close } = useAddTaskModal();
    const { createMutation } = useTasks();

    async function onSubmit(values: TaskFormValues) {
        await createMutation.mutateAsync(buildTaskCommandFields(values));
        close();
    }

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
