<template>
    <component
        :is="iconComponent"
        fill="currentColor"
        :preserveAspectRatio="preserveAspectRatio"
    />
</template>
<script setup lang="ts">
    import icons, { type IconName } from '@/components/icons';
    import { computed } from 'vue';

    export type UIconName = IconName;
    export type PreserveAspectRatio =
        | 'none'
        | 'xMinYMin meet'
        | 'xMidYMin meet'
        | 'xMaxYMin meet'
        | 'xMinYMid meet'
        | 'xMidYMid meet'
        | 'xMaxYMid meet'
        | 'xMinYMax meet'
        | 'xMidYMax meet'
        | 'xMaxYMax meet'
        | 'xMinYMin slice'
        | 'xMidYMin slice'
        | 'xMaxYMin slice'
        | 'xMinYMid slice'
        | 'xMidYMid slice'
        | 'xMaxYMid slice'
        | 'xMinYMax slice'
        | 'xMidYMax slice'
        | 'xMaxYMax slice';

    export interface UIconProps {
        name: UIconName;
        preserveAspectRatio?: PreserveAspectRatio;
    }

    const props = defineProps<UIconProps>();

    const iconComponent = computed(() => {
        return icons[props.name];
    });
</script>
