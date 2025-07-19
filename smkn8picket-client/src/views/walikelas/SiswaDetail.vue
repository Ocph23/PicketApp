<template>
  <WaliKelasLayout>
    <DetailSiswaComponent :student-id="studentId"></DetailSiswaComponent>
  </WaliKelasLayout>

</template>

<script setup lang="ts">
import DetailSiswaComponent from '@/views/shared/DetailSiswaComponent.vue';
import WaliKelasLayout from '@/components/layouts/WaliKelasLayout.vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const studentId = Number(route.params.id);
</script>

<style scoped></style>
