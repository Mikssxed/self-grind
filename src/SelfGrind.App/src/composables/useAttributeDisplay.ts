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
    Charisma:   { label: 'Charisma',   emoji: '🌟', variant: 'warning' },
    Focus:      { label: 'Focus',      emoji: '👁️', variant: 'violet'  },
    Creativity: { label: 'Creativity', emoji: '🎨', variant: 'violet'  },
};

const fallback: AttributeDisplay = { label: 'Unknown', emoji: '❓', variant: 'info' };

export function getAttributeDisplay(attribute: BaseAttribute | null | undefined): AttributeDisplay {
    if (!attribute) return fallback;
    return attributeMap[attribute] ?? fallback;
}
