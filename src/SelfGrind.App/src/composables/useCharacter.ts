import type {
    CharacterHeroDto,
    EquippedItemDto,
    EvolutionTierDto,
    InventoryItemDto,
    SkillCategoryDto,
} from '@/api/apiClient/models';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import { computed } from 'vue';
import { useApiClient } from './useApiClient';

const inventoryQueryKey = ['character', 'inventory'] as const;
const equippedItemsQueryKey = ['character', 'equipped-items'] as const;

export function useCharacterHero() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['character', 'hero'] as const,
        queryFn: (): Promise<CharacterHeroDto | undefined> =>
            apiClient.api.character.hero.get(),
    });

    return {
        hero: computed(() => query.data.value ?? undefined),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useEvolutionTiers() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['character', 'evolution-tiers'] as const,
        queryFn: (): Promise<EvolutionTierDto[] | undefined> =>
            apiClient.api.character.evolutionTiers.get(),
    });

    return {
        tiers: computed(() => query.data.value ?? []),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useSkillTrees() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: ['character', 'skill-trees'] as const,
        queryFn: (): Promise<SkillCategoryDto[] | undefined> =>
            apiClient.api.character.skillTrees.get(),
    });

    return {
        skillCategories: computed(() => query.data.value ?? []),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useInventory() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: inventoryQueryKey,
        queryFn: (): Promise<InventoryItemDto[] | undefined> =>
            apiClient.api.character.inventory.get(),
    });

    return {
        inventory: computed(() => query.data.value ?? []),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useEquippedItems() {
    const apiClient = useApiClient();

    const query = useQuery({
        queryKey: equippedItemsQueryKey,
        queryFn: (): Promise<EquippedItemDto[] | undefined> =>
            apiClient.api.character.equippedItems.get(),
    });

    return {
        equippedItems: computed(() => query.data.value ?? []),
        isLoading: computed(() => query.isLoading.value),
    };
}

export function useEquipItem() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (userItemId: string) =>
            apiClient.api.character.items.byUserItemId(userItemId).equip.post(),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: inventoryQueryKey });
            queryClient.invalidateQueries({ queryKey: equippedItemsQueryKey });
        },
    });
}

export function useUnequipItem() {
    const apiClient = useApiClient();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (userItemId: string) =>
            apiClient.api.character.items.byUserItemId(userItemId).unequip.post(),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: inventoryQueryKey });
            queryClient.invalidateQueries({ queryKey: equippedItemsQueryKey });
        },
    });
}
