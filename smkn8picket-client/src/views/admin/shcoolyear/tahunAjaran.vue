<template>
  <AdminLayout>
    <VTCard title="Data Tahun Ajaran">
      <template #right-side>
        <AddIcon @click="editModal(null)" class="w-7 h-7 cursor-pointer" />
        <router-link :to="{ name: 'addShoolyear' }">
        </router-link>
      </template>
      <VTTable :table-name="'tabel-ta'" :bordered="true" ref="vtTable" :source="data.schoolYears"
        :columns="teacherColumns">
        <template #no="row">
          {{ row.index + 1 }}
        </template>
        <template #status="row">
          <VTIconCheck v-if="row.data.actived" :size="'sm'" :color="'success'" />
        </template>
        <template #action="row">
          <div class="flex items-center gap-2">
            <router-link :to="`/admin/Tahun-ajaran/${row.id}/edit`">
              <VTButtonAction :style="'warning'" :type="'edit'" class="text-white flex">
              </VTButtonAction>
            </router-link>
            <VTButtonAction :type="'delete'" :style="'danger'" @click="confirmDelete(row)" class="text-white flex">
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


  <VTModal v-if="showEditModal">
    <template #header>
      <h3 class="text-lg font-medium leading-6 text-gray-900">Detail Tahun Ajaran</h3>
    </template>
    <template #body>
      <form @submit.prevent="addData">
        <!-- Input Tahun -->

        <div class="mb-5">
          <VTInput v-model="form?.year" type="number" placeholder="Tahun" label="Tahun" required />
        </div>

        <!-- Input Semester -->
        <div class="mb-5">
          <VTInput v-model="data.form.semester" type="number" placeholder="Semester" label="Semester" required />
        </div>

        <!-- Status Aktif -->
        <div class="mb-5">
          <VTSelect v-model="data.form.actived" :options="[
            { name: 'Aktif', value: 'true' },
            { name: 'Tidak Aktif', value: 'false' },
          ]" placeholder="Pilih Status" label="Status Aktif" required />
        </div>

        <!-- Tombol Update -->
        <button type="submit"
          class="flex justify-center items-center m-auto w-full text-white bg-green-700 hover:bg-green-800 focus:ring focus:ring-green-300 rounded-lg px-5 py-2.5">
          Simpan Perubahan
        </button>
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
  VTButtonAction,
  VTCard,
  VTIconCheck,
  VTIconDetail,
  VTInput,
  VTModal,
  VTSelect,
  VTTable,
  type VTTableColumn,
} from '@ocph23/vtocph23'


const showEditModal = ref(false)

const editModal = (schoolYear: SchoolYear | null) => {
  showEditModal.value = true
  form.value = schoolYear
}

const form = ref<SchoolYear | null>(null)
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


const addData = async () => {
  try {
    const schoolYear = {
      year: data.form.year,
      semester: data.form.semester,
      actived: Boolean(data.form.actived),
    } as unknown as SchoolYear

    const response = await SchoolYearService.post(schoolYear)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil ditambahkan')
      router.push({ path: '/admin/Tahun-ajaran' }) // Navigasi ke halaman tahun ajaran
    } else {
      ToastService.dangerToast(Helper.readError(data.errors, 'Message'))
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
