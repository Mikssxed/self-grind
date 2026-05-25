<template>
    <BaseBox>
        <BaseHeader tag="h3">Character Stats</BaseHeader>
        <div class="flex flex-col gap-4">
            <DashboardStatRow
                v-for="row in rows"
                :key="row.key"
                :label="row.label"
                :emoji="row.emoji"
                :percentage="row.percentage"
                :valueLabel="row.valueLabel"
                :variant="row.variant"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import type { AttributeStatDto } from '@/api/apiClient/models';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import { computed } from 'vue';
    import DashboardStatRow from './DashboardStatRow.vue';

    interface DashboardCharacterStatsProps {
        stats: AttributeStatDto[];
    }

    const props = defineProps<DashboardCharacterStatsProps>();

    const rows = computed(() =>
        props.stats.map((stat) => {
            const display = getAttributeDisplay(stat.attribute);
            const percentage =
                stat.requiredExp > 0 ? (stat.exp / stat.requiredExp) * 100 : 0;
            return {
                key: stat.attribute,
                label: display.label,
                emoji: display.emoji,
                variant: display.variant,
                percentage,
                valueLabel: `Lv ${stat.level} · ${stat.exp}/${stat.requiredExp}`,
            };
        })
    );
</script>
