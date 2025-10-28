<script setup lang="ts">
import Header from '@/components/layouts/Header.vue'
import SideBarWaliKelas from '@/components/layouts/SideBarWaliKelas.vue'
import { DialogService } from '@/services'
import { ref } from 'vue'
const showSide = ref(true)

const clickMenu = () => {
  showSide.value = !showSide.value
}



const logout = () => {
  DialogService.showDialog('Yakin keluar ?', null, 'warning').then(() => {
    localStorage.removeItem('authToken')
    window.location.href = '/login'
  })
}
</script>

<template class="no-print">
  <Header id="header" @on-click-menu="clickMenu" @on-click-logout="logout"></Header>
  <SideBarWaliKelas id="sidebar" @on-click-menu="clickMenu"
    :class="showSide ? '-translate-x-full  sm:translate-x-0' : ' sm:translate-x-0'"></SideBarWaliKelas>
  <div class="min-h-screen  p-8 pt-20 sm:ml-64 bg-[#ebecee]  shadow-md dark:bg-gray-600 " id="xbody">
    <slot></slot>
  </div>
</template>
