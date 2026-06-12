<template>
    <div
        class="w-full max-w-md rounded-3xl border border-primary-800 bg-gradient-midnight p-6 shadow-primary"
        aria-hidden="true"
    >
        <!-- Character header -->
        <div class="flex items-center gap-4">
            <div
                class="bg-gradient-purple flex h-14 w-14 items-center justify-center rounded-2xl text-xl font-bold text-white"
            >
                {{ levelLabel }}
            </div>
            <div class="flex-1">
                <div class="flex items-center justify-between gap-2">
                    <p class="text-sm font-bold text-white">Your Character</p>
                    <span
                        class="rounded-full bg-warning-500/15 px-2.5 py-1 text-xs font-bold text-warning-500"
                    >
                        🔥 {{ streakLabel }}
                    </span>
                </div>
                <div class="mt-2 h-2.5 w-full overflow-hidden rounded-full bg-primary-800">
                    <div
                        class="bg-gradient-accent h-full rounded-full"
                        :style="xpBarStyle"
                    ></div>
                </div>
                <p class="mt-1.5 text-xs text-primary-400">{{ xpLabel }}</p>
            </div>
        </div>

        <!-- Quest list -->
        <p class="mt-6 mb-3 text-xs font-semibold tracking-wide text-primary-400 uppercase">
            Today's quests
        </p>
        <ul class="space-y-2.5">
            <li
                v-for="quest in displayQuests"
                :key="quest.label"
                class="flex items-center gap-3 rounded-xl border border-primary-800 bg-primary-900/60 p-3"
            >
                <span :class="quest.checkClasses">
                    <span
                        v-if="quest.done"
                        class="text-xs"
                        >✓</span
                    >
                </span>
                <span :class="quest.labelClasses">{{ quest.label }}</span>
                <span
                    class="ml-auto rounded-full bg-violet-500/15 px-2.5 py-1 text-xs font-bold text-violet-500"
                >
                    +{{ quest.xp }} XP
                </span>
            </li>
        </ul>
    </div>
</template>
<script setup lang="ts">
    import { computed } from 'vue';

    interface PreviewQuest {
        label: string;
        xp: number;
        done: boolean;
    }

    const level = 7;
    const currentXp = 740;
    const nextLevelXp = 1000;
    const streakDays = 12;

    const levelLabel = `Lv.${level}`;
    const streakLabel = `${streakDays}-day streak`;
    const xpLabel = `${currentXp} / ${nextLevelXp} XP`;
    const xpBarStyle = `width: ${Math.round((currentXp / nextLevelXp) * 100)}%`;

    const quests: PreviewQuest[] = [
        { label: 'Morning workout', xp: 50, done: true },
        { label: 'Read 20 pages', xp: 30, done: true },
        { label: 'Ship side project', xp: 80, done: false },
    ];

    const displayQuests = computed(() =>
        quests.map((quest) => ({
            ...quest,
            checkClasses: quest.done
                ? 'flex h-6 w-6 items-center justify-center rounded-md bg-success-500 text-white'
                : 'flex h-6 w-6 items-center justify-center rounded-md border-2 border-primary-600',
            labelClasses: quest.done
                ? 'text-sm text-primary-400 line-through'
                : 'text-sm text-white',
        }))
    );
</script>
