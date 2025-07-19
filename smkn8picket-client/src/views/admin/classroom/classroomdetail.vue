<template>
  <AdminLayout>
    <ClassRoomDetailComponent :classroom-id="classroomId"></ClassRoomDetailComponent>
  </AdminLayout>

</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import ClassRoomDetailComponent from '@/views/shared/ClassRoomDetailComponent.vue'

const route = useRoute()
const classroomId = Number(route.params.id)

</script>
<style scoped></style>
