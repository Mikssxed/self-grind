<template>
    <div :class="classes">
        <span :class="emojiClass">{{ emoji }}</span>
    </div>
</template>
<script setup lang="ts">
    import { twMerge } from 'tailwind-merge';
    import { computed, useAttrs } from 'vue';

    export type AvatarSize = 'sm' | 'md' | 'lg';

    interface BaseAvatarProps {
        emoji: string;
        size?: AvatarSize;
    }

    const props = withDefaults(defineProps<BaseAvatarProps>(), {
        size: 'md',
    });

    const attrs = useAttrs();

    const sizeClasses: Record<AvatarSize, string> = {
        sm: 'w-8 h-8',
        md: 'w-10 h-10',
        lg: 'w-12 h-12',
    };

    const emojiSizeClasses: Record<AvatarSize, string> = {
        sm: 'text-sm',
        md: 'text-lg',
        lg: 'text-xl',
    };

    const classes = computed(() =>
        twMerge(
            'rounded-full bg-gradient-purple flex items-center justify-center shrink-0',
            sizeClasses[props.size],
            attrs.class as string,
        ),
    );

    const emojiClass = computed(() => emojiSizeClasses[props.size]);
</script>
