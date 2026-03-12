<template>
    <FormField
        :label="label"
        :name="name"
        :required="required"
    >
        <TextAreaInput
            :name="name"
            :placeholder="placeholder"
            :rows="rows"
        />
    </FormField>
</template>
<script setup lang="ts">
    import FormField from './FormField.vue';
    import TextAreaInput from './TextAreaInput.vue';

    export interface TextAreaFieldProps {
        label: string;
        name: string;
        required?: boolean;
        placeholder?: string;
        rows?: number;
    }

    withDefaults(defineProps<TextAreaFieldProps>(), {
        required: false,
        placeholder: '',
        rows: 3,
    });
</script>
