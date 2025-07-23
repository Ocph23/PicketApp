<template>
  <AdminLayout>

    <fwb-card class="m-0 p-6 !max-w-full !w-full">

      <PageHeader title="Biodata Siswa" />
      <div class="w-full flex justify-center mb-6">
        <input type="file" name="file" id="file" hidden />
        <img @click="changePhoto" class="w-32 h-32 rounded-full cursor-pointer bg-slate-200" :src="imageSrc" />
      </div>

      <form @submit.prevent="updateStudent">
        <div class="grid grid-cols-2 gap-4">
          <div class="mb-4">
            <VTInput label="NIS" type="text" v-model="data.form.nis" required />
          </div>
          <div class="mb-4">
            <VTInput label="NISN" type="text" v-model="data.form.nisn" required />
          </div>
          <div class="mb-4">
            <VTInput label="Nama" type="text" v-model="data.form.name" required />
          </div>
          <div class="mb-4">
            <fwb-select :options="Helper.genders" label="Jenis Kelamin" type="text" v-model="data.form.gender"
              required />
          </div>
          <div class="mb-4">
            <VTInput label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
          </div>
          <div class="mb-4">
            <VTInput label="Tanggal Lahir" type="date" v-model="data.form.dateOfBorn" required />
          </div>

          <div class="mb-4">
            <VTInput label="email" type="email" v-model="data.form.email" />
          </div>
          <div class="mb-4">
            <VTInput label="Parent Phone Number" type="text" v-model="data.form.parentPhoneNumber" />
          </div>

          <div class="mb-4">
            <fwb-textarea label="Catatan" placeholder="tulis jika ada catatan" v-model="data.form.description" />
          </div>
        </div>
        <fwb-button class="w-full" color="green" type="submit"> Simpan </fwb-button>
      </form>



    </fwb-card>

  </AdminLayout>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ToastService, StudentService } from '@/services'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { Helper, type ErrorResponse } from '@/commons'
import { FwbCard, FwbButton, FwbSelect, FwbTextarea } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import type { Student } from '@/models'
import { DateTime } from 'luxon'
import { VTInput } from '@ocph23/vtocph23'

// const activeTab = ref('biodata')

const data = reactive({
  form: {
    id: 0,
    nis: '',
    nisn: 'null',
    name: '',
    gender: '0',
    placeOfBorn: '',
    dateOfBorn: '',
    email: '',
    description: '',
    userId: '',
    parentPhoneNumber: '',
  },
  error: {} as ErrorResponse,
})
const router = useRouter()
const route = useRoute()
const imageSrc = ref('')
const studentId = Number(route.params.id)
try {
  StudentService.getById(studentId).then((response) => {
    if (response.isSuccess) {
      const student = response.data as Student
      setForm(student)
      imageSrc.value = Helper.getStudentAvatar(student.photo)
    }
  })
} catch (error) {
  console.error('Error fetching department data:', error)
}

// Fungsi untuk memperbarui siswa
const updateStudent = async () => {
  try {
    const siswa = collectStudent(data.form)
    const response = await StudentService.put(studentId, siswa)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/siswa' })
    } else {
      console.log('API Error Response:', response.error)
      data.error = response.error as ErrorResponse
      ToastService.dangerToast(Helper.readError(data.error.errors, 'Message'))
    }
  } catch (error) {
    console.error('Error saving teacher data:', error)
    alert('Terjadi kesalahan saat menyimpan data.')
  }
}

// const changePhoto = async () => {
//   const fileInput = document.getElementById('file') as HTMLElement
//   fileInput.addEventListener('change', (event: Event) => {
//     const target = event.target as HTMLInputElement
//     if (target.files && target.files.length > 0) {
//       const file = target.files[0]
//       Helper.fileToBase64(file).then(async (base64) => {
//         imageSrc.value = 'data:image/png;base64,' + base64
//         const response = await StudentService.updateFoto(data.form.id, base64)
//         if (response.isSuccess) {
//           ToastService.successToast('Photo berhasil diganti !')
//         } else {
//           const x = response.error as ErrorResponse
//           ToastService.dangerToast(x.detail)
//         }
//       })
//     }
//   })
//   fileInput.click()
// }

const changePhoto = async () => {
  const fileInput = document.getElementById('file') as HTMLElement
  fileInput.addEventListener('change', (event: Event) => {
    const target = event.target as HTMLInputElement
    if (target.files && target.files.length > 0) {
      const file = target.files[0]
      const reader = new FileReader()
      reader.onload = async (e) => {
        const base64 = e.target?.result as string
        imageSrc.value = base64

        const response = await StudentService.updateFoto(data.form.id, base64.split(',')[1]);
        if (response.isSuccess) {
          ToastService.successToast('Photo berhasil diganti !')
        } else {
          const x = response.error as ErrorResponse
          if (x && x.detail) {
            ToastService.dangerToast(x.detail)
          }
        }
      }
      reader.readAsDataURL(file)

      // Helper.fileToBase64(file).then(async (base64) => {
      //   imageSrc.value = 'data:image/png;base64,' + base64
      //   const response = await StudentService.updateFoto(data.form.id, base64)
      //   if (response.isSuccess) {
      //     ToastService.successToast('Photo berhasil diganti !')
      //   } else {
      //     const x = response.error as ErrorResponse
      //     ToastService.dangerToast(x.detail)
      //   }
      // })
    }
  })
  fileInput.click()
}

const setForm = (student: Student) => {
  data.form.id = student.id
  data.form.nis = student.nis
  data.form.nisn = student.nisn
  data.form.name = student.name
  data.form.gender = student.gender.toString()
  data.form.dateOfBorn =
    student.dateOfBorn == null ? '' : DateTime.fromISO(student.dateOfBorn).toFormat('yyyy-MM-dd')
  data.form.placeOfBorn = student.placeOfBorn
  data.form.email = student.email
  data.form.description = student.description
  data.form.parentPhoneNumber = student.parentPhoneNumber
  imageSrc.value = Helper.getStudentAvatar(student.photo)
}

function collectStudent(form: {
  id: number
  nis: string
  nisn: string
  name: string
  gender: string
  placeOfBorn: string
  dateOfBorn: string
  email: string
  description: string
  userId: string
  parentPhoneNumber: string
}): Student {
  const student = {} as Student
  student.dateOfBorn = DateTime.fromJSDate(new Date(form.dateOfBorn)).toISODate()
  student.description = form.description
  student.email = form.email
  student.gender = Number(form.gender)
  student.id = form.id
  student.name = form.name
  student.nis = form.nis
  student.nisn = form.nisn
  student.userId = form.userId
  student.parentPhoneNumber = form.parentPhoneNumber
  student.placeOfBorn = form.placeOfBorn
  return student
}
</script>
