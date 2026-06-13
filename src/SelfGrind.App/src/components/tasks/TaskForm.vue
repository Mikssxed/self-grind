<template>
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
                @click="onCancel"
            >
                Cancel
            </BaseButton>
            <BaseButton
                variant="primary"
                class="flex-1 py-3"
                type="submit"
            >
                {{ submitLabel }}
            </BaseButton>
        </div>
    </form>
</template>
<script setup lang="ts">
    import { useForm as useVeeValidateForm, useFieldValue } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import { computed, watch } from 'vue';
    import { addTaskSchema } from '@/schemas/addTaskSchema';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import TextAreaField from '@/components/form/TextAreaField.vue';
    import ToggleButtonGroup from '@/components/form/ToggleButtonGroup.vue';
    import type { ToggleOption } from '@/components/form/ToggleButtonGroup.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import AttributeSelector from '@/components/form/AttributeSelector.vue';
    import DayOfWeekSelector from '@/components/form/DayOfWeekSelector.vue';
    import DatePickerField from '@/components/form/DatePickerField.vue';
    import type { TaskFormValues } from '@/utils';

    interface TaskFormProps {
        initialValues: TaskFormValues;
        submitLabel: string;
    }

    const props = defineProps<TaskFormProps>();

    const emit = defineEmits<{
        submit: [values: TaskFormValues];
        cancel: [];
    }>();

    const taskTypeOptions: ToggleOption[] = [
        { label: 'Daily', value: 'Daily' },
        { label: 'Weekly', value: 'Weekly' },
        { label: 'Once', value: 'Once' },
    ];

    const schema = toTypedSchema(addTaskSchema);

    const { handleSubmit, setFieldValue } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: props.initialValues,
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
    const repeatIntervalUnit = computed(() => (isWeekly.value ? 'weeks' : 'days'));

    const onSubmit = handleSubmit((values) => {
        emit('submit', values as TaskFormValues);
    });

    function onCancel() {
        emit('cancel');
    }

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
</script>
