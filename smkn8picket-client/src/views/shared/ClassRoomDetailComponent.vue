<template>
  <div class="no-print">
    <PageHeader title="Detail Kelas"> </PageHeader>
    <fwb-card class="!max-w-1/2  bg-white p-4 rounded-lg shadow-md">
      <div class="flex gap-1 flex-col">
        <div class="flex">
          <label class="w-32 dark:text-gray-100">Nama Kelas</label>
          <label for="className" class="dark:text-gray-100 capitalize">: {{ classroom.className }}-{{
            classroom.departmentInitial }}</label>
        </div>
        <div class="flex">
          <label class="w-32 dark:text-gray-100">Nama Jurusan</label>
          <label for="className" class="dark:text-gray-100 capitalize">: {{ classroom.departmentName }}</label>
        </div>
        <div class="flex">
          <label class="w-32 dark:text-gray-100">Wali Kelas</label>
          <label for="className" class="dark:text-gray-100 capitalize">: {{ classroom.homeRoomTeacherName }}</label>
        </div>
        <div class="flex">
          <label class="w-32 dark:text-gray-100">Ketua Kelas</label>
          <label for="className" class="dark:text-gray-100 capitalize">: {{ classroom.classLeaderName }}</label>
        </div>
      </div>
    </fwb-card>

    <!-- Modal -->


    <!-- Classroom Table -->

    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-semibold"></h1>
      <div class="flex flex-column justify-center items-center "> <button @click="showModal = true"
          class="transition rounded text-black m-3 mr-5 w-6 items-center text-center">
          <AddIcon class="w-7 h-7" />
        </button>
        <fwb-button :color="'yellow'" type="submit" class="flex flex-row items-center justify-center p-2"
          @click="print">
          <PrinterIcon class="w-3 h-3"></PrinterIcon>
        </fwb-button>
      </div>
    </div>
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg mt-1">
      <fwb-table>
        <fwb-table-head>
          <fwb-table-head-cell class="px-6 py-3">No</fwb-table-head-cell>
          <fwb-table-head-cell class="px-6 py-3">Nama Siswa</fwb-table-head-cell>
          <fwb-table-head-cell class="px-6 py-3">NIS/NISN</fwb-table-head-cell>
          <fwb-table-head-cell class="px-6 py-3">Jenis Kelamin</fwb-table-head-cell>
          <fwb-table-head-cell class="px-6 py-3">TTL</fwb-table-head-cell>
          <fwb-table-head-cell class="w-12 px-6 py-3">Action</fwb-table-head-cell>
        </fwb-table-head>
        <fwb-table-body v-if="classroom.id">
          <fwb-table-row v-for="(student, index) in classroom.students.sort()" :key="index">
            <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>

            <fwb-table-cell class="flex items-center gap-2">
              <img class="w-8 h-8 rounded-full" :src="Helper.getStudentAvatar(student.photo)" />
              <span class="capitalize">
                {{ student.name }}</span> </fwb-table-cell>
            <fwb-table-cell>
              {{ student.nis }} <span v-if="student.nisn">/{{ student.nisn }}</span>
            </fwb-table-cell>
            <fwb-table-cell>{{ student.gender == 0 ? 'Laki-laki' : 'Perempuan' }}</fwb-table-cell>
            <fwb-table-cell class="capitalize">{{ student.placeOfBorn }},
              {{ student.dateOfBorn == null ? "" : DateTime.fromJSDate(new Date(student.dateOfBorn))
                .toFormat('dd LLL yyyy')
              }}</fwb-table-cell>
            <fwb-table-cell class=" flex gap-2 items-center justify-start">
              <router-link :to="isHomeRoomTeacher ? `/walikelas/siswa/${student.id}` : `/siswa/${student.id}/detail`">
                <DetailIcon></DetailIcon>
              </router-link>
              <button @click="confirmDelete(student)" class="transition-all duration-200">
                <DeleteIcon></DeleteIcon>
              </button>
            </fwb-table-cell>
          </fwb-table-row>
        </fwb-table-body>
      </fwb-table>
    </div>
  </div>

  <fwb-modal v-if="showModal" :size="'md'" not-escapable persistent class="no-print modal modal-open opacity-[99%] z-50"
    @close="showModal = false">
    <template #header>
      <fwb-heading tag="h3">Tambah Siswa</fwb-heading>
    </template>
    <template #body>
      <form @submit.prevent="addClassroom">
        <div class="form-control">
          <AutoComplete placeholder="cari siswa" label="Ketua Kelas" :service="'student'" v-model="form">
          </AutoComplete>
        </div>
        <div class="flex justify-end mt-5 gap-2">
          <fwb-button :color="'alternative'" type="button" @click="showModal = false">Cancel</fwb-button>
          <fwb-button :color="'green'" type="submit">Tambah</fwb-button>

        </div>
      </form>
    </template>
    <template #footer>

    </template>

  </fwb-modal>

  <classroommemberprint :classroom="classroom" v-if="showPrint"></classroommemberprint>

</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { DialogService, ToastService, ClassRoomService } from '@/services'
import AutoComplete from '@/components/AutoComplete.vue'
import { Helper, type ErrorResponse } from '@/commons'
import { AddIcon, DeleteIcon } from '@/components/icons'
import type { ClassRoom, Student } from '@/models'
import type ErrorResult from '@/models/Responses/ErrorResponse'
import PageHeader from '@/components/PageHeader.vue'
import classroommemberprint from './classroommemberprint.vue'
import { DetailIcon } from '@/components/icons'

import {
  FwbCard,
  FwbHeading,
  FwbButton, FwbModal,
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import { PrinterIcon } from '@heroicons/vue/24/solid'
import PrintStore from '@/stores/PrintModelStore';
import { DateTime } from 'luxon'
import AuthService from '@/services/AuthService'
const printStore = PrintStore();

const props = defineProps({
  classroomId: {
    type: Number,
    required: true,
  },
})

const isHomeRoomTeacher = ref(false)
AuthService.isHomeRoomTeacher().then((result) => {
  isHomeRoomTeacher.value = result
});

const showPrint = ref(false)
const data = reactive({
  showDeleteModal: false,
  error: {} as ErrorResponse,
  ketuaText: '',
  waliText: '',
  teachers: [],
  students: [],
  departments: [],
})

const classroom = ref({} as ClassRoom)
const form = ref({ id: 0, name: '' } as Student)
const showModal = ref(false)


try {
  ClassRoomService.getById(props.classroomId).then((response) => {
    if (!response.isSuccess) {
      ToastService.dangerToast('Data tidak ditemukan')
      return
    }
    classroom.value = response.data as ClassRoom
  })
} catch (error) {
  console.error('Error fetching data:', error)
}


const print = () => {
  printStore.setStudentClassMember(classroom.value.students)
  showPrint.value = true
  setTimeout(() => {
    window.print()
    showPrint.value = false
  }, 1000)
}

const addClassroom = async () => {
  try {
    const requestBody = {
      id: form.value.id,
    }

    if (form.value.id) {
      ClassRoomService.addStudentToClass(classroom.value.id, requestBody).then((response) => {
        if (response.isSuccess) {
          const student = {
            id: form.value.id,
            nis: form.value.nis,
            nisn: form.value.nisn,
            name: form.value.name,
            gender: form.value.gender,
            email: form.value.email,
            placeOfBorn: form.value.placeOfBorn,
            dateOfBorn: form.value.dateOfBorn
          } as unknown as Student
          classroom.value.students.push(student)
          showModal.value = false
          resetForm()
          ToastService.successToast('Data berhasil tambahkan')
        } else {
          data.error = response.error as ErrorResponse;
          ToastService.dangerToast(Helper.readDetailError(data.error))
        }
      })
    }
  } catch (error) {
    const errorResult = error as ErrorResult
    console.error('Error adding student to classroom:', errorResult)
    alert('Failed  add student to classroom. Please check the input data.')
  }
}

// Function to reset the form
const resetForm = () => {
  form.value = {
    id: 0,
  } as Student
}

// function ketuaKelasOnchange(tes) {
//   data.ketuaText = tes.target.value;
// }

// function waliKelasOnchange(tes) {
//   data.waliText = tes.target.value;
// }

// function searchKetua() {
//   StudentService.search(data.ketuaText).then(response => {
//     data.students = response.data.map((item) => {
//       return { id: item.id, name: item.name }
//     });
//   })
// }

// const diSableButton = computed(() => {
//   if (!form.value.id) {
//     return true
//   }
//   return false
// })

// let selectedClassRoom = {} as ClassRoom;

// const editClassroom = (classRoom: ClassRoom) => {
//   selectedClassRoom = classRoom;
//   showModal.value = true;
//   form.value.id = classRoom.id;
// }

const confirmDelete = (student: Student) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus siswa '${student.name}' dari kelas ?`,
    student,
    'warning',
    3000,
    'Batal',
    'Hapus',
  ).then(() => {
    deleteData(student)
  })
}

const deleteData = async (student: Student) => {
  if (student) {
    try {
      const response = await ClassRoomService.removeStudent(classroom.value.id, student.id)
      if (response.isSuccess) {
        ToastService.successToast('Data berhasil dihapus.')
        const index = classroom.value.students.indexOf(student)
        classroom.value.students.splice(index, 1)
      }
    } catch (error) {
      console.error('Error deleting data:', error)
    } finally {
      data.showDeleteModal = false
    }
  }
}
</script>
<style scoped></style>
