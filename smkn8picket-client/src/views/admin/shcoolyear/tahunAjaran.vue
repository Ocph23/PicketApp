<template>
  <AdminLayout>
    <VTCard title="Data Tahun Ajaran">
      <template #right-side>
        <AddIcon @click="editModal(null)" class="w-7 h-7 cursor-pointer" />
      </template>
      <VTTable :table-name="'tabel_ta'" :bordered="true" ref="vtTable" :source="data.schoolYears"
        :columns="teacherColumns">
        <template #no="row">
          {{ row.index + 1 }}
        </template>
        <template #status="row">
          <VTIconCheck v-if="row.data.actived" :size="'sm'" :color="'success'" />
        </template>
        <template #action="row">
          <div class="flex items-center gap-2">
            <VTButtonAction @click="editModal(row.data)" :style="'warning'" :type="'edit'" class="text-white flex">
            </VTButtonAction>
            <VTButtonAction :type="'delete'" :style="'danger'" @click="confirmDelete(row.data)" class="text-white flex">
            </VTButtonAction>
            <router-link :to="`/admin/Jadwal/${row.id}`">
              <VTButtonAction :type="'default'" :style="'info'" class="text-white flex">
                <VTIconDetail />
              </VTButtonAction>
            </router-link>
          </div>
        </template>
      </VTTable>
    </VTCard>
  </AdminLayout>

  <VTModal v-if="showEditModal" :size="'md'" @close="() => (showEditModal = false)">
    <template #header>
      <h3 class="text-lg font-medium leading-6 text-gray-900">Tambah/Edit Tahun Ajaran</h3>
    </template>
    <template #body>
      <form @submit.prevent="saveData">
        <!-- Input Tahun -->

        <div class="mb-5">
          <VTInput v-model="form.year" type="number" placeholder="Tahun" label="Tahun" required />
        </div>

        <!-- Input Semester -->
        <div class="mb-5">
          <VTInput v-model="form.semester" type="number" placeholder="Semester" label="Semester" required />
        </div>

        <!-- Status Aktif -->
        <div class="mb-5">
          <VTSelect v-model="form.actived" :options="[
            { name: 'Aktif', value: 'true' },
            { name: 'Tidak Aktif', value: 'false' },
          ]" placeholder="Pilih Status" label="Status Aktif" required />
        </div>

        <!-- Tombol Update -->
        <div class="flex justify-end gap-2">
          <VTButton color="red" @click="
            () => {
              showEditModal = false
            }
          ">Batal</VTButton>
          <VTButtonSave type="submit">Simpan</VTButtonSave>
        </div>
      </form>
    </template>
  </VTModal>
</template>
<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { DialogService, SchoolYearService, ToastService } from '@/services'
import { AddIcon } from '@/components/icons'
import type { SchoolYear } from '@/models'
import {
  VTButton,
  VTButtonAction,
  VTButtonSave,
  VTCard,
  VTIconCheck,
  VTIconDetail,
  VTInput,
  VTModal,
  VTSelect,
  VTTable,
  type VTTableColumn,
} from '@ocph23/vtocph23'
import { Helper, type ErrorResponse } from '@/commons'

const showEditModal = ref(false)

const editModal = (schoolYear: SchoolYear | null) => {
  showEditModal.value = true
  form.value = schoolYear ?? ({} as SchoolYear)
}

const form = ref<SchoolYear>({} as SchoolYear)
const data = reactive({
  schoolYears: [] as SchoolYear[],
})

const teacherColumns: VTTableColumn[] = [
  { title: 'No', name: 'no', type: 'Custome', headerClass: '!w-12 !text-center bg-red-500 !p-1' },
  {
    title: 'Nama',
    propName: 'name',
    isMobileHeader: true,
    headerClass: '!p-1',
  },
  { title: 'Semester', propName: 'semester', headerClass: '!p-1' },
  {
    title: 'Aktif',
    name: 'status',
    type: 'Custome',
    headerClass: '!w-12 !text-center bg-red-500 !p1',
  },
  {
    title: 'Action',
    name: 'action',
    type: 'Custome',
    headerClass: '!w-12 !text-center bg-red-500 !p-1',
  },
]

const vtTable = ref<InstanceType<typeof VTTable> | null>(null)
// Fungsi untuk mengambil data (GET)
const getData = async () => {
  try {
    const response = await SchoolYearService.get()
    if (response.isSuccess) {
      data.schoolYears = (response.data as SchoolYear[]).reverse()
      vtTable.value?.refresh()
    }
  } catch (error: unknown) {
    console.error('Error fetching data:', error)
  }
}

const saveData = async () => {
  try {
    const schoolYear = {
      id: form.value.id,
      semester: form.value.semester,
      year: form.value.year,
      actived: Boolean(form.value.actived),
    } as SchoolYear
    if (form.value.id) {

      const response = await SchoolYearService.put(form.value.id, schoolYear)
      if (response.isSuccess) {
        ToastService.successToast('Data berhasil diubah')
        vtTable.value?.refresh()
        form.value = {} as SchoolYear
        showEditModal.value = false
      } else {
        const err = response.error as ErrorResponse
        ToastService.dangerToast(Helper.readDetailError(err))
      }
    } else {
      const response = await SchoolYearService.post(schoolYear)
      if (response.isSuccess) {
        ToastService.successToast('Data berhasil ditambahkan')
        data.schoolYears.push(response.data as SchoolYear)
        vtTable.value?.refresh()
        form.value = {} as SchoolYear
        showEditModal.value = false
      } else {
        const err = response.error as ErrorResponse
        ToastService.dangerToast(Helper.readDetailError(err))
      }
    }
  } catch {
    ToastService.dangerToast('Terjadi kesalahan saat menambahkan data')
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
      vtTable.value?.refresh()
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
