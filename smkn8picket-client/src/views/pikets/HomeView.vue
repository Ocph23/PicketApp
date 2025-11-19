<script setup lang="ts">
import { Helper } from '@/commons'
import PicketLayout from '@/components/layouts/PicketLayout.vue'
</script>

<template>
  <PicketLayout>
    <div class="h-[80vh] flex justify-center flex-col items-center">
      <div class="col-span-2 hidden md:block">
        <div class="flex flex-col justify-center items-center">
          <img :src="Helper.infoSekolah.logo" class="w-1/5 m-5" alt="SMK Logo" />
          <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
            APLIKASI PIKET SEKOLAH
          </FwbHeading>
          <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
            SEKOLAH MENENGAH KEJURUAN NEGERI 8
          </FwbHeading>
          <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
            TEKNOLOGI INFORMASI DAN KOMUNIKASI
          </FwbHeading>
          <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2"> KOTA JAYAPURA </FwbHeading>
          <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
            @{{ new Date().getFullYear() }}
          </FwbHeading>
        </div>
      </div>
    </div>
  </PicketLayout>
</template>
