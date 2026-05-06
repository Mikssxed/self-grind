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
            class: 'relative w-full rounded-xl border-2 border-primary-800 bg-primary-900 focus-within:ring-2 focus-within:ring-primary-600 transition-shadow',
        },
        pcInputText: {
            root: {
                class: 'w-full px-4 py-3 text-sm text-white placeholder:text-primary-600 cursor-pointer bg-transparent outline-none',
            },
        },
        panel: {
            class: 'bg-primary-800 border-2 border-primary-600 rounded-2xl shadow-2xl p-4 z-[200] mt-1',
        },
        header: {
            class: 'flex items-center justify-between mb-3 pb-3 border-b border-primary-600/60',
        },
        title: {
            class: 'flex gap-1 items-center',
        },
        pcPrevButton: {
            root: {
                class: 'w-8 h-8 flex items-center justify-center text-primary-400 hover:text-white hover:bg-primary-900 rounded-lg cursor-pointer transition-all',
            },
        },
        pcNextButton: {
            root: {
                class: 'w-8 h-8 flex items-center justify-center text-primary-400 hover:text-white hover:bg-primary-900 rounded-lg cursor-pointer transition-all',
            },
        },
        selectMonth: {
            class: 'text-white font-semibold text-sm cursor-pointer hover:text-accent-500 transition-colors',
        },
        selectYear: {
            class: 'text-primary-400 text-sm cursor-pointer hover:text-accent-500 transition-colors',
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
        weekDay: {
            class: 'text-primary-500 text-xs font-medium uppercase tracking-wide',
        },
        tableBodyRow: {
            class: '',
        },
        dayCell: {
            class: 'p-0.5 text-center',
        },
        day: ({ context }: { context: { selected: boolean; today: boolean; disabled: boolean } }) => ({
            class: [
                'w-8 h-8 rounded-lg flex items-center justify-center text-sm mx-auto transition-all',
                context.disabled
                    ? 'text-primary-700 cursor-not-allowed'
                    : context.selected
                      ? 'button-primary text-white font-semibold cursor-pointer'
                      : context.today
                        ? 'border border-accent-500/50 text-accent-400 hover:bg-accent-500/15 hover:text-white cursor-pointer'
                        : 'text-primary-300 hover:bg-accent-500/15 hover:text-white cursor-pointer',
            ].join(' '),
        }),
    };
</script>
