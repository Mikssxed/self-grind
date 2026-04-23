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
            appendTo="body"
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
            class: 'relative w-full rounded-xl border-2 border-primary-800 bg-primary-900 px-4 py-3 focus-within:ring-2 focus-within:ring-primary-600 transition-shadow',
        },
        pcInput: {
            root: {
                class: 'w-full text-sm text-white placeholder:text-primary-600 cursor-pointer bg-transparent outline-none',
            },
        },
        panel: {
            class: 'bg-primary-900 border border-primary-800 rounded-2xl shadow-2xl p-4 z-[200]',
        },
        header: {
            class: 'flex items-center justify-between mb-3 pb-3 border-b border-primary-800/60',
        },
        title: {
            class: 'flex gap-1 items-center',
        },
        pcPreviousButton: {
            root: {
                class: 'w-8 h-8 flex items-center justify-center text-primary-400 hover:text-white hover:bg-primary-800 rounded-lg cursor-pointer transition-all',
            },
        },
        pcNextButton: {
            root: {
                class: 'w-8 h-8 flex items-center justify-center text-primary-400 hover:text-white hover:bg-primary-800 rounded-lg cursor-pointer transition-all',
            },
        },
        pcMonthTitle: {
            root: {
                class: 'text-white font-semibold text-sm cursor-pointer hover:text-accent-500 transition-colors',
            },
        },
        pcYearTitle: {
            root: {
                class: 'text-primary-400 text-sm cursor-pointer hover:text-accent-500 transition-colors',
            },
        },
        dayView: {
            class: 'w-full mt-1',
        },
        tableHeaderRow: {
            class: '',
        },
        tableHeaderCell: {
            class: 'text-center pb-2',
        },
        columnHeader: {
            class: 'text-primary-500 text-xs font-medium uppercase tracking-wide',
        },
        tableBodyRow: {
            class: '',
        },
        tableBodyCell: {
            class: 'p-0.5 text-center',
        },
        day: {
            class: 'w-8 h-8 rounded-lg flex items-center justify-center text-sm cursor-pointer transition-all text-primary-300 hover:bg-accent-500/15 hover:text-white mx-auto',
        },
        daySelected: {
            class: 'w-8 h-8 rounded-lg flex items-center justify-center text-sm font-semibold cursor-pointer transition-all button-primary text-white mx-auto',
        },
        dayToday: {
            class: 'w-8 h-8 rounded-lg flex items-center justify-center text-sm cursor-pointer transition-all border border-accent-500/50 text-accent-400 hover:bg-accent-500/15 hover:text-white mx-auto',
        },
        dayDisabled: {
            class: 'w-8 h-8 rounded-lg flex items-center justify-center text-sm text-primary-700 cursor-not-allowed mx-auto',
        },
    };
</script>
