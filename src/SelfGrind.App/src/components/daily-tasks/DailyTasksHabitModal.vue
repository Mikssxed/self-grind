<template>
    <BaseModal
        :isOpen="isOpen"
        :header="isEditing ? 'Edit Habit' : 'Add New Habit'"
        @update:isOpen="handleVisibilityChange"
    >
        <form
            class="flex flex-col gap-5"
            @submit.prevent="onSubmit"
        >
            <TextField
                label="Habit Name"
                name="name"
                :required="true"
            />

            <div class="flex gap-3">
                <div class="flex-1">
                    <RangeSliderField
                        label="Target"
                        name="targetValue"
                        :min="1"
                        :max="20"
                        :step="1"
                    />
                </div>
                <div class="flex-1">
                    <TextField
                        label="Unit"
                        name="unit"
                    />
                </div>
            </div>

            <div class="flex gap-3 pt-2">
                <BaseButton
                    v-if="isEditing"
                    variant="secondary"
                    class="py-3 px-4 !bg-error-900/40 !text-error-500 !border-error-500/30"
                    type="button"
                    @click="onDelete"
                >
                    Delete
                </BaseButton>
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
                    {{ isEditing ? 'Save Changes' : 'Add Habit' }}
                </BaseButton>
            </div>
        </form>
    </BaseModal>
</template>

<script setup lang="ts">
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import { object, string, number } from 'zod';
    import { watch, computed } from 'vue';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import { useHabitModal } from '@/composables/useHabitModal';
    import { useHabits } from '@/composables/useHabits';

    const { isOpen, editingHabit, close } = useHabitModal();
    const { createMutation, updateMutation, deleteMutation } = useHabits();

    const isEditing = computed(() => editingHabit.value !== null);

    const schema = toTypedSchema(
        object({
            name: string().min(1, 'Habit name is required'),
            targetValue: number().min(1).max(20),
            unit: string(),
        })
    );

    const { handleSubmit, resetForm, setValues } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: {
            name: '',
            targetValue: 5,
            unit: '',
        },
    });

    const onSubmit = handleSubmit(async (formValues) => {
        if (isEditing.value && editingHabit.value?.id) {
            await updateMutation.mutateAsync({
                id: editingHabit.value.id,
                command: { name: formValues.name, targetValue: formValues.targetValue, unit: formValues.unit },
            });
        } else {
            await createMutation.mutateAsync({
                name: formValues.name,
                targetValue: formValues.targetValue,
                unit: formValues.unit,
            });
        }
        close();
    });

    const onDelete = async () => {
        if (editingHabit.value?.id) {
            await deleteMutation.mutateAsync(editingHabit.value.id);
        }
        close();
    };

    watch(isOpen, (open) => {
        if (open && editingHabit.value) {
            setValues({
                name: editingHabit.value.name ?? '',
                targetValue: editingHabit.value.targetValue ?? 5,
                unit: editingHabit.value.unit ?? '',
            });
        } else if (open) {
            resetForm();
        }
    });

    function handleVisibilityChange(visible: boolean) {
        if (!visible) close();
    }
</script>
