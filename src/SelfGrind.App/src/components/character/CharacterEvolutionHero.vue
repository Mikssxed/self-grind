<template>
    <BaseBox>
        <BaseHeader tag="h3">Character Evolution</BaseHeader>
        <div class="flex flex-col md:flex-row gap-6 items-start">
            <!-- Avatar -->
            <div class="relative shrink-0">
                <div
                    class="bg-gradient-purple rounded-3xl w-32 h-32 md:w-44 md:h-44 flex items-center justify-center"
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

                <span class="text-xs text-primary-400">{{ nextEvolutionDisplay }}</span>

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
                        v-for="stat in mappedStats"
                        :key="stat.label"
                        :emoji="stat.emoji"
                        :label="stat.label"
                        :value="stat.value"
                        :variant="stat.cardVariant"
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
    import type { HeroStatDto, HeroStatVariant } from '@/api/apiClient/models';
    import { HeroStatVariantObject } from '@/api/apiClient/models';
    import { computed } from 'vue';

    interface CharacterEvolutionHeroProps {
        level: number;
        stageName: string;
        nextEvolutionLabel?: string | null;
        xpCurrent: number;
        xpMax: number;
        stats: HeroStatDto[];
    }

    const props = defineProps<CharacterEvolutionHeroProps>();

    const statCardVariants: Record<HeroStatVariant, StatCardVariant> = {
        [HeroStatVariantObject.DefaultEscaped]: 'default',
        [HeroStatVariantObject.Info]: 'info',
        [HeroStatVariantObject.Violet]: 'violet',
    };

    const xpProgressLabel = computed(
        () => `${props.xpCurrent.toLocaleString()} / ${props.xpMax.toLocaleString()}`
    );

    const xpPercentage = computed(() =>
        props.xpMax > 0 ? (props.xpCurrent / props.xpMax) * 100 : 0
    );

    const nextEvolutionDisplay = computed(() =>
        props.nextEvolutionLabel
            ? `Next evolution at ${props.nextEvolutionLabel}`
            : 'You have reached the highest evolution stage'
    );

    const mappedStats = computed(() =>
        props.stats.map(stat => ({
            emoji: stat.emoji,
            label: stat.label,
            value: stat.value,
            cardVariant: statCardVariants[stat.variant],
        }))
    );
</script>
