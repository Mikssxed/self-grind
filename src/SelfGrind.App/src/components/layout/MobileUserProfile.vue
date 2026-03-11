<template>
    <div class="px-4 py-4 border-t border-t-primary-800">
        <div class="flex items-center gap-3 px-3 py-2">
            <BaseAvatar
                emoji="⚔️"
                size="lg"
            />
            <div class="min-w-0">
                <BaseHeader
                    tag="h6"
                    class="text-sm truncate"
                >
                    Alex Chen
                </BaseHeader>
                <BaseText class="text-xs">Level 42 Warrior</BaseText>
            </div>
        </div>
        <button
            class="flex items-center justify-center gap-2 w-full mt-3 px-3 py-2.5 text-sm text-error-500 rounded-lg bg-error-500/10 hover:bg-error-500/20 transition-colors duration-150 cursor-pointer"
            @click="handleLogout"
        >
            <BaseIcon
                name="logout"
                class="w-4 h-4"
            />
            Logout
        </button>
    </div>
</template>
<script setup lang="ts">
    import BaseAvatar from '../base/BaseAvatar.vue';
    import BaseHeader from '../base/BaseHeader.vue';
    import BaseIcon from '../base/BaseIcon.vue';
    import BaseText from '../base/BaseText.vue';
    import { useLogout } from '@/composables/useAuth';

    const logout = useLogout();

    function handleLogout() {
        logout();
    }
</script>
