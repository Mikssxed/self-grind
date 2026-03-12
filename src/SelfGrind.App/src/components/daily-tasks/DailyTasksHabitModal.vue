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
                name="label"
                :required="true"
            />

            <TextField
                label="Emoji Icon"
                name="emoji"
                :required="true"
            />

            <div class="flex gap-3">
                <div class="flex-1">
                    <RangeSliderField
                        label="Target"
                        name="target"
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

            <ToggleButtonGroup
                label="Color"
                name="variant"
                :options="variantOptions"
            />

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
    import ToggleButtonGroup from '@/components/form/ToggleButtonGroup.vue';
    import type { ToggleOption } from '@/components/form/ToggleButtonGroup.vue';
    import RangeSliderField from '@/components/form/RangeSliderField.vue';
    import { useHabitModal } from '@/composables/useHabitModal';

    const { isOpen, editingHabit, close } = useHabitModal();

    const isEditing = computed(() => editingHabit.value !== null);

    const variantOptions: ToggleOption[] = [
        { label: 'Blue', value: 'info' },
        { label: 'Green', value: 'success' },
        { label: 'Yellow', value: 'warning' },
        { label: 'Purple', value: 'violet' },
    ];

    const schema = toTypedSchema(
        object({
            label: string().min(1, 'Habit name is required'),
            emoji: string().min(1, 'Emoji is required'),
            target: number().min(1).max(20),
            unit: string(),
            variant: string(),
        })
    );

    const { handleSubmit, resetForm, setValues } = useVeeValidateForm({
        validationSchema: schema,
        initialValues: {
            label: '',
            emoji: '',
            target: 5,
            unit: '',
            variant: 'info',
        },
    });

    const onSubmit = handleSubmit((values) => {
        if (isEditing.value) {
            console.log('Update Habit:', values);
        } else {
            console.log('New Habit:', values);
        }
        close();
    });

    const onDelete = () => {
        console.log('Delete Habit:', editingHabit.value);
        close();
    };

    watch(isOpen, (open) => {
        if (open && editingHabit.value) {
            setValues({
                label: editingHabit.value.label,
                emoji: editingHabit.value.emoji,
                target: 5,
                unit: '',
                variant: editingHabit.value.variant,
            });
        } else if (open) {
            resetForm();
        }
    });

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
