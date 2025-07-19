<template>
  <WaliKelasLayout>
    <ClassRoomDetailComponent :classroom-id="classroomId"></ClassRoomDetailComponent>
  </WaliKelasLayout>
</template>

<script setup lang="ts">
import ClassRoomDetailComponent from '@/views/shared/ClassRoomDetailComponent.vue';
import WaliKelasLayout from '@/components/layouts/WaliKelasLayout.vue';
import { useRoute } from 'vue-router';


const route = useRoute();

const classroomId = Number(route.params.id);

</script>

<style scoped></style>
