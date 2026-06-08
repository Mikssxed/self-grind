<template>
    <div
        class="flex flex-col items-center gap-2 p-4 rounded-2xl border min-w-28 transition-all duration-300"
        :class="containerClass"
    >
        <div
            class="flex items-center justify-center w-12 h-12 rounded-xl text-2xl"
            :class="iconBgClass"
        >
            <span v-if="isUnlocked">{{ emoji }}</span>
            <span v-else class="opacity-40">🔒</span>
        </div>
        <span
            class="text-xs font-bold text-center"
            :class="nameClass"
        >
            {{ name }}
        </span>
        <span class="text-xs text-primary-400 text-center">{{ description }}</span>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import type { SkillStatus } from '@/api/apiClient/models';
    import { SkillStatusObject } from '@/api/apiClient/models';

    interface CharacterSkillNodeProps {
        name: string;
        status: SkillStatus;
        emoji: string;
        description: string;
    }

    const props = defineProps<CharacterSkillNodeProps>();

    const containerClasses: Record<SkillStatus, string> = {
        [SkillStatusObject.Unlocked]: 'bg-accent-500/15 border-accent-500/30',
        [SkillStatusObject.Locked]: 'bg-primary-900 border-primary-800 opacity-50',
    };

    const iconBgClasses: Record<SkillStatus, string> = {
        [SkillStatusObject.Unlocked]: 'bg-accent-500/30',
        [SkillStatusObject.Locked]: 'bg-primary-800',
    };

    const nameClasses: Record<SkillStatus, string> = {
        [SkillStatusObject.Unlocked]: 'text-white',
        [SkillStatusObject.Locked]: 'text-primary-400',
    };

    const isUnlocked = computed(() => props.status === SkillStatusObject.Unlocked);
    const containerClass = computed(() => containerClasses[props.status]);
    const iconBgClass = computed(() => iconBgClasses[props.status]);
    const nameClass = computed(() => nameClasses[props.status]);
</script>
