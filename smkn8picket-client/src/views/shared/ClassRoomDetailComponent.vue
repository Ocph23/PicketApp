<template>
  <div class="no-print">
    <PageHeader title="Detail Kelas"> </PageHeader>
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <fwb-card class="!max-w-full bg-red-100 p-4 rounded-lg shadow-md">
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

    </div>

    <!-- Modal -->


    <!-- Classroom Table -->

    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-semibold"></h1>
      <div class="flex flex-column justify-center items-center ">
        <button v-if="classroom.schoolYearId == schoolYearActive?.id" @click="() => showModal = true"
          class="transition rounded text-black m-3  w-6 items-center text-center">
          <AddIcon class="w-7 h-7" />
        </button>
        <VTButton @click="print" class="cursor-pointer ">
          <PrinterIcon class="w-7 h-7 text-yellow-400 hover:text-yellow-600  transition-all duration-200"></PrinterIcon>
        </VTButton>

      </div>
    </div>
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg mt-1">
      <fwb-table class="table w-full">
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
              <router-link
                :to="isHomeRoomTeacher ? `/walikelas/siswa/${student.id}` : `/admin/siswa/${student.id}/detail`">
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

  <form @submit.prevent="addClassroom">
    <fwb-modal v-if="showModal" :size="'md'" not-escapable persistent
      class="no-print modal modal-open opacity-[99%] z-50" @close="showModal = false">
      <template #header>
        <fwb-heading tag="h3">Tambah Siswa</fwb-heading>
      </template>
      <template #body>
        <div class="form-control">
          {{ selectedStudents }}
          <VTAutocomplete placeholder="cari siswa" label="Nama Siswa" :sources="studentOptions"
            v-model="selectedStudents" :multiple="true" @search="handleSearch" />
        </div>
      </template>
      <template #footer>
        <div class="flex justify-end gap-2">
          <fwb-button :color="'alternative'" type="button" @click="showModal = false">Cancel</fwb-button>
          <fwb-button :color="'green'" type="submit">Tambah</fwb-button>

        </div>
      </template>
    </fwb-modal>
  </form>

  <classroommemberprint :classroom="classroom" v-if="showPrint"></classroommemberprint>

</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { ClassRoomService, SchoolYearService, StudentService } from '@/services'
import { Helper, type ErrorResponse } from '@/commons'
import { AddIcon, DeleteIcon } from '@/components/icons'
import { type ClassRoom, type SchoolYear, type Student } from '@/models'
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
import { DateTime } from 'luxon'
import AuthService from '@/services/AuthService'
import PrintStore from '@/stores/PrintModelStore';
import { VTDialogService, VTToastService, VTAutocomplete } from '@ocph23/vtocph23'
import type { SelectOption, VTButton } from '@ocph23/vtocph23'
import type AddStudentsToClassroomResponse from '@/models/Responses/AddStudentsToClassroomResponse'
const printStore = PrintStore();

const props = defineProps({
  classroomId: {
    type: Number,
    required: true,
  },
})

const schoolYearActive = ref<SchoolYear>()

const isHomeRoomTeacher = ref(false)
AuthService.isHomeRoomTeacher().then((result) => {
  isHomeRoomTeacher.value = result
  SchoolYearService.getActive().then((response) => {
    if (response.isSuccess)
      schoolYearActive.value = response.data as SchoolYear
  })
});

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
// const form = ref({ id: 0, name: '' } as Student)
const selectedStudents = ref<number[]>([])
const studentOptions = ref<SelectOption[]>([])
const showModal = ref(false)
const showPrint = ref(false)

const handleSearch = async (query: string) => {
  if (query && query.length >= 2) {
    const response = await StudentService.search(query)
    if (response.isSuccess && response.data) {
      const students = response.data as Student[]
      studentOptions.value = students.map((student) => ({
        value: student.id,
        name: student.name
      }))
    }
  }
}


try {
  ClassRoomService.getById(props.classroomId).then((response) => {
    if (!response.isSuccess) {
      VTToastService.error('Data tidak ditemukan')
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
    if (selectedStudents.value.length === 0) {
      VTToastService.error('Pilih minimal 1 siswa untuk ditambahkan')
      return
    }

    const studentIds = selectedStudents.value
    const response = await ClassRoomService.addStudentsToClass(classroom.value.id, studentIds)

    if (response.isSuccess) {
      const result = response.data as AddStudentsToClassroomResponse;
      const successCount = result.successCount || 0
      const failedCount = result.failedCount || 0

      // Refresh classroom data to get updated student list
      const refreshResponse = await ClassRoomService.getById(props.classroomId)
      if (refreshResponse.isSuccess) {
        classroom.value = refreshResponse.data as ClassRoom
      }

      showModal.value = false
      selectedStudents.value = []
      studentOptions.value = []

      if (failedCount === 0) {
        VTToastService.success(`Berhasil menambahkan ${successCount} siswa`)
      } else {
        VTToastService.warning(`Berhasil menambahkan ${successCount} siswa, ${failedCount} gagal`)
        if (result.errors && result.errors.length > 0) {
          result.errors.forEach((error: string) => {
            VTToastService.warning(error)
          })
        }
      }
    } else {
      VTToastService.error('Gagal menambahkan siswa')
    }
  } catch (error) {
    const errorResult = error as ErrorResult
    console.error('Error adding students to classroom:', errorResult)
    VTToastService.error('Terjadi kesalahan saat menambahkan siswa')
  }
}

// // Function to reset the form
// const resetForm = () => {
//   form.value = {
//     id: 0,
//   } as Student
//   selectedStudents.value = []
//   studentOptions.value = []
// }

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
  VTDialogService.asyncShowDialog(
    'Perhatian',
    `Apakah Anda yakin ingin menghapus siswa '${student.name}' dari kelas ?`,
    student,
    'warning',
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
        VTToastService.success('Data berhasil dihapus.')
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
