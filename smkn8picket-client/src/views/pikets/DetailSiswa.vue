<template>
  <PicketLayout>
    <DetailSiswaComponent :student-id="studentId"></DetailSiswaComponent>
  </PicketLayout>
</template>

<script setup lang="ts">

import PicketLayout from '@/components/layouts/PicketLayout.vue';
import DetailSiswaComponent from '@/views/shared/DetailSiswaComponent.vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const studentId = Number(route.params.id);
</script>
