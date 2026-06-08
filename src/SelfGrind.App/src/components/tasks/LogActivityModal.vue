<template>
    <BaseModal
        :isOpen="isOpen"
        header="Log Activity"
        @update:isOpen="handleVisibilityChange"
    >
        <form
            class="flex flex-col gap-5"
            @submit.prevent="onSubmit"
        >
            <TextField
                label="What did you do?"
                name="activityName"
                :required="true"
            />

            <TextAreaField
                label="Notes (optional)"
                name="notes"
                placeholder="How did it go?"
                :rows="2"
            />

            <RangeSliderField
                label="XP Earned"
                name="exp"
                :min="5"
                :max="100"
                :step="5"
                unit="XP"
            />

            <AttributeSelector name="attribute" />

            <div class="flex gap-3 pt-2">
                <BaseButton
                    variant="secondary"
                    class="flex-1 py-3"
                    type="button"
                    :disabled="isSubmitting"
                    @click="close"
                >
                    Cancel
                </BaseButton>
                <BaseButton
                    variant="primary"
                    class="flex-1 py-3"
                    type="submit"
                    :disabled="isSubmitting"
                >
                    {{ submitLabel }}
                </BaseButton>
            </div>
        </form>
    </BaseModal>
</template>
<script setup lang="ts">
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import { computed, watch } from 'vue';
    import { logActivitySchema } from '@/schemas/logActivitySchema';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import TextAreaField from '@/components/form/TextAreaField.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import AttributeSelector from '@/components/form/AttributeSelector.vue';
    import { useLogActivityModal } from '@/composables/useLogActivityModal';
    import { useTasks } from '@/composables/useTasks';
    import type { BaseAttribute } from '@/api/apiClient/models';

    const { isOpen, close } = useLogActivityModal();
    const { logActivityMutation } = useTasks();

    const schema = toTypedSchema(logActivitySchema);

    const { handleSubmit, resetForm } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: {
            activityName: '',
            notes: '',
            exp: 10,
            attribute: undefined,
        },
    });

    const isSubmitting = computed(() => logActivityMutation.isPending.value);
    const submitLabel = computed(() => (isSubmitting.value ? 'Logging...' : 'Log Activity'));

    const onSubmit = handleSubmit(values => {
        logActivityMutation.mutate(
            {
                title: values.activityName,
                notes: values.notes ?? null,
                exp: values.exp,
                attribute: values.attribute as BaseAttribute,
            },
            {
                onSuccess: () => close(),
            }
        );
    });

    watch(isOpen, open => {
        if (open) resetForm();
    });

    function handleVisibilityChange(visible: boolean) {
        if (!visible) close();
    }
</script>
