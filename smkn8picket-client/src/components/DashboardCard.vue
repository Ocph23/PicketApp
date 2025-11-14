<template>
  <FwbCard class="col-6 p-5 !min-w-full ">
    <div class="flex flex-col items-start justify-start">
      <div class="w-full flex  items-start justify-between ">
        <h1 class="text-center dark:text-white text-2xl xl:text-4xl font-bold">{{ props.total || 0 }}</h1>
        <img :src="props.picture" class="text-primary w-10 h-10" />
      </div>
      <div class="flex justify-between w-full">
        <h1 class="text-center text-xl xl:text-2xl text-teal-500">{{ props.title }}</h1>
        <div class="flex gap-4" v-if="props.hasGender">
          <div class="flex items-center ">
            <img src="/male.png" class="text-primary w-5 h-5" />
            <h1 class="text-center dark:text-white text-2xl ">{{ props.man || 0 }}</h1>
          </div>
          <div class="flex items-center ">
            <img src="/female.png" class="text-primary w-5 h-5" />
            <h1 class="text-center dark:text-white text-2xl ">{{ props.women || 0 }}</h1>
          </div>
        </div>
      </div>
    </div>
  </FwbCard>


</template>

<script setup lang="ts">
import { FwbCard } from 'flowbite-vue';


const props = defineProps<{
  total: number;
  picture: string;
  title: string;
  man?: number;
  women?: number;
  hasGender?: boolean;
}>();

</script>

<style scoped></style>
