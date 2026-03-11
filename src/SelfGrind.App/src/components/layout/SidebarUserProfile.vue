<template>
    <div class="px-4 pt-4 border-t border-t-primary-900">
        <div
            class="flex items-center gap-3 rounded-xl px-3 py-2 cursor-pointer transition-colors duration-200 hover:bg-primary-800"
            @click="toggleMenu"
        >
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
        <BasePopoverMenu
            ref="menuRef"
            :items="menuItems"
        />
    </div>
</template>
<script setup lang="ts">
    import { useLogout } from '@/composables/useAuth';
    import { ref } from 'vue';
    import BaseAvatar from '../base/BaseAvatar.vue';
    import BaseHeader from '../base/BaseHeader.vue';
    import type { PopoverMenuItem } from '../base/BasePopoverMenu.vue';
    import BasePopoverMenu from '../base/BasePopoverMenu.vue';
    import BaseText from '../base/BaseText.vue';

    const logout = useLogout();
    const menuRef = ref<InstanceType<typeof BasePopoverMenu> | null>(null);

    const menuItems: PopoverMenuItem[] = [
        {
            label: 'Logout',
            icon: 'logout',
            variant: 'danger',
            action: () => logout(),
        },
    ];

    function toggleMenu(event: Event) {
        menuRef.value?.toggle(event);
    }
</script>
