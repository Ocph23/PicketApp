<script setup lang="ts">
import Header from '@/components/layouts/Header.vue'
import Sidebar from '@/components/layouts/SideBar.vue'
import { ref } from 'vue'
const showSide = ref(true)

const clickMenu = () => {
  showSide.value = !showSide.value
}
</script>

<template class="no-print">
  <Header id="header" @on-click-menu="clickMenu"></Header>
  <Sidebar id="sidebar" @on-click-menu="clickMenu"
    :class="showSide ? '-translate-x-full  sm:translate-x-0' : ' sm:translate-x-0'"></Sidebar>
  <div class="min-h-screen  p-8 pt-20 sm:ml-64 bg-[#ebecee]  shadow-md dark:bg-gray-600 " id="xbody">
    <slot></slot>
  </div>
</template>
