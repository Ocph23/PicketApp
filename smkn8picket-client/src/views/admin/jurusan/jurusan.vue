<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { DialogService, ToastService, DepartmentService } from '@/services'
import { DeleteIcon, AddIcon, EditIcon } from '@/components/icons'
import PageHeader from '@/components/PageHeader.vue'


import { VTCard, VTTable, type VTTableColumn } from '@ocph23/vtocph23'
import type { Department } from '@/models'

const departmentTable = ref<InstanceType<typeof VTTable> | null>(null)

const departmentColumns = [
  { title: 'No', name: 'no', sortable: false, type: 'Custome' },
  { title: 'Nama', propName: 'name', isMobileHeader: true, sortable: true },
  { title: 'Kode Jurusan', propName: 'initial', sortable: true },
  { title: 'Deskripsi Jurusan', propName: 'description', sortable: false },
  { title: 'Aksi', name: 'actions', sortable: false, type: 'Custome' },
] as VTTableColumn[];

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
      departmentTable.value?.refresh()
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
    <VTCard title="Data Jurusan">
      <template #right-side>
        <router-link :to="{ name: 'addJurusan' }">
          <AddIcon class="w-7 h-7" />
        </router-link>
      </template>
      <VTTable ref="departmentTable" :columns="departmentColumns" :source="data.departments"
        table-name="departmentTable">
        <template #actions="row">
          <div class="flex items-center space-x-2">
            <router-link :to="`/admin/jurusan/${row.data.id}/edit`">
              <button class="text-white flex">
                <EditIcon />
              </button>
            </router-link>
            <button @click="confirmDelete(row.data)" class="text-white flex">
              <DeleteIcon />
            </button>
          </div>
        </template>
        <template #no="row">
          {{ row.index + 1 }}
        </template>
      </VTTable>
    </VTCard>
  </AdminLayout>
</template>
