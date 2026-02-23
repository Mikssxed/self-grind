<template>
    <form
        @submit="handleSubmit"
        class="flex flex-col gap-4"
    >
        <div
            v-if="generalError"
            class="text-error-500 text-sm bg-error-50 border border-error-200 rounded-lg px-4 py-3"
        >
            {{ generalError }}
        </div>
        <slot />
    </form>
</template>

<script setup lang="ts">
    import { useFormErrors } from 'vee-validate';
    import { computed } from 'vue';

    export interface BaseFormEmits {
        (e: 'submit', event: Event): void;
    }

    const emit = defineEmits<BaseFormEmits>();
    const errors = useFormErrors();

    const generalError = computed(() => errors.value.general || null);

    const handleSubmit = (event: Event) => {
        emit('submit', event);
    };
</script>
