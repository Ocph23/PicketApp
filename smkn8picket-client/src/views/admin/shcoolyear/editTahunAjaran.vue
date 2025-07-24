<template>
  <AdminLayout>
    <div class="grid grid-cols-2 gap-4">
      <fwb-card class="p-6">
        <PageHeader title="Edit Tahun Ajaran" />
        <form @submit.prevent="updateSchoolYear">
          <!-- Input Tahun -->

          <div class="mb-5">
            <VTInput v-model="data.form.year" type="number" placeholder="Tahun" label="Tahun" required />
          </div>

          <!-- Input Semester -->
          <div class="mb-5">
            <VTInput v-model="data.form.semester" type="number" placeholder="Semester" label="Semester" required />
          </div>

          <!-- Status Aktif -->
          <div class="mb-5">
            <fwb-select v-model="data.form.actived" :options="[
              { name: 'Aktif', value: '1' },
              { name: 'Tidak Aktif', value: '0' },
            ]" placeholder="Pilih Status" label="Status Aktif" required />
          </div>

          <!-- Tombol Update -->
          <button type="submit"
            class="flex justify-center items-center m-auto w-full text-white bg-green-700 hover:bg-green-800 focus:ring focus:ring-green-300 rounded-lg px-5 py-2.5">
            Simpan Perubahan
          </button>
        </form>
      </fwb-card>
    </div>
  </AdminLayout>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { SchoolYearService, ToastService } from '@/services'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { FwbCard, FwbSelect } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import type { SchoolYear } from '@/models'
import { Helper, type ErrorDetail } from '@/commons'
import VTInput from '@/components/VTInput/VTInput.vue'


const data = reactive({
  form: { id: 0, year: 0, semester: 1, actived: 'true' },
})
const router = useRouter()
const route = useRoute()

const schoolYearId = Number(route.params.id)
try {
  SchoolYearService.getById(schoolYearId).then((response) => {
    if (response.isSuccess) {
      const schoolYear = response.data as SchoolYear
      data.form = {
        id: schoolYear.id,
        year: schoolYear.year,
        semester: schoolYear.semester,
        actived: schoolYear.actived ? 'true' : 'false',
      }
    }
  })
} catch (error) {
  console.error('Error fetching schoolYear data:', error)
}

const updateSchoolYear = async () => {
  try {
    const schoolYear = {
      id: data.form.id,
      semester: data.form.semester,
      year: data.form.year,
      actived: Boolean(data.form.actived)
    } as SchoolYear

    const response = await SchoolYearService.put(schoolYearId, schoolYear)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/Tahun-ajaran' })
    } else {
      const err = response.error?.errors as ErrorDetail[];
      ToastService.dangerToast(Helper.readError(err, 'Message'))
    }
  } catch (error) {
    console.error('Error saving teacher data:', error)
    alert('Terjadi kesalahan saat menyimpan data.')
  }
}
</script>
