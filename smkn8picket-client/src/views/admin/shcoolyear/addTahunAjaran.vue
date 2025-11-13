<template>
  <AdminLayout>
    <div class="grid grid-cols-2 gap-4">
      <fwb-card class="p-6">
        <PageHeader title="Tambah Tahun Ajaran" />
        <form @submit.prevent="addData">
          <!-- Input Tahun -->

          <div class="mb-5">
            <VTInput  v-model="data.form.year" type="number" placeholder="Tahun" label="Tahun" required />
          </div>

          <!-- Input Semester -->
          <div class="mb-5">
            <VTInput v-model="data.form.semester" type="number" placeholder="Semester" label="Semester" required />
          </div>

          <!-- Status Aktif -->
          <div class="mb-5">
            <fwb-select v-model="data.form.actived" :options="[
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
      </fwb-card>
    </div>
  </AdminLayout>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { SchoolYearService, ToastService } from '@/services'
import { Helper } from '@/commons'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { FwbCard, FwbInput, FwbSelect } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import { useRouter } from 'vue-router'
import type { SchoolYear } from '@/models'
import VTInput from '@/components/VTInput/VTInput.vue'

const router = useRouter()

const data = reactive({
  form: {
    year: 'null',
    semester: 'null',
    actived: 'true', // Nilai default
  },
  errors: [],
})

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
</script>


<style scoped>
  
</style>
