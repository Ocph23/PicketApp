<template>
  <FwbTabs v-model="activeTab">
    <FwbTab name="biodata" title="Biodata">
      <VTCard class="m-0 p-6 !max-w-full !w-full">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <PageHeader title="Biodata Siswa" />
            <div class="w-full flex flex-col justify-center items-center mb-6">
              <input type="file" name="file" id="file" hidden />
              <div class="w-32 h-32 bg-gray-200 p-2 rounded-full cursor-pointer">
                <img :src="imageSrc" class="w-full h-full" />
              </div>
              <VTStatus class="mt-4" :type="data.form.status ? 'success' : 'danger'"
                :text="Helper.studentStatus(data.form.status)">
              </VTStatus>
            </div>

            <form @submit.prevent>
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="mb-4">
                  <VTInput disabled label="NIS" type="text" v-model="data.form.nis" required />
                </div>
                <div class="mb-4">
                  <VTInput disabled label="NISN" type="text" v-model="data.form.nisn" required />
                </div>
                <div class="mb-4">
                  <VTInput disabled label="Nama" type="text" v-model="data.form.name" required />
                </div>
                <div class="mb-4">
                  <VTSelect :options="Helper.genders" label="Jenis Kelamin" v-model="data.form.gender" required />
                </div>
                <div class="mb-4">
                  <VTInput disabled label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
                </div>
                <div class="mb-4">
                  <VTInput disabled label="Tanggal Lahir" type="date" v-model="data.form.dateOfBorn" required />
                </div>

                <div class="mb-4">
                  <VTInput disabled label="email" type="email" v-model="data.form.email" />
                </div>
                <div class="mb-4">
                  <VTInput disabled label="Parent Phone Number" type="text" v-model="data.form.parentPhoneNumber" />
                </div>
                <div class="mb-4" v-if="data.kelas.classRoomId">
                  <VTInput disabled label="Kelas" type="text" v-model="data.kelas.classRoomName" />
                </div>
                <div class="mb-4" v-if="data.kelas.classRoomId">
                  <VTInput disabled label="Jurusan" type="text" v-model="data.kelas.departmenName" />
                </div>
                <div class="mb-4 " v-if="!data.kelas.classRoomId">
                  <VTInput disabled class="text-red-500 font-bold" label="Kelas" type="text"
                    model-value="Belum terdaftar dalam kelas" />
                </div>
                <div class="mb-4 flex flex-col">
                  <label class="text-sm mb-2">Akun</label>
                  <VTButton :color="'yellow'" v-if="!data.form.userId || data.form.userId == 'NULL'"
                    @click="createAccount">Buat
                    Akun</VTButton>
                  <VTButton :color="'red'" v-if="data.form.userId && data.form.userId != 'NULL'" @click="resetPassword">
                    Reset
                    Password</VTButton>
                </div>
              </div>
            </form>


          </div>
          <div ref="myDiv" style="width: 100%">
            <StudentAttendanceChart :student-id="studentId"></StudentAttendanceChart>
          </div>
        </div>
      </VTCard>
    </FwbTab>
    <FwbTab name="absen" title="Absen">
      <VTCard class="m-0 p-6 !max-w-full !w-full">
        <StudentAttendanceList :student-id="studentId" />
      </VTCard>
    </FwbTab>
    <FwbTab name="riwayat" title="Riwayat Catatan Siswa">
      <VTCard class="m-0 p-6 !max-w-full !w-full">
        <StudentProgressNoteView :student-id="studentId" :classroom-id="data.kelas.classRoomId">
        </StudentProgressNoteView>
      </VTCard>
    </FwbTab>
  </FwbTabs>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { StudentService } from '@/services'
import { Helper, type ErrorResponse, } from '@/commons'
import { FwbTabs, FwbTab } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import type { Student } from '@/models'
import { DateTime } from 'luxon'
import StudentAttendanceChart from './studentAttendanceChart.vue'
import StudentAttendanceList from './studentAttendanceList.vue'
import StudentProgressNoteView from './StudentProgressNoteView.vue'
import VTInput from '@/components/VTInput/VTInput.vue'
import { VTButton, VTCard, VTSelect, VTStatus, VTToastService } from '@ocph23/vtocph23'

const props = defineProps({
  studentId: {
    type: Number,
    required: true,
  },
})


const activeTab = ref('biodata')


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
    status: 0, // Assuming status is a string, adjust as necessary
  },
  kelas: {} as {
    classRoomName: string
    departmenName: string
    classRoomId: number
  },
  error: {} as ErrorResponse,
})
const imageSrc = ref('')


try {
  StudentService.getById(props.studentId).then((response) => {
    if (response.isSuccess) {
      const student = response.data as Student
      setForm(student)
      imageSrc.value = Helper.getStudentAvatar(student.photo)
      StudentService.getStudentWithClassById(props.studentId).then((response) => {
        if (response.isSuccess) {
          data.kelas = response.data as {
            classRoomName: string
            departmenName: string
            classRoomId: number
          }
        }
      })
    }
  })
} catch (error) {
  console.error('Error fetching department data:', error)
}



const setForm = (student: Student) => {
  data.form.id = student.id
  data.form.nis = student.nis
  data.form.nisn = student.nisn
  data.form.userId = student.userId
  data.form.name = student.name
  data.form.status = student.status
  data.form.gender = student.gender.toString()
  data.form.dateOfBorn =
    student.dateOfBorn == null ? '' : DateTime.fromISO(student.dateOfBorn).toFormat('yyyy-MM-dd')
  data.form.placeOfBorn = student.placeOfBorn
  data.form.email = student.email
  data.form.description = student.address
  data.form.parentPhoneNumber = student.parentPhoneNumber
  imageSrc.value = Helper.getStudentAvatar(student.photo)
}


const createAccount = () => {
  StudentService.createAccount(props.studentId).then(response => {
    data.form.userId = response.data as string
  })

}
const resetPassword = () => {
  StudentService.resetPassword(props.studentId).then(response => {
    if (response.isSuccess) {
      VTToastService.success('Password berhasil di reset')
    }

  })
}

</script>
