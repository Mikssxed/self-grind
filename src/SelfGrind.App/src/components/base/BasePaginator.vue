<script lang="ts" setup>
    import { computed } from 'vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import BaseText from '@/components/base/BaseText.vue';

    interface Props {
        page: number;
        totalPages: number;
    }

    const props = defineProps<Props>();
    const emit = defineEmits<{ 'update:page': [page: number] }>();

    const shouldRender = computed(() => props.totalPages > 1);
    const isPrevDisabled = computed(() => props.page <= 1);
    const isNextDisabled = computed(() => props.page >= props.totalPages);
    const label = computed(() => `Page ${props.page} of ${props.totalPages}`);

    function goPrev() {
        if (!isPrevDisabled.value) emit('update:page', props.page - 1);
    }

    function goNext() {
        if (!isNextDisabled.value) emit('update:page', props.page + 1);
    }
</script>

<template>
    <div
        v-if="shouldRender"
        class="flex items-center justify-center gap-4 pt-4"
    >
        <BaseButton
            variant="secondary"
            size="sm"
            :disabled="isPrevDisabled"
            @click="goPrev"
        >
            ‹ Prev
        </BaseButton>
        <BaseText class="text-sm">{{ label }}</BaseText>
        <BaseButton
            variant="secondary"
            size="sm"
            :disabled="isNextDisabled"
            @click="goNext"
        >
            Next ›
        </BaseButton>
    </div>
</template>
