<template>
  <PicketLayout>
    <StudentsComponent></StudentsComponent>
  </PicketLayout>
</template>

<script setup lang="ts">
import PicketLayout from '@/components/layouts/PicketLayout.vue'
import StudentsComponent from '../shared/StudentsComponent.vue';
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
