<template>
  <FwbTabs v-model="activeTab">
    <FwbTab name="biodata" title="Biodata">
      <fwb-card class="m-0 p-6 !max-w-full !w-full">

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <PageHeader title="Biodata Siswa" />
            <div class="w-full flex flex-col justify-center items-center mb-6">
              <input type="file" name="file" id="file" hidden />
              <img class="w-32 h-32 rounded-full cursor-pointer" :src="imageSrc" />
              <label for="">Status Siswa : {{ Helper.studentStatus(data.form.status) }}</label>
            </div>

            <form>
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="mb-4">
                  <fwb-input label="NIS" type="text" v-model="data.form.nis" required />
                </div>
                <div class="mb-4">
                  <fwb-input label="NISN" type="text" v-model="data.form.nisn" required />
                </div>
                <div class="mb-4">
                  <fwb-input label="Nama" type="text" v-model="data.form.name" required />
                </div>
                <div class="mb-4">
                  <fwb-select :options="Helper.genders" label="Jenis Kelamin" type="text" v-model="data.form.gender"
                    required />
                </div>
                <div class="mb-4">
                  <fwb-input label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
                </div>
                <div class="mb-4">
                  <VTInput label="Tanggal Lahir" type="date" v-model="data.form.dateOfBorn" required />
                </div>

                <div class="mb-4">
                  <fwb-input label="email" type="email" v-model="data.form.email" />
                </div>
                <div class="mb-4">
                  <fwb-input label="Parent Phone Number" type="text" v-model="data.form.parentPhoneNumber" />
                </div>
                <div class="mb-4">
                  <fwb-input label="Kelas" type="text" v-model="data.kelas.classRoomName" />
                </div>
                <div class="mb-4">
                  <fwb-input label="Jurusan" type="text" v-model="data.kelas.departmenName" />
                </div>

              </div>
            </form>


          </div>
          <div ref="myDiv" style="width: 100%">
            <StudentAttendanceChart :student-id="studentId"></StudentAttendanceChart>
          </div>
        </div>




      </fwb-card>
    </FwbTab>
    <FwbTab name="absen" title="Absen">
      <StudentAttendanceList :student-id="studentId" />
    </FwbTab>
    <FwbTab name="riwayat" title="Riwayat Catatan Siswa">
      <fwb-card class="m-0 p-6 !max-w-full !w-full">
        <StudentProgressNoteView :student-id="studentId" :classroom-id="data.kelas.classRoomId">
        </StudentProgressNoteView>
      </fwb-card>

    </FwbTab>
  </FwbTabs>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { StudentService } from '@/services'
import { Helper, type ErrorResponse, } from '@/commons'
import { FwbCard, FwbInput, FwbSelect, FwbTabs, FwbTab } from 'flowbite-vue'
import PageHeader from '@/components/PageHeader.vue'
import type { Student } from '@/models'
import VTInput from '@/commons/VTInput/VTInput.vue'
import { DateTime } from 'luxon'
import StudentAttendanceChart from './studentAttendanceChart.vue'
import StudentAttendanceList from './studentAttendanceList.vue'
import StudentProgressNoteView from './StudentProgressNoteView.vue'


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

</script>
