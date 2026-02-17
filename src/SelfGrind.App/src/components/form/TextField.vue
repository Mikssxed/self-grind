<template>
    <FormField
        :label="label"
        :name="name"
        :required="required"
        :error="error"
    >
        <TextInput
            :name="name"
            type="text"
            :iconName="iconName"
        />
    </FormField>
</template>
<script setup lang="ts">
    import type { IconName } from '../icons';
    import FormField from './FormField.vue';
    import TextInput from './TextInput.vue';

    export interface TextFieldProps {
        label: string;
        name: string;
        required?: boolean;
        error?: string;
        iconName?: IconName;
    }

    defineProps<TextFieldProps>();
</script>
