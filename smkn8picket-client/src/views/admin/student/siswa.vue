<template>
  <AdminLayout>
    <StudentsComponent></StudentsComponent>
  </AdminLayout>
</template>

<script setup lang="ts">
import StudentsComponent from '../../shared/StudentsComponent.vue';
import AdminLayout from '@/components/layouts/AdminLayout.vue';
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
