<template>
    <div class="flex flex-col gap-3">
        <div class="flex items-center gap-2">
            <span class="text-lg">{{ emoji }}</span>
            <span class="text-sm font-bold text-white">{{ name }}</span>
        </div>
        <div class="flex items-center gap-2">
            <template
                v-for="(skill, index) in skills"
                :key="skill.name"
            >
                <CharacterSkillNode
                    :name="skill.name"
                    :status="skill.status"
                    :emoji="skill.emoji"
                    :description="skill.description"
                    class="flex-1"
                />
                <span
                    v-if="index < skills.length - 1"
                    class="text-primary-600 text-lg font-bold"
                >
                    →
                </span>
            </template>
        </div>
    </div>
</template>
<script setup lang="ts">
    import CharacterSkillNode from './CharacterSkillNode.vue';
    import type { SkillNode } from './CharacterSkillNode.vue';

    export type SkillCategoryVariant = 'error' | 'info' | 'success';

    export interface SkillCategory {
        name: string;
        emoji: string;
        variant: SkillCategoryVariant;
        skills: SkillNode[];
    }

    interface CharacterSkillCategoryProps {
        name: string;
        emoji: string;
        variant: SkillCategoryVariant;
        skills: SkillNode[];
    }

    defineProps<CharacterSkillCategoryProps>();
</script>
