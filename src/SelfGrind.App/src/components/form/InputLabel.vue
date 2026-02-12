<template>
    <label
        :for="name"
        class="text-sm font-semibold text-primary-200"
    >
        {{ label }}
        <span class="text-error-500">*</span>
    </label>
</template>
<script setup lang="ts">
    type InputLabelProps = {
        name: string;
        label: string;
    };

    defineProps<InputLabelProps>();
</script>
