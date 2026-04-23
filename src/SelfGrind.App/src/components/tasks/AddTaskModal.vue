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
                label="Description"
                name="description"
                placeholder="Add details about this task..."
                :rows="3"
                :required="true"
            />

            <ToggleButtonGroup
                label="Task Type"
                name="repetitionType"
                :options="taskTypeOptions"
            />

            <DayOfWeekSelector
                v-if="isWeekly"
                name="daysOfWeek"
                label="Repeat on"
            />

            <DatePickerField
                v-if="isOnce"
                name="taskDate"
                label="Date"
                :minDate="today"
                placeholder="Select a date"
            />

            <DatePickerField
                v-if="isRecurring"
                name="startDate"
                label="Start Date"
                :minDate="today"
                placeholder="Select start date"
            />

            <DatePickerField
                v-if="isRecurring"
                name="endDate"
                label="End Date"
                :minDate="minEndDate"
                placeholder="Select end date"
            />

            <RangeSliderField
                v-if="isRecurring"
                label="Repeat Every"
                name="repeatInterval"
                :min="1"
                :max="7"
                :step="1"
                :unit="repeatIntervalUnit"
            />

            <RangeSliderField
                label="XP Reward"
                name="exp"
                :min="5"
                :max="100"
                :step="5"
                unit="XP"
            />

            <AttributeSelector
                name="attribute"
                label="Character Attribute"
            />

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
    import { watch, computed } from 'vue';
    import { addTaskSchema } from '@/schemas/addTaskSchema';
    import { DateOnly } from '@microsoft/kiota-abstractions';
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
    import { useTasks } from '@/composables/useTasks';
    import { DayOfWeekObject, type DayOfWeek } from '@/api/apiClient/models';

    const { isOpen, close } = useAddTaskModal();
    const { createMutation } = useTasks();

    const dayNumberToEnum: DayOfWeek[] = Object.values(DayOfWeekObject) as DayOfWeek[];

    const taskTypeOptions: ToggleOption[] = [
        { label: 'Daily', value: 'Daily' },
        { label: 'Weekly', value: 'Weekly' },
        { label: 'Once', value: 'Once' },
    ];

    const schema = toTypedSchema(addTaskSchema);

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
            startDate: undefined as Date | undefined,
            endDate: undefined as Date | undefined,
            repeatInterval: 1,
        },
    });

    const repetitionTypeValue = useFieldValue<string>('repetitionType');
    const startDateValue = useFieldValue<Date | undefined>('startDate');

    const today = computed(() => {
        const d = new Date();
        d.setHours(0, 0, 0, 0);
        return d;
    });

    const isWeekly = computed(() => repetitionTypeValue.value === 'Weekly');
    const isOnce = computed(() => repetitionTypeValue.value === 'Once');
    const isRecurring = computed(() => repetitionTypeValue.value !== 'Once');
    const minEndDate = computed(() => startDateValue.value ?? today.value);
    const repeatIntervalUnit = computed(() => isWeekly.value ? 'weeks' : 'days');

    const onSubmit = handleSubmit(async (values) => {
        const startDate = isOnce.value
            ? (values.taskDate ? DateOnly.fromDate(values.taskDate) : null)
            : (values.startDate ? DateOnly.fromDate(values.startDate) : null);

        const endDate = isOnce.value
            ? (values.taskDate ? DateOnly.fromDate(values.taskDate) : null)
            : (values.endDate ? DateOnly.fromDate(values.endDate) : null);

        await createMutation.mutateAsync({
            title: values.title,
            description: values.description,
            repetitionType: values.repetitionType,
            exp: values.exp,
            attribute: values.attribute ?? null,
            daysOfWeek: values.daysOfWeek?.map(n => dayNumberToEnum[n]).filter((d): d is DayOfWeek => d !== undefined) ?? null,
            startDate,
            endDate,
            repeatInterval: values.repeatInterval ?? 1,
        });
        close();
    });

    watch(repetitionTypeValue, (newType) => {
        if (newType !== 'Weekly') {
            setFieldValue('daysOfWeek', []);
        }
        if (newType === 'Once') {
            setFieldValue('startDate', undefined);
            setFieldValue('endDate', undefined);
        } else {
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
