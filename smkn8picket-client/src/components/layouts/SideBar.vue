<template>
  <VTSidebar>
    <VTSidebarItem>
      <template #icon>
        <svg class=" w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900
    dark:group-hover:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
          viewBox="0 0 22 21">
          <path d="M16.975 11H10V4.025a1 1 0 0 0-1.066-.998 8.5 8.5 0 1 0 9.039 9.039.999.999 0 0 0-1-1.066h.002Z" />
          <path
            d="M12.5 0c-.157 0-.311.01-.565.027A1 1 0 0 0 11 1.02V10h8.975a1 1 0 0 0 1-.935c.013-.188.028-.374.028-.565A8.51 8.51 0 0 0 12.5 0Z" />
        </svg>
      </template>
      <template #default>Dashboard</template>
    </VTSidebarItem>
    <VTSidebarItem>
      <template #icon>
        <HomeIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Home
      </template>
    </VTSidebarItem>
    <VTSidebarItem link="/admin/tahun-ajaran">
      <template #icon>
        <CalendarIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Tahun Ajaran</template>
    </VTSidebarItem>
    <VTSidebarItem link="/admin/jurusan">
      <template #icon>
        <TagIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Jurusan</template>
    </VTSidebarItem>
    <VTSidebarItem link="/admin/guru">
      <template #icon>
        <UsersIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Guru</template>
    </VTSidebarItem>
    <VTSidebarItem link="/admin/siswa">
      <template #icon>
        <AcademicCapIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Siswa</template>
    </VTSidebarItem>
    <VTSidebarItem link="/admin/classroom">
      <template #icon>
        <CubeIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Kelas</template>
    </VTSidebarItem>
    <fwb-sidebar-dropdown-item-custome>
      <template #icon>
        <ClipboardDocumentCheckIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #trigger> Picket </template>
      <template #default>
        <VTSidebarItem link="/admin/hariini" class="pl-11">
          <template #icon>
            <CalendarIcon
              class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
          </template>
          Hari Ini
        </VTSidebarItem>
        <VTSidebarItem link="/admin/history" class="pl-11">
          <template #icon>
            <ClockIcon
              class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
          </template>
          Riwayat Piket
        </VTSidebarItem>
        <VTSidebarItem link="/admin/jadwalpiket" class="pl-11">
          <template #icon>
            <ClockIcon
              class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
          </template>
          Jadwal Guru Piket
        </VTSidebarItem>
      </template>
    </fwb-sidebar-dropdown-item-custome>
    <fwb-sidebar-dropdown-item-custome>
      <template #icon>
        <ClipboardDocumentCheckIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #trigger> Laporan </template>
      <template #default>
        <VTSidebarItem link="/admin/laporan/absenguru" class="pl-11">
          <template #icon>
            <CalendarIcon
              class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
          </template>
          Absen Guru
        </VTSidebarItem>

      </template>
    </fwb-sidebar-dropdown-item-custome>


    <VTSidebarItem @click="logout">
      <template #icon>
        <LockClosedIcon
          class="w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" />
      </template>
      <template #default>Keluar</template>
    </VTSidebarItem>




  </VTSidebar>
</template>
<script setup lang="ts">
import { CalendarIcon, HomeIcon, AcademicCapIcon, UsersIcon, TagIcon, CubeIcon, ClockIcon, ClipboardDocumentCheckIcon, LockClosedIcon } from '@heroicons/vue/24/solid'
import FwbSidebarDropdownItemCustome from '@/components/FwbSidebarDropdownItemCustome.vue'
import { VTDialogService, VTSidebar, VTSidebarItem } from '@ocph23/vtocph23';



const logout = () => {
  VTDialogService.asyncShowDialog('Perhatian', 'Yakin keluar ?', null, 'danger').then(() => {
    localStorage.removeItem('authToken')
    window.location.href = '/login'
  })

}



</script>

<style scoped></style>
