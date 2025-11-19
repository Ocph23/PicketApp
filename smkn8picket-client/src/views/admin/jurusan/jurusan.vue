<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { DepartmentService } from '@/services'
import { DeleteIcon, AddIcon, EditIcon } from '@/components/icons'

import {
  VTButton,
  VTButtonSave,
  VTCard,
  VTDialogService,
  VTInput,
  VTModal,
  VTTable,
  VTTextArea,
  VTToastService,
  type VTTableColumn,
} from '@ocph23/vtocph23'
import type { Department } from '@/models'
import { Helper, type ErrorResponse } from '@/commons'

const showModal = ref(false)
const departmentTable = ref<InstanceType<typeof VTTable> | null>(null)
const departmentColumns = [
  { title: 'No', name: 'no', type: 'Custome' },
  { title: 'Nama', propName: 'name', isMobileHeader: true },
  { title: 'Kode Jurusan', propName: 'initial' },
  { title: 'Deskripsi Jurusan', propName: 'description' },
  { title: 'Aksi', name: 'actions', type: 'Custome' },
] as VTTableColumn[]

const data = reactive({
  departments: [] as Department[],
  showDeleteModal: false,
  form: {} as Department,
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
  VTDialogService.asyncShowDialog(
    'Perhatian',
    `Apakah Anda yakin ingin menghapus jurusan ${department.name} ?`,
    department,
    'danger',
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
        VTToastService.success('Data berhasil dihapus.')
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
const editData = (department: Department) => {
  showModal.value = true
  data.form = { ...department }
}
const saveData = async () => {
  try {
    if (data.form.id) {
      const response = await DepartmentService.put(data.form.id, data.form)
      if (response.isSuccess) {
        VTToastService.success('Data berhasil diubah')
        const department = data.departments.find((x) => x.id == data.form.id)
        if (department) {
          department.name = data.form.name
          department.initial = data.form.initial
          department.description = data.form.description
        }

        data.form = {} as Department
        departmentTable.value?.refresh()
        showModal.value = false
      } else {
        const err = response.error as ErrorResponse
        VTToastService.error(Helper.readDetailError(err))
      }
    } else {
      const response = await DepartmentService.post(data.form)
      if (response.isSuccess) {
        VTToastService.success('Data berhasil ditambahkan')
        data.departments.push(response.data as Department)
        departmentTable.value?.refresh()
        data.form = {} as Department
        showModal.value = false
      } else {
        const err = response.error as ErrorResponse
        VTToastService.error(Helper.readDetailError(err))
      }
    }
  } catch {
    VTToastService.error('Terjadi kesalahan saat menambahkan data')
  }
}

onMounted(getData)
</script>
<template>
  <AdminLayout>
    <VTCard title="Data Jurusan">
      <template #rightSide>
        <AddIcon class="w-7 h-7 cursor-pointer" @click="showModal = true" />
      </template>
      <VTTable ref="departmentTable" :columns="departmentColumns" :source="data.departments"
        table-name="departmentTable">
        <template #actions="row">
          <div class="flex items-center space-x-2">
            <button class="text-white flex" @click="editData(row.data)">
              <EditIcon />
            </button>
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

  <VTModal v-if="showModal" :size="'md'" @close="() => (showModal = false)">
    <template #header>
      <h3 class="text-lg font-medium leading-6 text-gray-900">Tambah/Edit Jurusan</h3>
    </template>
    <template #body>
      <form @submit.prevent="saveData">
        <!-- Input Tahun -->

        <div class="mb-5">
          <VTInput v-model="data.form.name" placeholder="Nama Jurusan" label="Nama Jurusan" required />
        </div>

        <div class="mb-5">
          <VTInput v-model="data.form.initial" placeholder="Inisial" label="Inisial" required />
        </div>
        <div class="mb-5">
          <VTTextArea v-model="data.form.description" placeholder="Deskripsi Jurusan" label="Deskripsi Jurusan"
            required />
        </div>

        <!-- Tombol Update -->
        <div class="flex justify-end gap-2">
          <VTButton color="red" @click="
            () => {
              showModal = false
            }
          ">Batal</VTButton>
          <VTButtonSave type="submit">Simpan</VTButtonSave>
        </div>
      </form>
    </template>
  </VTModal>
</template>
