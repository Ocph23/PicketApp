<template>
  <WaliKelasLayout>
    <ClassRoomAbsenComponent :classroom-id="Number($route.params.id)" :classroom-name="$route.params.name" />
  </WaliKelasLayout>
</template>

<script setup lang="ts">
import WaliKelasLayout from '@/components/layouts/WaliKelasLayout.vue';
import ClassRoomAbsenComponent from '../shared/ClassRoomAbsenComponent.vue';


</script>

<style scoped></style>
