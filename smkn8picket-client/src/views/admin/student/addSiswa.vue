<template>
  <AdminLayout>
    <fwb-card class="p-6 !max-w-full !w-full">
      <PageHeader title="Tambah Siswa"> </PageHeader>
      <form @submit.prevent="addData" class="space-y-4">
        <div class="grid grid-cols-2 gap-4">
          <div class="mb-4">
            <fwb-input label="NIS" type="text" v-model="data.formData.nis" required />
          </div>
          <div class="mb-4">
            <fwb-input label="NISN" type="text" v-model="data.formData.nisn" required />
          </div>
          <div class="mb-4">
            <fwb-input label="Nama" type="text" v-model="data.formData.name" required />
          </div>

          <div class="mb-4">
            <fwb-select :options="Helper.genders" label="Jenis Kelamin" type="text" v-model="data.formData.gender"
              required />
          </div>
          <div class="mb-4">
            <fwb-input label="Tempat Lahir" type="text" v-model="data.formData.placeOfBorn" required />
          </div>
          <div class="mb-4">
            <fwb-input label="Tanggal Lahir" type="date" v-model="data.formData.dateOfBorn" required />
          </div>

          <div class="mb-4">
            <fwb-input label="email" type="email" v-model="data.formData.email" />
          </div>
          <div class="mb-4">
            <fwb-input label="Telepon Orang Tua" type="text" v-model="data.formData.parentPhoneNumber" />
          </div>

          <div class="mb-4">
            <fwb-textarea label="Catatan" placeholder="tulis jika ada catatan" v-model="data.formData.description" />
          </div>
        </div>
        <fwb-button class="w-full cursor-pointer" color="green" type="submit"> Tambah Siswa </fwb-button>
      </form>
    </fwb-card>
  </AdminLayout>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { reactive } from 'vue'
import { StudentService, ToastService } from '@/services'
import { Helper } from '@/commons'
import { FwbCard, FwbButton, FwbInput, FwbSelect, FwbTextarea } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import type { Student } from '@/models'
import { DateTime } from 'luxon'

const router = useRouter()

const data = reactive({
  formData: {
    id: 0,
    nis: '',
    nisn: '',
    name: '',
    gender: '0',
    placeOfBorn: '',
    dateOfBorn: '',
    email: '',
    description: '',
    photo: '',
    status: 0,
    parentPhoneNumber: '',
    userId: '',
  },
  errors: [],
})

const addData = async () => {
  try {
    data.errors = []
    const student = collectStudent(data.formData)
    const response = await StudentService.post(student)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/siswa' })
    } else {
      ToastService.dangerToast(Helper.readError(data.errors, 'Message'))
    }
  } catch (error) {
    console.error('Error adding data:', error)
    console.log('Error data:', error)
  }
}


const collectStudent = (form: {
  id: number
  nis: string
  nisn: string
  name: string
  gender: string
  placeOfBorn: string
  dateOfBorn: string
  email: string
  description: string
  photo: string
  status: number
  parentPhoneNumber: string
  userId: string
}): Student => {
  const student = {
    id: form.id,
    nis: form.nis,
    nisn: form.nisn,
    name: form.name,
    dateOfBorn: DateTime.fromJSDate(new Date(form.dateOfBorn)).toFormat('yyyy-MM-dd'),
    placeOfBorn: form.placeOfBorn,
    email: form.email,
    description: form.description,
    gender: Number(form.gender),
    parentPhoneNumber: form.parentPhoneNumber,
    photo: form.photo,
    status: form.status,
    userId: form.userId,
  } as unknown as Student
  return student
}
</script>
