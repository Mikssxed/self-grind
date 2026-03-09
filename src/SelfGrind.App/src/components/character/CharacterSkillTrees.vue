<template>
    <BaseBox>
        <BaseHeader tag="h3">Skill Trees</BaseHeader>
        <div class="flex flex-col gap-6">
            <CharacterSkillCategory
                v-for="category in categories"
                :key="category.name"
                :name="category.name"
                :emoji="category.emoji"
                :variant="category.variant"
                :skills="category.skills"
            />
        </div>
    </BaseBox>
</template>
<script setup lang="ts">
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterSkillCategory from './CharacterSkillCategory.vue';
    import type { SkillCategory } from './CharacterSkillCategory.vue';

    interface CharacterSkillTreesProps {
        categories: SkillCategory[];
    }

    defineProps<CharacterSkillTreesProps>();
</script>
