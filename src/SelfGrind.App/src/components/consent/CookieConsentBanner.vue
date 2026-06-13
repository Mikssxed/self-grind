<template>
    <div
        v-if="visible"
        class="fixed inset-x-0 bottom-0 z-50 p-4"
    >
        <div
            class="mx-auto flex max-w-3xl flex-col gap-4 rounded-2xl border border-primary-800 bg-primary-900 p-6 shadow-primary md:flex-row md:items-center md:justify-between"
        >
            <div class="flex flex-col gap-1">
                <BaseHeader
                    tag="h6"
                    class="text-sm"
                >
                    {{ title }}
                </BaseHeader>
                <BaseText class="text-sm text-primary-400">{{ message }}</BaseText>
            </div>
            <div class="flex shrink-0 gap-3">
                <BaseButton
                    variant="secondary"
                    size="sm"
                    @click="onReject"
                >
                    {{ rejectLabel }}
                </BaseButton>
                <BaseButton
                    variant="primary"
                    size="sm"
                    @click="onAccept"
                >
                    {{ acceptLabel }}
                </BaseButton>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseText from '@/components/base/BaseText.vue';
    import { shouldShowConsentBanner, updateAnalyticsConsent } from '@/composables/useTracking';
    import { onMounted, ref } from 'vue';

    const title = 'We value your privacy';
    const message =
        'We use cookies to measure traffic and improve SelfGrind. You can accept or reject analytics cookies.';
    const acceptLabel = 'Accept';
    const rejectLabel = 'Reject';

    const visible = ref(false);

    onMounted(() => {
        visible.value = shouldShowConsentBanner();
    });

    function onAccept() {
        updateAnalyticsConsent(true);
        visible.value = false;
    }

    function onReject() {
        updateAnalyticsConsent(false);
        visible.value = false;
    }
</script>
