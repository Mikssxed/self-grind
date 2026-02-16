<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import TextField from '@/components/form/TextField.vue';
    import GradientIcon from '@/components/GradientIcon.vue';
</script>

<template>
    <div class="w-full flex justify-center items-center pt-25">
        <div class="flex flex-col gap-8">
            <div class="flex flex-col w-md gap-4 p-2">
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
                <form>
                    <TextField
                        label="Full Name"
                        name="username"
                        required
                    />
                </form>
            </BaseBox>
        </div>
    </div>
</template>
