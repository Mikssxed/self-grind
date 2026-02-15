<template>
    <component
        :is="tag"
        :class="classes"
        ><slot
    /></component>
</template>
<script lang="ts" setup>
    import { twMerge } from 'tailwind-merge';
    import { computed } from 'vue';

    type HeaderTag = 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6';

    type HeaderVariant = 'primary' | 'gradient';

    interface BaseHeaderProps {
        tag?: HeaderTag;
        variant?: HeaderVariant;
    }

    const props = withDefaults(defineProps<BaseHeaderProps>(), {
        tag: 'h1',
        variant: 'primary',
    });

    const tagClassess: Record<HeaderTag, string> = {
        h1: 'text-4xl leading-[1.2]',
        h2: 'text-3xl leading-[1.2]',
        h3: 'text-2xl leading-[1.2]',
        h4: 'text-xl leading-[1.2]',
        h5: 'text-lg leading-[1.2]',
        h6: 'text-base leading-[1.2]',
    };

    const variantClasses = computed(() => {
        return twMerge('font-bold', props.variant === 'gradient' && 'text-gradient-accent');
    });

    const classes = computed(() => {
        return twMerge(tagClassess[props.tag], variantClasses.value);
    });
</script>
