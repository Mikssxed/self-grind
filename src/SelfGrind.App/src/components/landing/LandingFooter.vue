<template>
    <footer class="border-t border-primary-800/60">
        <div
            class="mx-auto flex max-w-6xl flex-col items-center justify-between gap-4 px-6 py-10 sm:flex-row"
        >
            <div class="flex items-center gap-2.5">
                <img
                    src="/favicon.svg"
                    alt="SelfGrind logo"
                    class="h-7 w-7"
                />
                <span class="text-sm font-bold text-white">SelfGrind</span>
            </div>
            <p class="text-sm text-primary-400">Turn your tasks into XP.</p>
            <div class="flex items-center gap-5 text-sm">
                <RouterLink
                    :to="{ name: 'login' }"
                    class="text-primary-200 transition-colors hover:text-white"
                >
                    Sign in
                </RouterLink>
                <RouterLink
                    :to="{ name: 'register' }"
                    class="text-primary-200 transition-colors hover:text-white"
                >
                    Create account
                </RouterLink>
            </div>
        </div>
        <p class="pb-8 text-center text-xs text-primary-600">© {{ year }} SelfGrind</p>
    </footer>
</template>
<script setup lang="ts">
    const year = new Date().getFullYear();
</script>
