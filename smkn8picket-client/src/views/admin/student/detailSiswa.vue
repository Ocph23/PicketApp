<template>
  <AdminLayout>
    <DetailSiswaComponent :student-id="studentId"></DetailSiswaComponent>
  </AdminLayout>
</template>

<script setup lang="ts">

import AdminLayout from '@/components/layouts/AdminLayout.vue';
import DetailSiswaComponent from '@/views/shared/DetailSiswaComponent.vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const studentId = Number(route.params.id);
</script>
