<script lang="ts" setup>
    import type { HabitDto } from '@/api/apiClient/models';
    import { computed } from 'vue';
    import { useHabits } from '@/composables/useHabits';

    interface Props {
        habit: HabitDto & { id: string };
    }

    const props = defineProps<Props>();
    const emit = defineEmits<{ edit: [] }>();

    const { logEntryMutation } = useHabits();

    const todayValue = computed(() => props.habit.todayValue ?? 0);
    const targetValue = computed(() => props.habit.targetValue ?? 0);

    const progressLabel = computed(() => {
        const unit = props.habit.unit ? ` ${props.habit.unit}` : '';
        return `${todayValue.value}/${targetValue.value}${unit}`;
    });

    function increment() {
        if (todayValue.value >= targetValue.value) return;
        logEntryMutation.mutate({ id: props.habit.id, value: todayValue.value + 1 });
    }

    function decrement() {
        if (todayValue.value <= 0) return;
        logEntryMutation.mutate({ id: props.habit.id, value: todayValue.value - 1 });
    }
</script>

<template>
    <div class="flex flex-col items-center gap-3 p-4 rounded-xl border-b-4 bg-primary-800/40 border-b-accent-500">
        <span
            class="text-sm text-primary-400 cursor-pointer hover:text-primary-200 transition-colors"
            @click="emit('edit')"
        >
            {{ habit.name }}
        </span>
        <span class="text-xl font-bold text-accent-500">{{ progressLabel }}</span>
        <div class="flex gap-2 w-full">
            <button
                class="flex-1 py-1 rounded-lg bg-primary-700 text-primary-300 transition-colors hover:bg-primary-600 disabled:opacity-30 disabled:cursor-not-allowed"
                :disabled="todayValue <= 0"
                @click="decrement"
            >
                −
            </button>
            <button
                class="flex-1 py-1 rounded-lg bg-accent-500/20 text-accent-400 transition-colors hover:bg-accent-500/30 disabled:opacity-30 disabled:cursor-not-allowed"
                :disabled="todayValue >= targetValue"
                @click="increment"
            >
                +
            </button>
        </div>
    </div>
</template>
