<template>
  <div
    v-if="showModal"
    class="fixed inset-0 z-50 bg-gray-500 bg-opacity-50 flex justify-center items-center"
  >
    <div class="bg-white p-6 rounded-lg shadow-lg">
      <p>
        Apakah Anda yakin ingin menghapus jurusan
        {{ departmentToDelete?.name }}?
      </p>
      <div class="mt-4 flex gap-4 justify-end">
        <button
          @click="cancelDelete"
          class="px-4 py-2 bg-gray-400 text-white rounded"
        >
          Batal
        </button>
        <button
          @click="deleteData"
          class="px-4 py-2 bg-red-500 text-white rounded"
        >
          Hapus
        </button>
      </div>
    </div>
  </div>
</template>
<script setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps({
  showModal: {
    type: Boolean,
    required: true,
  },
  departmentToDelete: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["cancel", "delete"]);

const cancelDelete = () => {
  emit("cancel");
};

const deleteData = () => {
  emit("delete");
};
</script>
