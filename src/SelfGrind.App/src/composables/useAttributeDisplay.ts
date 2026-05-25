import type { QuestItemVariant } from '@/components/daily-tasks/DailyTasksQuestItem.vue';
import type { BaseAttribute } from '@/api/apiClient/models';

interface AttributeDisplay {
    label: string;
    emoji: string;
    variant: QuestItemVariant;
}

const attributeMap: Record<BaseAttribute, AttributeDisplay> = {
    Strength:   { label: 'Strength',   emoji: '💪', variant: 'error'   },
    Knowledge:  { label: 'Knowledge',  emoji: '📘', variant: 'info'    },
    Health:     { label: 'Health',     emoji: '💚', variant: 'success' },
    Discipline: { label: 'Discipline', emoji: '🛡️', variant: 'violet'  },
    Focus:      { label: 'Focus',      emoji: '👁️', variant: 'violet'  },
    Energy:     { label: 'Energy',     emoji: '⚡', variant: 'warning' },
};

const fallback: AttributeDisplay = { label: 'Unknown', emoji: '❓', variant: 'info' };

export function getAttributeDisplay(attribute: BaseAttribute | null | undefined): AttributeDisplay {
    if (!attribute) return fallback;
    return attributeMap[attribute] ?? fallback;
}
