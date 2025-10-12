<script setup lang="ts">
import Header from '@/components/layouts/Header.vue'
import Sidebar from '@/components/layouts/SideBar.vue'
import { ref } from 'vue'


const showMenu = ref(true);

const clickMenu = () => {
  if (showMenu.value) {
    menuClass.value = "md:ml-0"
  }
  else {
    menuClass.value = "md:ml-64"
  }
  showMenu.value = !showMenu.value
}

const menuClass = ref("md:ml-64")


</script>

<template class="no-print">
  <Header id="header" @on-click-menu="clickMenu"></Header>
  <Sidebar v-if="showMenu" @on-click-menu="clickMenu"></Sidebar>
  <div :class="menuClass" class="min-h-screen p-8 pt-20   bg-[#ebecee]  shadow-md dark:bg-gray-600 " id="xbody">
    <slot></slot>
  </div>
</template>
