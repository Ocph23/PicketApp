<template>
  <AdminLayout>
    <fwb-card class="p-6 !max-w-full !w-full">
      <PageHeader title="Tambah Guru/Staff"> </PageHeader>
      <form @submit.prevent="addData">
        <div class="w-full grid grid-cols-1 gap-4 md:grid-cols-2">
          <div class="mb-4">
            <VTInput label="Nomor register" type="text" v-model="data.form.registerNumber" required />
          </div>
          <div class="mb-4">
            <VTInput label="Nama" type="text" v-model="data.form.name" required />
          </div>
          <div class="mb-4">
            <VTSelect :options="genders" label="Jenis Kelamin" v-model="data.form.gender"></VTSelect>
          </div>

          <div class="mb-4">
            <VTInput label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
          </div>

          <div class="mb-4">
            <VTInput label="Tanggal Lahir" type="date" v-model="data.form.dateOfBorn" required />
          </div>
          <div class="mb-4">
            <VTInput label="Email" type="email" v-model="data.form.email" required />
          </div>
          <div class="mb-4">
            <VTSelect :options="Helper.jobs" label="Jabatan" type="number" v-model="data.form.job" required />
          </div>

          <div class="mb-4">
            <VTSelect :options="Helper.jobStatus" label="Jenis Pegawai" type="number" v-model="data.form.jobStatus"
              required />
          </div>


        </div>
        <div class="mb-4">
          <fwb-textarea label="Alamat" placeholder="Alamat" v-model="data.form.address" />
        </div>
        <div class="mb-4 flex  justify-end">
          <fwb-button class="!w-auto" color="green" type="submit">Simpan </fwb-button>
        </div>
      </form>
    </fwb-card>
  </AdminLayout>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { TeacherService, ToastService } from '@/services'
import { Helper, type ErrorDetail } from '@/commons'
import { FwbCard, FwbButton, FwbTextarea } from 'flowbite-vue'
import { reactive } from 'vue'
import PageHeader from '@/components/PageHeader.vue'
import type { Teacher } from '@/models'
import { DateTime } from 'luxon'
import VTInput from '@/components/VTInput/VTInput.vue'
import { VTSelect } from '@ocph23/vtocph23'


const router = useRouter()
const data = reactive({
  form: {
    id: 0,
    registerNumber: '',
    name: '',
    gender: '0',
    job: 0,
    jobStatus: 0,
    placeOfBorn: '',
    dateOfBorn: '',
    email: '',
    address: '',
    userId: '',
  },
  errors: [] as ErrorDetail[],
})

const genders = [
  { value: '1', name: 'Perempuan' },
  { value: '0', name: 'Laki-laki' },
]

// Fungsi untuk menambah data Guru (POST)
const addData = async () => {
  try {
    data.errors = []
    const teacher = {
      id: data.form.id,
      name: data.form.name,
      dateOfBorn: DateTime.fromJSDate(new Date(data.form.dateOfBorn)).toFormat('yyyy-MM-dd') as unknown,
      placeOfBorn: data.form.placeOfBorn,
      registerNumber: data.form.registerNumber,
      email: data.form.email,
      job: data.form.job,
      jobStatus: data.form.jobStatus,
      address: data.form.address,
      gender: Number(data.form.gender),
    } as Teacher

    const response = await TeacherService.post(teacher)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/guru' })
    } else {
      console.log('API Error Response:', response.error)
      ToastService.dangerToast(Helper.readError(data.errors, 'Message'))
    }
  } catch (error) {
    console.log('Error data:', error)
  }
}
</script>
