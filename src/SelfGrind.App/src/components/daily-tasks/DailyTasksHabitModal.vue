<template>
    <BaseModal
        :isOpen="isOpen"
        :header="modalHeader"
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
                    {{ submitLabel }}
                </BaseButton>
            </div>
        </form>
    </BaseModal>
</template>

<script setup lang="ts">
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { toTypedSchema } from '@vee-validate/zod';
    import {watch, computed } from 'vue';
    import { habitSchema } from '@/schemas/habitSchema';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import TextField from '@/components/form/TextField.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import { useHabitModal } from '@/composables/useHabitModal';
    import { useHabits } from '@/composables/useHabits';
    import type {CreateHabitCommand, UpdateHabitCommand} from "@/api/apiClient/models";

    const { isOpen, editingHabit, close } = useHabitModal();
    const { createMutation, updateMutation, deleteMutation } = useHabits();

    const isEditing = computed(() => editingHabit.value !== null);
    const modalHeader = computed(() => isEditing.value ? 'Edit Habit' : 'Add New Habit');
    const submitLabel = computed(() => isEditing.value ? 'Save Changes' : 'Add Habit');

    const schema = toTypedSchema(habitSchema);

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
            await updateHabit(formValues);
        } else {
            await createHabit(formValues);
        }
        close();
    });
    
    const updateHabit = async (formValues: UpdateHabitCommand) => {
      await updateMutation.mutateAsync({
        id: editingHabit.value.id,
        command: { name: formValues.name, targetValue: formValues.targetValue, unit: formValues.unit },
      });
    }
    
    const createHabit = async (formValues: CreateHabitCommand) => {
      await createMutation.mutateAsync({
        name: formValues.name,
        targetValue: formValues.targetValue,
        unit: formValues.unit,
      });
    }}

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
