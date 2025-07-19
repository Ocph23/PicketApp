<template>
  <AdminLayout>
    <div>
      <PageHeader title="Data Tahun Ajaran">
        <router-link :to="{ name: 'addShoolyear' }">
          <AddIcon class="w-7 h-7" />
        </router-link>
      </PageHeader>
      <fwb-table>
        <fwb-table-head>
          <fwb-table-head-cell>No</fwb-table-head-cell>
          <fwb-table-head-cell>Nama</fwb-table-head-cell>
          <fwb-table-head-cell>Semester</fwb-table-head-cell>
          <fwb-table-head-cell>Tahun</fwb-table-head-cell>
          <fwb-table-head-cell>Active</fwb-table-head-cell>
          <fwb-table-head-cell class="w-5">
            <span class="sr-only">Action</span>
          </fwb-table-head-cell>
        </fwb-table-head>
        <fwb-table-body>
          <fwb-table-row v-for="(schoolYear, index) in data.schoolYears" :key="index">
            <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
            <fwb-table-cell>{{ schoolYear.name }}</fwb-table-cell>
            <fwb-table-cell>{{ schoolYear.semester }}</fwb-table-cell>
            <fwb-table-cell>{{ schoolYear.actived }}</fwb-table-cell>
            <fwb-table-cell>
              <fwb-checkbox v-model="schoolYear.actived" :disabled="true" />
            </fwb-table-cell>
            <fwb-table-cell>
              <div class="flex items-center">
                <router-link :to="`/admin/Tahun-ajaran/${schoolYear.id}/edit`">
                  <button class="text-white flex">
                    <EditIcon />
                  </button>
                </router-link>
                <button @click="confirmDelete(schoolYear)" class="text-white flex">
                  <DeleteIcon />
                </button>
                <router-link :to="`/admin/Jadwal/${schoolYear.id}`">
                  <button class="text-white flex">
                    <DetailIcon />
                  </button>
                </router-link>
              </div>
            </fwb-table-cell>
          </fwb-table-row>
        </fwb-table-body>
      </fwb-table>
    </div>
  </AdminLayout>
</template>
<script setup lang="ts">
import { onMounted, reactive } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { DialogService, SchoolYearService, ToastService } from '@/services'
import { DeleteIcon, DetailIcon, EditIcon, AddIcon } from '@/components/icons'
import PageHeader from '@/components/PageHeader.vue'
import type { SchoolYear } from '@/models'

import {
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
  FwbCheckbox,
} from 'flowbite-vue'

const data = reactive({
  schoolYears: [] as SchoolYear[],
  form: { name: null, year: null, semester: null, active: true } as unknown,
})

// Fungsi untuk mengambil data (GET)
const getData = async () => {
  try {
    const response = await SchoolYearService.get()
    if (response.isSuccess) {
      data.schoolYears = (response.data as SchoolYear[]).reverse()
    }
  } catch (error: unknown) {
    console.error('Error fetching data:', error)
  }
}

// Fungsi untuk menghapus data (DELETE)
const deleteData = async (schoolYear: SchoolYear) => {
  try {
    const response = await SchoolYearService.delete(schoolYear.id)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil dihapus.')
      const index = data.schoolYears.indexOf(schoolYear)
      data.schoolYears.splice(index, 1)
    }
  } catch (error) {
    console.error('Error deleting data:', error)
  }
}

const confirmDelete = (schoolYear: SchoolYear) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus Tahun Ajaran ${schoolYear.name} ?`,
    schoolYear,
    'warning',
    3000,
    'Batal',
    'Hapus',
  ).then(() => {
    deleteData(schoolYear)
  })
}

onMounted(getData)
</script>
