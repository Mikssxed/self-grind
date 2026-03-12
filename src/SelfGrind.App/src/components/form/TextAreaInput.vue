<template>
    <div class="input-default">
        <textarea
            v-model="value"
            :name="name"
            :placeholder="placeholder"
            :rows="rows"
            class="w-full px-4 py-3 outline-none bg-transparent resize-none"
        />
    </div>
</template>
<script setup lang="ts">
    import { useField } from 'vee-validate';

    export interface TextAreaInputProps {
        name: string;
        placeholder?: string;
        rows?: number;
    }

    const props = withDefaults(defineProps<TextAreaInputProps>(), {
        placeholder: '',
        rows: 3,
    });

    const { value } = useField<string>(() => props.name);
</script>
