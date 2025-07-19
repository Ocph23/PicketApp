<template>
  <AdminLayout>
    <fwb-card class="!max-w-full w-full p-6">
      <PageHeader title="Edit Guru/Staff"> </PageHeader>
      <div class="w-full flex justify-center mb-6">
        <input type="file" name="file" id="file" hidden />
        <img @click="changePhoto" class="w-32 h-32 rounded-full cursor-pointer bg-slate-200" :src="imageSrc" />
      </div>
      <form @submit.prevent="updateTeacher">
        <div class="grid gap-6 mb-6 md:grid-cols-2">
          <div class="mb-4">
            <fwb-input label="Nomor register" type="text" v-model="data.form.registerNumber" required />
          </div>
          <div class="mb-4">
            <fwb-input label="Nama" type="text" v-model="data.form.name" required />
          </div>
          <div class="mb-4">
            <fwb-select :options="Helper.genders" label="Jenis Kelamin" type="number" v-model="data.form.gender"
              required />
          </div>

          <div class="mb-4">
            <fwb-input label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
          </div>

          <div class="mb-4">
            <VTInput v-model="data.form.dateOfBorn" :type="'date'" label="Tanggal Lahir" required></VTInput>
          </div>
          <div class="mb-4">
            <fwb-input label="Email" type="email" v-model="data.form.email" required />
          </div>
        </div>
        <div class="mb-4">
          <fwb-textarea label="Catatan" placeholder="tulis jika ada catatan" v-model="data.form.description" />
        </div>
        <fwb-button color="green" type="submit">Simpan Perubahan</fwb-button>
      </form>
    </fwb-card>
  </AdminLayout>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ToastService, TeacherService } from '@/services'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import PageHeader from '@/components/PageHeader.vue'
import { Helper, type ErrorResponse } from '@/commons'
import { FwbTextarea, FwbCard, FwbInput, FwbSelect, FwbButton } from 'flowbite-vue'
import type { Teacher } from '@/models'
import VTInput from '@/commons/VTInput/VTInput.vue'

const data = reactive({
  form: {
    id: 0,
    registerNumber: '',
    name: '',
    gender: '0',
    placeOfBorn: '',
    dateOfBorn: new Date(),
    email: '',
    description: '',
    userId: '',
  }
})

const imageSrc = ref('')

const router = useRouter()
const route = useRoute()
const teacherId = Number(route.params.id)

try {
  TeacherService.getById(teacherId).then((response) => {
    if (response.isSuccess) {
      const teacher = response.data as Teacher
      setForm(teacher)
    }
  })
} catch (error) {
  console.error('Error fetching department data:', error)
}

const updateTeacher = async () => {
  try {
    const teacher = collectTeacher(data.form)
    const response = await TeacherService.put(teacher.id, teacher)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/guru' })
    } else {
      console.log('API Error Response:', response.error)
      const err = response.error as ErrorResponse;
      ToastService.dangerToast(Helper.readError(err.errors, 'Message'))
    }
  } catch (error) {
    console.log(error)
    ToastService.dangerToast('Terjadi kesalahan saat menyimpan data.')
  }
}

const changePhoto = async () => {
  const fileInput = document.getElementById('file') as HTMLElement
  fileInput.addEventListener('change', (event: Event) => {
    const target = event.target as HTMLInputElement
    if (target.files && target.files.length) {
      const file = target.files[0]
      Helper.fileToBase64(file).then(async (base64) => {
        imageSrc.value = 'data:image/png;base64,' + base64
        const response = await TeacherService.updateFoto(data.form.id, base64)
        if (response.isSuccess) {
          ToastService.successToast('Photo berhasil diganti !')
        } else {
          ToastService.dangerToast(response.error != null ? response.error.detail : '')
        }
      })
    }
  })
  fileInput.click()
}

function setForm(teacher: Teacher) {
  data.form.id = teacher.id
  data.form.registerNumber = teacher.registerNumber
  data.form.name = teacher.name
  data.form.gender = teacher.gender.toString()
  data.form.dateOfBorn = teacher.dateOfBorn
  data.form.placeOfBorn = teacher.placeOfBorn
  data.form.email = teacher.email
  data.form.description = teacher.description
  imageSrc.value = Helper.getTeacherAvatar(teacher.photo)
}

function collectTeacher(form: {
  id: number
  registerNumber: string
  name: string
  gender: string
  placeOfBorn: string
  dateOfBorn: Date
  email: string
  description: string
  userId: string
}) {
  const teacher = {
    id: form.id,
    name: form.name,
    dateOfBorn: form.dateOfBorn,
    placeOfBorn: form.placeOfBorn,
    registerNumber: form.registerNumber,
    email: form.email,
    description: form.description,
    gender: Number(form.gender),
  } as Teacher

  return teacher
}
</script>

<style scoped>
.input-field {
  width: 100%;
  padding: 10px;
  border-radius: 4px;
  border: 1px solid #ccc;
  font-size: 14px;
}

button {
  width: 100%;
  padding: 12px;
  border-radius: 5px;
  font-size: 16px;
}
</style>
