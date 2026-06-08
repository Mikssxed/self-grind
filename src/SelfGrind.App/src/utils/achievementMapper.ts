import type { AchievementDto } from '@/api/apiClient/models';
import type { Achievement } from '@/components/base/BaseAchievementGrid.vue';
import type { AchievementCardVariant } from '@/components/base/BaseAchievementCard.vue';

const VALID_VARIANTS: ReadonlySet<AchievementCardVariant> = new Set([
    'orange',
    'blue',
    'green',
    'purple',
    'crimson',
]);

function normalizeVariant(value: string | null | undefined): AchievementCardVariant {
    if (value && VALID_VARIANTS.has(value as AchievementCardVariant)) {
        return value as AchievementCardVariant;
    }
    return 'blue';
}

export function mapAchievementsToCards(items: AchievementDto[] | undefined): Achievement[] {
    if (!items) return [];
    return items.map(item => ({
        emoji: item.emoji ?? '',
        label: item.label ?? '',
        subtitle: item.subtitle ?? '',
        variant: normalizeVariant(item.variant),
        locked: item.locked ?? false,
    }));
}
