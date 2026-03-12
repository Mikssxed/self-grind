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

            <TextField
                label="Duration (optional)"
                name="duration"
            />

            <RangeSliderField
                label="XP Earned"
                name="exp"
                :min="5"
                :max="50"
                :step="5"
                unit="XP"
            />

            <AttributeSelector name="attribute" />

            <div class="flex gap-3 pt-2">
                <BaseButton
                    variant="secondary"
                    class="flex-1 py-3"
                    type="button"
                    @click="close"
                >
                    Cancel
                </BaseButton>
                <BaseButton
                    variant="primary"
                    class="flex-1 py-3"
                    type="submit"
                >
                    Log Activity
                </BaseButton>
            </div>
        </form>
    </BaseModal>
</template>
<script setup lang="ts">
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import { object, string, number, enum as zEnum, optional } from 'zod';
    import { watch } from 'vue';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import TextAreaField from '@/components/form/TextAreaField.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import AttributeSelector from '@/components/form/AttributeSelector.vue';
    import { useLogActivityModal } from '@/composables/useLogActivityModal';

    const { isOpen, close } = useLogActivityModal();

    const schema = toTypedSchema(
        object({
            activityName: string().min(1, 'Activity name is required'),
            notes: optional(string()),
            duration: optional(string()),
            exp: number().min(5).max(50),
            attribute: optional(zEnum(['Strength', 'Knowledge', 'Health', 'Charisma', 'Focus', 'Creativity'])),
        })
    );

    const { handleSubmit, resetForm } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: {
            activityName: '',
            notes: '',
            duration: '',
            exp: 10,
            attribute: undefined,
        },
    });

    const onSubmit = handleSubmit((values) => {
        console.log('Logged Activity:', values);
        close();
    });

    watch(isOpen, (open) => {
        if (open) {
            resetForm();
        }
    });

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
