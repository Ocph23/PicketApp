<template>
  <AdminLayout>
    <ClassRoomAbsenComponent :classroom-id="Number($route.params.id)" :classroom-name="$route.params.name" />
  </AdminLayout>
</template>

<script setup lang="ts">
import AdminLayout from '@/components/layouts/AdminLayout.vue';
import ClassRoomAbsenComponent from '../../shared/ClassRoomAbsenComponent.vue';


</script>

<style scoped></style>
