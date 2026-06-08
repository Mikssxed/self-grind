<template>
    <BaseBox>
        <BaseHeader tag="h3">Skill Trees</BaseHeader>
        <div v-if="hasCategories" class="flex flex-col gap-6">
            <CharacterSkillCategory
                v-for="category in categories"
                :key="category.name"
                :name="category.name"
                :emoji="category.emoji"
                :variant="category.variant"
                :skills="category.skills"
            />
        </div>
        <BaseEmptyState
            v-else
            emoji="🌳"
            title="No skills available yet"
            message="Skill trees will appear as the catalog is populated."
        />
    </BaseBox>
</template>
<script setup lang="ts">
    import { computed } from 'vue';
    import BaseBox from '@/components/base/BaseBox.vue';
    import BaseEmptyState from '@/components/base/BaseEmptyState.vue';
    import BaseHeader from '@/components/base/BaseHeader.vue';
    import CharacterSkillCategory from './CharacterSkillCategory.vue';
    import type { SkillCategoryDto } from '@/api/apiClient/models';

    interface CharacterSkillTreesProps {
        categories: SkillCategoryDto[];
    }

    const props = defineProps<CharacterSkillTreesProps>();
    const hasCategories = computed(() => props.categories.length > 0);
</script>
