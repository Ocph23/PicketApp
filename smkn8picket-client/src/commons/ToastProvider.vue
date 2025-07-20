<!-- Toast.vue -->
<template>
  <div class="toast">
    <div v-for="toast in toastList" :key="toast.id" class="flex flex-col">
      <ToastView :message="toast.message" :type="toast.type" :duration="toast.duration"></ToastView>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ToastService } from "@/services";
import { ToastView } from "@/commons";
import { computed } from "vue";

const toastList = computed(() => ToastService.toasts);

</script>

<style scoped>
.toast {
  position: fixed;
  top: 10px;
  right: 10px;
  padding: 10px 20px;
  border-radius: 5px;
  color: white;
  font-size: 16px;
  cursor: pointer;
  transition: opacity 0.3s ease;
  z-index: 9999999;
}
</style>
