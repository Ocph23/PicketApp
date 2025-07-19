<template>
  <PicketLayout>
    <HistoryBase></HistoryBase>
  </PicketLayout>
</template>

<script setup lang="ts">
import PicketLayout from '@/components/layouts/PicketLayout.vue'
import HistoryBase from '../shared/HistoryBase.vue';
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
