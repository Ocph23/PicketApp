<script setup lang="ts">
import { onMounted, reactive } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { DialogService, ToastService, DepartmentService } from '@/services'
import { DeleteIcon, AddIcon, EditIcon } from '@/components/icons'
import PageHeader from '@/components/PageHeader.vue'

import {
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import type { Department } from '@/models'

const data = reactive({
  departments: [] as Department[],
  showDeleteModal: false,
  form: { name: '', initial: '', description: '' },
})

// Fungsi untuk mengambil data (GET)
const getData = async () => {
  try {
    const response = await DepartmentService.get()
    if (response.isSuccess) {
      data.departments = response.data as Department[]
    }
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}

const confirmDelete = (department: Department) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus jurusan ${department.name} ?`,
    department,
    'danger',
    3000,
    'Batal',
    'Hapus',
  ).then(() => {
    deleteData(department)
  })
}

// Fungsi untuk menghapus data (DELETE)
const deleteData = async (department: Department) => {
  if (department) {
    try {
      const response = await DepartmentService.delete(department.id)
      if (response.isSuccess) {
        ToastService.successToast('Data berhasil dihapus.')
        const index = data.departments.indexOf(department)
        data.departments.splice(index, 1)
      }
    } catch (error) {
      console.error('Error deleting data:', error)
    } finally {
      data.showDeleteModal = false
    }
  }
}

onMounted(getData)
</script>
<template>
  <AdminLayout>
    <div class="">
      <PageHeader title="Data Tahun Ajaran">
        <router-link :to="{ name: 'addJurusan' }">
          <AddIcon class="w-7 h-7" />
        </router-link>
      </PageHeader>

      <div class="relative overflow-x-auto shadow-md sm:rounded-lg mt-1">
        <fwb-table>
          <fwb-table-head>
            <fwb-table-head-cell>No</fwb-table-head-cell>
            <fwb-table-head-cell>Nama</fwb-table-head-cell>
            <fwb-table-head-cell>Kode Jurusan</fwb-table-head-cell>
            <fwb-table-head-cell>Deskripsi Jurusan</fwb-table-head-cell>
            <fwb-table-head-cell class="w-5">
              <span class="sr-only">Action</span>
            </fwb-table-head-cell>
          </fwb-table-head>
          <fwb-table-body>
            <fwb-table-row v-for="(department, index) in data.departments" :key="index">
              <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
              <fwb-table-cell>{{ department.name }}</fwb-table-cell>
              <fwb-table-cell>{{ department.initial }}</fwb-table-cell>
              <fwb-table-cell>{{ department.description }}</fwb-table-cell>
              <fwb-table-cell>
                <div class="flex items-center">
                  <router-link :to="`/admin/jurusan/${department.id}/edit`">
                    <button class="text-white flex">
                      <EditIcon />
                    </button>
                  </router-link>
                  <button @click="confirmDelete(department)" class="text-white flex">
                    <DeleteIcon />
                  </button>
                </div>
              </fwb-table-cell>
            </fwb-table-row>
          </fwb-table-body>
        </fwb-table>
      </div>
    </div>
  </AdminLayout>
</template>
