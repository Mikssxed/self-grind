<template>
    <div class="input-default">
        <input
            :type="type"
            v-model="value"
            :class="inputClasses"
        />
    </div>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { useField } from 'vee-validate';
    import { computed } from 'vue';
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
            'w-full px-4 py-2 h-full outline-none bg-transparent',
            props.iconName && 'pl-12'
        );
    });
</script>
