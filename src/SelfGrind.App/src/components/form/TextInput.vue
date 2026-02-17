<template>
    <div class="input-default">
        <div
            v-if="iconName"
            class="absolute left-0 top-0 w-12 h-full flex items-center justify-center"
        >
            <BaseIcon
                :name="iconName"
                class="max-w-4 h-4"
            />
        </div>
        <input
            :type="type"
            v-model="value"
            :class="inputClasses"
            :name="name"
        />
    </div>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { useField } from 'vee-validate';
    import { computed } from 'vue';
    import BaseIcon from '../base/BaseIcon.vue';
    import type { IconName } from '../icons';

    export interface TextInputProps {
        name: string;
        type?: string;
        iconName?: IconName;
    }

    const props = withDefaults(defineProps<TextInputProps>(), {
        type: 'text',
    });

    const { value } = useField(() => props.name);

    const inputClasses = computed(() => {
        return twMerge(
            'w-full px-4 py-3 h-full outline-none bg-transparent',
            props.iconName && 'pl-12'
        );
    });
</script>
