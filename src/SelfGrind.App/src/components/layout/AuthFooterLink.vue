<template>
    <p class="text-center text-sm text-primary-400">
        {{ prompt }}
        <RouterLink
            :to="{ name: to }"
            class="font-medium text-accent-500 transition-colors hover:text-accent-500/80"
        >
            {{ linkText }}
        </RouterLink>
    </p>
</template>
<script setup lang="ts">
    import type { AppRouteNames } from '@/router';
    import { RouterLink } from 'vue-router';

    interface AuthFooterLinkProps {
        prompt: string;
        linkText: string;
        to: AppRouteNames;
    }

    defineProps<AuthFooterLinkProps>();
</script>
