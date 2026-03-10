<template>
    <div class="w-full flex justify-center items-center pt-25 px-4">
        <div class="flex flex-col gap-8 w-full max-w-md">
            <div class="flex flex-col gap-4 p-2">
                <GradientIcon name="addUser" />
                <div class="flex flex-col gap-2">
                    <BaseHeader
                        variant="gradient"
                        class="text-center"
                    >
                        Start Your Journey
                    </BaseHeader>
                    <BaseText class="text-center">Create your account and level up!</BaseText>
                </div>
            </div>
            <BaseBox>
                <BaseForm
                    @submit="onSubmit"
                >
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
        </div>
    </div>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseForm from '@/components/base/BaseForm.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import TextField from '@/components/form/TextField.vue';
    import GradientIcon from '@/components/GradientIcon.vue';
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
