<template>
  <div class="flex justify-between items-center mb-5 ">
    <div class="flex items-center space-x-4">
      <fwb-heading tag="h5">{{ props.title }}</fwb-heading>
    </div>
    <slot />
  </div>
</template>

<script setup lang="ts">
import { FwbHeading } from "flowbite-vue";

const props = defineProps<{
  title: string;
}>();
</script>

<style scoped></style>
