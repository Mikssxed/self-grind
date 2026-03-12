<template>
    <BaseModal
        :isOpen="isOpen"
        header="Add New Task"
        @update:isOpen="handleVisibilityChange"
    >
        <form
            class="flex flex-col gap-5"
            @submit.prevent="onSubmit"
        >
            <TextField
                label="Task Title"
                name="title"
                :required="true"
            />

            <TextAreaField
                label="Description (optional)"
                name="description"
                placeholder="Add details about this task..."
                :rows="3"
            />

            <ToggleButtonGroup
                label="Task Type"
                name="repetitionType"
                :options="taskTypeOptions"
            />

            <DayOfWeekSelector
                v-if="repetitionTypeValue === 'Weekly'"
                name="daysOfWeek"
                label="Repeat on"
            />

            <DatePickerField
                v-if="repetitionTypeValue === 'Once'"
                name="taskDate"
                label="Date"
                :minDate="today"
                placeholder="Select a date"
            />

            <RangeSliderField
                label="XP Reward"
                name="exp"
                :min="5"
                :max="100"
                :step="5"
                unit="XP"
            />

            <AttributeSelector name="attribute" />

            <div class="flex gap-3 pt-2">
                <BaseButton
                    variant="secondary"
                    class="flex-1 py-3"
                    type="button"
                    @click="close"
                >
                    Cancel
                </BaseButton>
                <BaseButton
                    variant="primary"
                    class="flex-1 py-3"
                    type="submit"
                >
                    Add Task
                </BaseButton>
            </div>
        </form>
    </BaseModal>
</template>
<script setup lang="ts">
    import { useForm as useVeeValidateForm, useFieldValue } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import { object, string, number, enum as zEnum, optional, array, date } from 'zod';
    import { watch, computed } from 'vue';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import TextAreaField from '@/components/form/TextAreaField.vue';
    import ToggleButtonGroup from '@/components/form/ToggleButtonGroup.vue';
    import type { ToggleOption } from '@/components/form/ToggleButtonGroup.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import AttributeSelector from '@/components/form/AttributeSelector.vue';
    import DayOfWeekSelector from '@/components/form/DayOfWeekSelector.vue';
    import DatePickerField from '@/components/form/DatePickerField.vue';
    import { useAddTaskModal } from '@/composables/useAddTaskModal';

    const { isOpen, close } = useAddTaskModal();

    const taskTypeOptions: ToggleOption[] = [
        { label: 'Daily', value: 'Daily' },
        { label: 'Weekly', value: 'Weekly' },
        { label: 'Once', value: 'Once' },
    ];

    const schema = toTypedSchema(
        object({
            title: string().min(1, 'Task title is required'),
            description: optional(string()),
            repetitionType: zEnum(['Daily', 'Weekly', 'Once']),
            exp: number().min(5).max(100),
            attribute: optional(zEnum(['Strength', 'Knowledge', 'Health', 'Charisma', 'Focus', 'Creativity'])),
            daysOfWeek: optional(array(number().min(0).max(6))),
            taskDate: optional(date()),
        }).superRefine((data, ctx) => {
            if (data.repetitionType === 'Weekly') {
                if (!data.daysOfWeek || data.daysOfWeek.length === 0) {
                    ctx.addIssue({
                        code: 'custom',
                        message: 'Select at least one day',
                        path: ['daysOfWeek'],
                    });
                }
            }
            if (data.repetitionType === 'Once') {
                if (!data.taskDate) {
                    ctx.addIssue({
                        code: 'custom',
                        message: 'Date is required for one-time tasks',
                        path: ['taskDate'],
                    });
                }
            }
        })
    );

    const { handleSubmit, resetForm, setFieldValue } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: {
            title: '',
            description: '',
            repetitionType: 'Daily',
            exp: 10,
            attribute: undefined,
            daysOfWeek: [] as number[],
            taskDate: undefined as Date | undefined,
        },
    });

    const repetitionTypeValue = useFieldValue<string>('repetitionType');

    const today = computed(() => {
        const d = new Date();
        d.setHours(0, 0, 0, 0);
        return d;
    });

    const onSubmit = handleSubmit((values) => {
        console.log('New Task:', values);
        close();
    });

    watch(repetitionTypeValue, (newType) => {
        if (newType !== 'Weekly') {
            setFieldValue('daysOfWeek', []);
        }
        if (newType !== 'Once') {
            setFieldValue('taskDate', undefined);
        }
    });

    watch(isOpen, (open) => {
        if (open) {
            resetForm();
        }
    });

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
