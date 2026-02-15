<template>
    <label
        :for="name"
        class="text-sm font-semibold text-primary-200"
    >
        <slot />
        <span
            v-if="required"
            class="text-error-500"
            >*</span
        >
    </label>
</template>
<script setup lang="ts">
    type InputLabelProps = {
        name: string;
        required?: boolean;
    };

    defineProps<InputLabelProps>();
</script>
