<template>
    <div class="flex flex-col gap-2">
        <slot name="label">
            <InputLabel
                v-if="label"
                :name="name"
                :required="required"
            >
                {{ label }}
            </InputLabel>
        </slot>
        <slot name="default" />
        <FormFieldError v-show="fieldError">
            {{ fieldError }}
        </FormFieldError>
    </div>
</template>
<script setup lang="ts">
    import { useFieldError } from 'vee-validate';
    import FormFieldError from './FormFieldError.vue';
    import InputLabel from './InputLabel.vue';

    export interface FormFieldProps {
        label: string;
        name: string;
        required?: boolean;
        error?: string;
    }

    const props = defineProps<FormFieldProps>();

    const fieldError = useFieldError(() => props.name);
</script>
