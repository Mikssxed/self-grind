<template>
    <div class="w-full flex justify-center items-center pt-25">
        <div class="flex flex-col gap-8">
            <div class="flex flex-col w-md gap-4 p-2">
                <GradientIcon name="user" />
                <div class="flex flex-col gap-2">
                    <BaseHeader
                        variant="gradient"
                        class="text-center"
                    >
                        Welcome Back
                    </BaseHeader>
                    <BaseText class="text-center">Sign in to continue your journey</BaseText>
                </div>
            </div>
            <BaseBox>
                <form
                    @submit="onSubmit"
                    class="flex flex-col gap-4"
                >
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
                    <div
                        v-if="loginMutation.error.value"
                        class="text-error-500 text-sm"
                    >
                        {{
                            loginMutation.error.value.message ||
                            'Login failed. Please check your credentials.'
                        }}
                    </div>
                    <BaseButton
                        class="mt-4"
                        :disabled="loginMutation.isPending.value"
                    >
                        {{ loginMutation.isPending.value ? 'Signing in...' : 'Sign in' }}
                    </BaseButton>
                </form>
            </BaseBox>
        </div>
    </div>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import TextField from '@/components/form/TextField.vue';
    import GradientIcon from '@/components/GradientIcon.vue';
    import { useLoginMutation } from '@/composables/useAuth';
    import { toTypedSchema } from '@vee-validate/zod';
    import { useForm } from 'vee-validate';
    import { object, string } from 'zod';

    const loginSchema = object({
        email: string().email('Invalid email address'),
        password: string().min(1, 'Password is required'),
    });

    const schema = toTypedSchema(loginSchema);
    const { handleSubmit } = useForm({ validationSchema: schema });
    const loginMutation = useLoginMutation();

    const onSubmit = handleSubmit((values) => {
        loginMutation.mutate({
            email: values.email,
            password: values.password,
        });
    });
</script>
