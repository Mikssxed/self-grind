<template>
    <BaseModal
        :isOpen="isOpen"
        :header="header"
        @update:isOpen="handleVisibilityChange"
    >
        <div
            v-if="task"
            class="flex flex-col gap-5"
        >
            <template v-if="mode === 'edit'">
                <TaskForm
                    :initial-values="formValues"
                    submit-label="Save changes"
                    @submit="onEditSubmit"
                    @cancel="backToView"
                />
            </template>

            <template v-else>
                <BaseText>{{ task.description }}</BaseText>

                <dl class="flex flex-col gap-3">
                    <div
                        v-for="row in detailRows"
                        :key="row.label"
                        class="flex items-center justify-between gap-4"
                    >
                        <dt class="text-sm text-primary-400">{{ row.label }}</dt>
                        <dd class="text-sm font-medium text-white text-right">{{ row.value }}</dd>
                    </div>
                </dl>

                <div
                    v-if="confirmingDelete"
                    class="flex flex-col gap-3 pt-2"
                >
                    <BaseText class="text-sm text-error-500">
                        Delete this task permanently?
                    </BaseText>
                    <div class="flex gap-3">
                        <BaseButton
                            variant="secondary"
                            class="flex-1 py-3"
                            @click="cancelDelete"
                        >
                            Cancel
                        </BaseButton>
                        <button
                            class="flex-1 py-3 font-bold rounded-xl text-white bg-error-500 hover:opacity-80 transition-opacity duration-200 cursor-pointer"
                            @click="confirmDelete"
                        >
                            Yes, delete
                        </button>
                    </div>
                </div>

                <div
                    v-else
                    class="flex gap-3 pt-2"
                >
                    <button
                        class="flex-1 py-3 font-bold rounded-xl text-error-500 bg-error-500/10 hover:bg-error-500/20 transition-colors duration-200 cursor-pointer"
                        @click="startDelete"
                    >
                        Delete
                    </button>
                    <BaseButton
                        variant="primary"
                        class="flex-1 py-3"
                        @click="startEdit"
                    >
                        Edit
                    </BaseButton>
                </div>
            </template>
        </div>
    </BaseModal>
</template>
<script setup lang="ts">
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import { useTaskDetailModal } from '@/composables/useTaskDetailModal';
    import { useTasks } from '@/composables/useTasks';
    import {
        buildTaskCommandFields,
        emptyTaskFormValues,
        taskToFormValues,
        type TaskFormValues,
    } from '@/utils';
    import { DateOnly } from '@microsoft/kiota-abstractions';
    import { computed, ref, watch } from 'vue';
    import TaskForm from './TaskForm.vue';

    const { isOpen, selectedTask, close } = useTaskDetailModal();
    const { updateMutation, deleteMutation } = useTasks();

    const mode = ref<'view' | 'edit'>('view');
    const confirmingDelete = ref(false);

    const task = computed(() => selectedTask.value);

    const header = computed(() => {
        if (mode.value === 'edit') return 'Edit Task';
        return task.value?.title ?? 'Task';
    });

    const formValues = computed<TaskFormValues>(() =>
        task.value ? taskToFormValues(task.value) : emptyTaskFormValues()
    );

    function formatDate(value: DateOnly | null | undefined): string {
        if (!value) return '—';
        return new Date(value.year, value.month - 1, value.day).toLocaleDateString();
    }

    const detailRows = computed<{ label: string; value: string }[]>(() => {
        const t = task.value;
        if (!t) return [];

        const attribute = getAttributeDisplay(t.attribute);
        const rows: { label: string; value: string }[] = [
            { label: 'Type', value: t.repetitionType ?? '—' },
            { label: 'XP Reward', value: `+${t.exp ?? 0} XP` },
            { label: 'Attribute', value: `${attribute.emoji} ${attribute.label}` },
        ];

        if (t.repetitionType === 'Once') {
            rows.push({ label: 'Date', value: formatDate(t.startDate) });
        } else {
            rows.push({ label: 'Start', value: formatDate(t.startDate) });
            rows.push({ label: 'End', value: formatDate(t.endDate) });
            const unit = t.repetitionType === 'Weekly' ? 'week(s)' : 'day(s)';
            rows.push({ label: 'Repeat', value: `Every ${t.repeatInterval ?? 1} ${unit}` });
        }

        if (t.repetitionType === 'Weekly' && t.daysOfWeek && t.daysOfWeek.length > 0) {
            rows.push({ label: 'Days', value: t.daysOfWeek.join(', ') });
        }

        return rows;
    });

    // The modal stays mounted; reset its sub-state whenever it (re)opens.
    watch(isOpen, (open) => {
        if (open) {
            mode.value = 'view';
            confirmingDelete.value = false;
        }
    });

    function startEdit() {
        mode.value = 'edit';
    }

    function backToView() {
        mode.value = 'view';
    }

    function startDelete() {
        confirmingDelete.value = true;
    }

    function cancelDelete() {
        confirmingDelete.value = false;
    }

    async function confirmDelete() {
        const id = task.value?.id;
        if (!id) return;
        await deleteMutation.mutateAsync(id);
        close();
    }

    async function onEditSubmit(values: TaskFormValues) {
        const id = task.value?.id;
        if (!id) return;
        await updateMutation.mutateAsync({
            id,
            command: { id, ...buildTaskCommandFields(values) },
        });
        close();
    }

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
