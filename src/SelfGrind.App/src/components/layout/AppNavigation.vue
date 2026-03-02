<template>
    <div class="flex flex-col gap-2 p-4 border-b border-b-primary-900 flex-1">
        <AppNavigationItem
            v-for="item in navigationList"
            :key="item.name"
            :name="item.name"
            :iconName="item.icon"
            :isActive="item.isActive"
        />
    </div>
</template>
<script setup lang="ts">
    import useNavigation from '@/composables/useNavigation';
    import AppNavigationItem from './AppNavigationItem.vue';

    const navigation = useNavigation();
    const navigationList = navigation.navigationList;
</script>
