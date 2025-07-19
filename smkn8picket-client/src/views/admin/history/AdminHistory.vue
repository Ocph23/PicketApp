<template>
  <AdminLayout>
    <HostoryBase></HostoryBase>
  </AdminLayout>
</template>

<script setup lang="ts">
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import HostoryBase from '../../shared/HistoryBase.vue'
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
