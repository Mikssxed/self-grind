<template>
    <BaseBox>
        <BaseHeader tag="h3">Character Stats</BaseHeader>
        <div v-if="hasRows" class="flex flex-col gap-4">
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
        <BaseEmptyState
            v-else
            emoji="📊"
            title="No stats yet"
            message="Complete tasks tied to attributes to start growing your stats."
            size="sm"
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import type { AttributeStatDto } from '@/api/apiClient/models';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import { getAttributeDisplay } from '@/composables/useAttributeDisplay';
    import { computed } from 'vue';
    import DashboardStatRow from './DashboardStatRow.vue';
    import {calculatePercentage} from "@/utils";

    interface DashboardCharacterStatsProps {
        stats: AttributeStatDto[];
    }

    const props = defineProps<DashboardCharacterStatsProps>();

    const rows = computed(() =>
        props.stats.map((stat) => {
            const display = getAttributeDisplay(stat.attribute);
            const percentage =
                stat.requiredExp > 0 ? calculatePercentage(stat.exp, stat.requiredExp) : 0;
            
            const valueLabel = buildValueLabel(stat);
          
            return {
                key: stat.attribute,
                label: display.label,
                emoji: display.emoji,
                variant: display.variant,
                percentage,
                valueLabel,
            };
        })
    );
    
    const buildValueLabel = (stat: AttributeStatDto) => {
        return `Lv ${stat.level} · ${stat.exp}/${stat.requiredExp}`;
    };

    const hasRows = computed(() => rows.value.length > 0);
</script>
