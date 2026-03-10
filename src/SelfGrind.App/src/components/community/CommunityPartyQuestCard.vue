<script setup lang="ts">
    import { computed } from 'vue';
    import { twMerge } from 'tailwind-merge';
    import BaseAvatar from '@/components/base/BaseAvatar.vue';

    export type PartyQuestVariant = 'purple' | 'blue';

    export interface PartyQuest {
        title: string;
        description: string;
        memberEmojis: string[];
        extraMembers: number;
        progress: number;
        total: number;
        unit: string;
        reward: string;
        variant: PartyQuestVariant;
    }

    interface Props {
        quest: PartyQuest;
    }

    const props = defineProps<Props>();

    const bgClasses: Record<PartyQuestVariant, string> = {
        purple: 'bg-gradient-quest-purple-bg',
        blue: 'bg-gradient-quest-blue-bg',
    };

    const outlineClasses: Record<PartyQuestVariant, string> = {
        purple: 'outline-quest-purple-outline',
        blue: 'outline-quest-blue-outline',
    };

    const badgeClasses: Record<PartyQuestVariant, string> = {
        purple: 'bg-quest-purple-badge',
        blue: 'bg-quest-blue-badge',
    };

    const barClasses: Record<PartyQuestVariant, string> = {
        purple: 'bg-gradient-quest-purple-bar',
        blue: 'bg-gradient-quest-blue-bar',
    };

    const progressTextClasses: Record<PartyQuestVariant, string> = {
        purple: 'text-quest-purple-progress',
        blue: 'text-quest-blue-progress',
    };

    const ringClasses: Record<PartyQuestVariant, string> = {
        purple: 'shadow-[0_0_0_2px_var(--color-quest-purple-ring)]',
        blue: 'shadow-[0_0_0_2px_var(--color-quest-blue-ring)]',
    };

    const progressPercent = computed(
        () => (props.quest.progress / props.quest.total) * 100,
    );

    const progressText = computed(
        () => `${props.quest.progress} / ${props.quest.total} ${props.quest.unit}`,
    );

    const bgClass = computed(() => bgClasses[props.quest.variant]);
    const outlineClass = computed(() => outlineClasses[props.quest.variant]);
    const badgeClass = computed(() => badgeClasses[props.quest.variant]);
    const barClass = computed(() => barClasses[props.quest.variant]);
    const progressTextClass = computed(() => progressTextClasses[props.quest.variant]);
    const ringClass = computed(() => ringClasses[props.quest.variant]);

    const progressBarStyle = computed(() => ({ width: `${progressPercent.value}%` }));

    function memberAvatarClass(index: number): string {
        return twMerge(ringClass.value, index > 0 ? '-ml-1' : '');
    }
</script>

<template>
    <div
        class="rounded-2xl outline-2 -outline-offset-2 flex flex-col gap-4 p-5"
        :class="[bgClass, outlineClass]"
    >
        <div class="flex items-center justify-between gap-3">
            <span class="font-bold text-white text-base">{{ quest.title }}</span>
            <span
                class="text-xs font-bold text-white px-3 py-1 rounded-full shadow-sm shrink-0"
                :class="badgeClass"
            >
                Active
            </span>
        </div>

        <p class="text-sm text-primary-400">{{ quest.description }}</p>

        <div class="flex items-center">
            <BaseAvatar
                v-for="(emoji, index) in quest.memberEmojis"
                :key="index"
                :emoji="emoji"
                size="sm"
                :class="memberAvatarClass(index)"
            />
            <span v-if="quest.extraMembers > 0" class="text-xs text-primary-400 pl-2">
                +{{ quest.extraMembers }} more
            </span>
        </div>

        <div class="flex flex-col gap-1">
            <div class="flex items-center justify-between">
                <span class="text-xs text-primary-400">Progress</span>
                <span
                    class="text-xs font-bold"
                    :class="progressTextClass"
                >
                    {{ progressText }}
                </span>
            </div>
            <div class="w-full h-2 rounded-full bg-primary-900 overflow-hidden">
                <div
                    class="h-full rounded-full transition-all duration-500 shadow-sm"
                    :class="barClass"
                    :style="progressBarStyle"
                ></div>
            </div>
        </div>

        <span class="text-xs text-primary-600">Reward: {{ quest.reward }}</span>
    </div>
</template>
