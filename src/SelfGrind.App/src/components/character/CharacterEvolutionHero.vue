<template>
    <BaseBox>
        <BaseHeader tag="h3">Character Evolution</BaseHeader>
        <div class="flex gap-6 items-start">
            <!-- Avatar -->
            <div class="relative shrink-0">
                <div
                    class="bg-gradient-purple rounded-3xl w-44 h-44 flex items-center justify-center"
                >
                    <BaseIcon
                        name="character"
                        class="w-14 h-14 text-white"
                    />
                </div>
                <BaseBadge
                    variant="accent"
                    size="medium"
                    class="absolute -top-3 -right-3"
                >
                    Lv {{ level }}
                </BaseBadge>
            </div>

            <!-- Character Info -->
            <div class="flex flex-col gap-4 flex-1">
                <!-- Current Stage -->
                <div class="flex items-center justify-between">
                    <span class="text-sm text-primary-400">Current Stage</span>
                    <BaseBadge variant="gradient">{{ stageName }}</BaseBadge>
                </div>

                <span class="text-xs text-primary-400">{{ nextEvolutionLabel }}</span>

                <!-- XP Progress -->
                <div class="flex flex-col gap-2">
                    <div class="flex justify-between">
                        <span class="text-sm text-primary-400">XP Progress</span>
                        <span class="text-sm font-bold text-accent-500">{{ xpProgressLabel }}</span>
                    </div>
                    <BaseProgressBar :percentage="xpPercentage" />
                </div>

                <!-- Stat Cards -->
                <div class="grid grid-cols-2 gap-3">
                    <BaseStatCard
                        v-for="stat in stats"
                        :key="stat.label"
                        :emoji="stat.emoji"
                        :label="stat.label"
                        :value="stat.value"
                        :variant="stat.variant"
                    />
                </div>
            </div>
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBadge from '@/components/base/BaseBadge.vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import BaseIcon from '@/components/base/BaseIcon.vue';
    import BaseProgressBar from '@/components/base/BaseProgressBar.vue';
    import BaseStatCard from '@/components/base/BaseStatCard.vue';
    import type { StatCardVariant } from '@/components/base/BaseStatCard.vue';
    import { computed } from 'vue';

    export interface HeroStat {
        emoji: string;
        label: string;
        value: string;
        variant: StatCardVariant;
    }

    interface CharacterEvolutionHeroProps {
        level: number;
        stageName: string;
        nextEvolution: string;
        xpCurrent: number;
        xpMax: number;
        stats: HeroStat[];
    }

    const props = defineProps<CharacterEvolutionHeroProps>();

    const xpProgressLabel = computed(
        () => `${props.xpCurrent.toLocaleString()} / ${props.xpMax.toLocaleString()}`
    );

    const xpPercentage = computed(() => (props.xpCurrent / props.xpMax) * 100);

    const nextEvolutionLabel = computed(
        () => `Next evolution at ${props.nextEvolution}`
    );
</script>
