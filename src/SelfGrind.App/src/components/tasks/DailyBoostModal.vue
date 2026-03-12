<template>
    <BaseModal
        :isOpen="isOpen"
        header="Daily Boost"
        @update:isOpen="handleVisibilityChange"
    >
        <div class="flex flex-col items-center gap-6 py-4">
            <div class="text-6xl">{{ boostEmoji }}</div>

            <div class="text-center">
                <p class="text-3xl font-bold text-gradient-accent">
                    +{{ boostXp }} XP
                </p>
                <p class="text-sm text-primary-400 mt-1">
                    {{ boostMessage }}
                </p>
            </div>

            <div class="bg-gradient-quote rounded-xl p-4 w-full text-center">
                <p class="text-primary-200 italic text-sm">
                    "{{ quoteText }}"
                </p>
                <p class="text-primary-500 text-xs mt-2">
                    — {{ quoteAuthor }}
                </p>
            </div>

            <BaseButton
                variant="primary"
                class="w-full py-3"
                @click="claimBoost"
            >
                Claim Boost
            </BaseButton>
        </div>
    </BaseModal>
</template>
<script setup lang="ts">
    import { computed, watch, ref } from 'vue';
    import BaseModal from '@/components/base/BaseModal.vue';
    import BaseButton from '@/components/base/BaseButton.vue';
    import { useDailyBoostModal } from '@/composables/useDailyBoostModal';

    const { isOpen, close } = useDailyBoostModal();

    const boostXp = ref(0);
    const boostIndex = ref(0);

    const quotes = [
        { text: 'The only way to do great work is to love what you do.', author: 'Steve Jobs' },
        { text: 'Discipline is the bridge between goals and accomplishment.', author: 'Jim Rohn' },
        { text: 'Success is the sum of small efforts repeated day in and day out.', author: 'Robert Collier' },
        { text: 'The secret of getting ahead is getting started.', author: 'Mark Twain' },
        { text: 'It does not matter how slowly you go as long as you do not stop.', author: 'Confucius' },
    ];

    const boostEmojis = ['🚀', '⚡', '🔥', '💎', '🌟'];

    const boostMessages = [
        'Your dedication is paying off!',
        'Keep pushing your limits!',
        'Champions never rest!',
        'Consistency is your superpower!',
        'Every day you grow stronger!',
    ];

    const quoteText = computed(() => quotes[boostIndex.value % quotes.length]?.text ?? '');
    const quoteAuthor = computed(() => quotes[boostIndex.value % quotes.length]?.author ?? '');
    const boostEmoji = computed(() => boostEmojis[boostIndex.value % boostEmojis.length]);
    const boostMessage = computed(() => boostMessages[boostIndex.value % boostMessages.length]);

    function randomizeBoost() {
        boostXp.value = Math.floor(Math.random() * 4 + 1) * 25;
        boostIndex.value = Math.floor(Math.random() * quotes.length);
    }

    watch(isOpen, (open) => {
        if (open) {
            randomizeBoost();
        }
    });

    function claimBoost() {
        console.log('Daily Boost Claimed:', {
            xp: boostXp.value,
            quote: quoteText.value,
            author: quoteAuthor.value,
        });
        close();
    }

    function handleVisibilityChange(visible: boolean) {
        if (!visible) {
            close();
        }
    }
</script>
