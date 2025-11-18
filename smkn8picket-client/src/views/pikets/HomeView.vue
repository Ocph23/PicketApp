<script setup lang="ts">
import { Helper } from '@/commons'
import PicketLayout from '@/components/layouts/PicketLayout.vue'
import { ref } from 'vue'

const year = ref(new Date().getFullYear())
</script>

<template>
  <PicketLayout>
    <div class="h-[80vh] flex justify-center flex-col items-center">
      <img class="m-12" width="15%" :src="Helper.infoSekolah.logo" alt="" />
      <p class="text-lg dark:text-white">APLIKASI PIKET</p>
      <p class="text-lg dark:text-white">{{ Helper.infoSekolah.nama_sekolah }}</p>
      <p class="text-lg dark:text-white">{{ Helper.infoSekolah.kota }}</p>
      <p class="text-lg dark:text-white">{{ year }}</p>
    </div>
  </PicketLayout>
</template>
