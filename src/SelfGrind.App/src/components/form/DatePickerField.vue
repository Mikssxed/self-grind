<template>
    <FormField
        :label="label"
        :name="name"
    >
        <DatePicker
            v-model="value"
            :minDate="minDate"
            :placeholder="placeholder"
            dateFormat="yy-mm-dd"
            :pt="passThroughStyles"
            appendTo="self"
        />
    </FormField>
</template>
<script setup lang="ts">
    import { useField } from 'vee-validate';
    import DatePicker from 'primevue/datepicker';
    import FormField from './FormField.vue';

    export interface DatePickerFieldProps {
        name: string;
        label: string;
        minDate?: Date;
        placeholder?: string;
    }

    const props = withDefaults(defineProps<DatePickerFieldProps>(), {
        minDate: undefined,
        placeholder: 'Select a date',
    });

    const { value } = useField<Date | undefined>(() => props.name);

    const passThroughStyles = {
        root: {
            class: 'relative w-full',
        },
        pcInput: {
            root: {
                class: 'input-default w-full px-4 py-3 text-sm cursor-pointer',
            },
        },
        panel: {
            class: 'bg-primary-900 border border-primary-800 rounded-xl shadow-xl mt-2 p-4 z-[100]',
        },
        header: {
            class: 'flex items-center justify-between mb-3',
        },
        pcPreviousButton: {
            root: {
                class: 'text-primary-400 hover:text-white transition-colors p-2 rounded-lg hover:bg-primary-800 cursor-pointer',
            },
        },
        pcNextButton: {
            root: {
                class: 'text-primary-400 hover:text-white transition-colors p-2 rounded-lg hover:bg-primary-800 cursor-pointer',
            },
        },
        title: {
            class: 'flex gap-2',
        },
        pcMonthTitle: {
            root: {
                class: 'text-white font-semibold text-sm cursor-pointer hover:text-accent-500 transition-colors',
            },
        },
        pcYearTitle: {
            root: {
                class: 'text-white font-semibold text-sm cursor-pointer hover:text-accent-500 transition-colors',
            },
        },
        dayView: {
            class: 'w-full',
        },
        tableHeaderRow: {
            class: 'text-primary-400 text-xs font-semibold',
        },
        tableHeaderCell: {
            class: 'p-2 text-center',
        },
        tableBodyRow: {
            class: '',
        },
        tableBodyCell: {
            class: 'p-1 text-center',
        },
        day: {
            class: 'w-9 h-9 rounded-full flex items-center justify-center text-sm cursor-pointer transition-all text-primary-200 hover:bg-accent-500/20 hover:text-white',
        },
        daySelected: {
            class: 'w-9 h-9 rounded-full flex items-center justify-center text-sm cursor-pointer transition-all button-primary text-white',
        },
        dayToday: {
            class: 'w-9 h-9 rounded-full flex items-center justify-center text-sm cursor-pointer transition-all border border-accent-500 text-accent-500 hover:bg-accent-500/20',
        },
        dayDisabled: {
            class: 'w-9 h-9 rounded-full flex items-center justify-center text-sm text-primary-600 cursor-not-allowed',
        },
    };
</script>
