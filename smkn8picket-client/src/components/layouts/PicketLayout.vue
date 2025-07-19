<script setup lang="ts">
import Header from '@/components/layouts/Header.vue'
import PicketSidebar from '@/components/layouts/PicketSideBar.vue'
import { ref } from 'vue'
const showSide = ref(true)

const clickMenu = () => {
  showSide.value = !showSide.value
}
</script>

<template>
  <Header id="header" @on-click-menu="clickMenu"></Header>
  <PicketSidebar id="sidebar" @on-click-menu="clickMenu"
    :class="showSide ? '-translate-x-full  sm:translate-x-0' : ' sm:translate-x-0'"></PicketSidebar>

  <div class="min-h-screen p-8 pt-20 sm:ml-64 bg-white shadow-md dark:bg-gray-600" id="xbody">
    <slot></slot>
  </div>
</template>
