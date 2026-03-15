<template>
    <AuthLayout
        icon="addUser"
        title="Start Your Journey"
        subtitle="Create your account and level up!"
    >
        <BaseBox>
            <BaseForm @submit="onSubmit">
                <TextField
                    label="Username"
                    name="username"
                    iconName="user"
                    required
                />
                <TextField
                    label="Email"
                    name="email"
                    iconName="mail"
                    required
                />
                <TextField
                    label="Password"
                    name="password"
                    iconName="lock"
                    type="password"
                    required
                />
                <TextField
                    label="Confirm Password"
                    name="confirmPassword"
                    iconName="lock"
                    type="password"
                    required
                />
                <BaseButton
                    class="mt-4"
                    :disabled="form.isProcessing.value"
                >
                    {{ submitButtonLabel }}
                </BaseButton>
            </BaseForm>
        </BaseBox>
    </AuthLayout>
</template>
<script setup lang="ts">
    import AuthLayout from '@/components/layout/AuthLayout.vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseForm from '@/components/base/BaseForm.vue';
    import TextField from '@/components/form/TextField.vue';
    import { useRegisterMutation } from '@/composables/useAuth';
    import { useForm } from '@/composables/useForm';
    import { registerSchema } from '@/schemas';
    import { toTypedSchema } from '@vee-validate/zod';
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { computed } from 'vue';

    const registerMutation = useRegisterMutation();
    const form = useForm(registerMutation);

    const schema = toTypedSchema(registerSchema);
    const { handleSubmit } = useVeeValidateForm({ validationSchema: schema });

    const submitButtonLabel = computed(() =>
        form.isProcessing.value ? 'Creating account...' : 'Create account'
    );

    const onSubmit = handleSubmit(form.handler);
</script>
