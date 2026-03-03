<template>
    <BaseBox class="col-span-2">
        <div class="flex gap-6 items-start">
            <!-- Avatar -->
            <div class="relative shrink-0">
                <div
                    class="bg-gradient-purple rounded-3xl w-48 h-48 flex items-center justify-center"
                >
                    <BaseIcon
                        name="user"
                        class="w-16 h-16 text-white"
                    />
                </div>
                <BaseBadge
                    variant="accent"
                    size="medium"
                    class="absolute -top-3 -right-3"
                    >Lv {{ level }}</BaseBadge
                >
            </div>

            <!-- Character Info -->
            <div class="flex flex-col gap-4 flex-1">
                <div>
                    <BaseHeader tag="h2">Your Character</BaseHeader>
                    <BaseText>{{ title }} • {{ streakDays }} Day Streak 🔥</BaseText>
                </div>

                <!-- XP Progress -->
                <div class="flex flex-col gap-2">
                    <div class="flex justify-between">
                        <span class="text-sm text-primary-400">XP Progress</span>
                        <span class="text-sm font-bold text-accent-500"
                            >{{ xpProgressLabel }}</span
                        >
                    </div>
                    <BaseProgressBar :percentage="xpPercentage" />
                </div>

                <!-- Streak + Achievements -->
                <div class="grid grid-cols-2 gap-4">
                    <BaseStatCard
                        emoji="🔥"
                        label="Streak"
                        :value="streakValue"
                        variant="info"
                    />
                    <BaseStatCard
                        emoji="🏆"
                        label="Achievements"
                        :value="achievementsValue"
                        variant="violet"
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
    import BaseText from '@/components/base/BaseText.vue';
    import { computed } from 'vue';

    interface DashboardCharacterProps {
        level: number;
        title: string;
        streakDays: number;
        xpCurrent: number;
        xpMax: number;
        achievementsCount: number;
        achievementsTotal: number;
    }

    const props = defineProps<DashboardCharacterProps>();

    const xpProgressLabel = computed(
        () => `${props.xpCurrent.toLocaleString()} / ${props.xpMax.toLocaleString()}`
    );

    const xpPercentage = computed(() => (props.xpCurrent / props.xpMax) * 100);

    const streakValue = computed(() => `${props.streakDays} Days`);

    const achievementsValue = computed(
        () => `${props.achievementsCount} / ${props.achievementsTotal}`
    );
</script>
