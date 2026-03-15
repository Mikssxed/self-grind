<template>
    <AuthLayout
        icon="gamepad"
        title="Welcome Back!"
        subtitle="Ready to continue your journey?"
    >
        <BaseBox>
            <BaseForm @submit="onSubmit">
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
    import { useLoginMutation } from '@/composables/useAuth';
    import { useForm } from '@/composables/useForm';
    import { toTypedSchema } from '@vee-validate/zod';
    import { useForm as useVeeValidateForm } from 'vee-validate';
    import { computed } from 'vue';
    import { object, string } from 'zod';

    const loginSchema = object({
        email: string().email('Invalid email address'),
        password: string().min(1, 'Password is required'),
    });

    const loginMutation = useLoginMutation();
    const form = useForm(loginMutation);

    const schema = toTypedSchema(loginSchema);
    const { handleSubmit } = useVeeValidateForm({ validationSchema: schema });

    const submitButtonLabel = computed(() =>
        form.isProcessing.value ? 'Signing in...' : 'Sign in'
    );

    const onSubmit = handleSubmit(form.handler);
</script>
