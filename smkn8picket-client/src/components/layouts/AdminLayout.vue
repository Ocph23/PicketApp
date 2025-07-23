<script setup lang="ts">
import Header from '@/components/layouts/Header.vue'
import Sidebar from '@/components/layouts/SideBar.vue'
import { ref } from 'vue'

const clickMenu = () => {
  if (menuClass.value == "!-translate-x-100")
    menuClass.value = "!translate-x-100"
  else
    menuClass.value = "!-translate-x-100"
}

const menuClass = ref("!-translate-x-100 md:!translate-x-0")

</script>

<template class="no-print">
  <Header id="header" @on-click-menu="clickMenu"></Header>
  <Sidebar @on-click-menu="clickMenu" :class="menuClass">
  </Sidebar>
  <div class="min-h-screen p-8 pt-20  md:ml-64 bg-[#ebecee]  shadow-md dark:bg-gray-600 " id="xbody">
    <slot></slot>
  </div>
</template>
