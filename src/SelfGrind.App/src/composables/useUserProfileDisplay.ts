import { computed } from 'vue';
import { useCharacterHero } from './useCharacter';
import { useCurrentUser } from './useCurrentUser';

/**
 * Derived display values for the logged-in user shown in the sidebar / mobile profile.
 * Username comes from the identity endpoint; level + title come from the character hero.
 */
export function useUserProfileDisplay() {
    const { currentUser } = useCurrentUser();
    const { hero } = useCharacterHero();

    const displayName = computed(() => currentUser.value?.username ?? 'Adventurer');
    const levelLabel = computed(
        () => `Level ${hero.value?.level ?? 1} ${hero.value?.stageName ?? 'Adventurer'}`
    );

    return {
        displayName,
        levelLabel,
    };
}
