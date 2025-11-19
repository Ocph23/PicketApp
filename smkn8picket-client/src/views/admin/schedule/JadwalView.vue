<template>
  <AdminLayout>
    <VTCard title="Jadwal Piket" class="no-print">
      <template #rightSide>
        <PrinterIcon class="w-8 h-8 cursor-pointer text-amber-400" @click="print"></PrinterIcon>
      </template>
      <h6
        class="text-center mb-8 text-xl font-bold leading-none tracking-tight text-gray-900 dark:text-white  md:mx-auto">
        <span v-if="data.schoolYear" class="relative inline-block">
          Tahun Ajaran : {{ data.schoolYear.name }} {{ data.schoolYear.semesterName }}
        </span>
      </h6>


      <div class="grid grid-cols-2 gap-2">
        <div v-for="item in dayOfWeeks" :key="item.name" class="my-2">
          <div class="shadow-lg rounded-lg bg-slate-100 dark:bg-gray-500">
            <div class="flex justify-between items-center p-3 gap-x-4 mb-3">
              <div class="flex items-center">
                <div
                  class="inline-flex justify-center items-center w-10 h-10 rounded-full border-4 border-blue-50 bg-blue-100 dark:border-blue-900 dark:bg-blue-800">
                  <svg class="w-4 h-4 text-blue-800 dark:text-blue-300 hover:animate-spin-on-hover"
                    xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
                    <path
                      d="M20 4a2 2 0 0 0-2-2h-2V1a1 1 0 0 0-2 0v1h-3V1a1 1 0 0 0-2 0v1H6V1a1 1 0 0 0-2 0v1H2a2 2 0 0 0-2 2v2h20V4ZM0 18a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V8H0v10Zm5-8h10a1 1 0 0 1 0 2H5a1 1 0 0 1 0-2Z" />
                  </svg>
                </div>
                <div class="shrink-0">
                  <h3 class="text-lg ml-3 font-semibold text-gray-800 dark:text-white">
                    {{ item.title }}
                  </h3>
                </div>
              </div>
              <!-- start modal  -->
              <div class="mr-7">
                <button v-if="canEdit" class="dark:text-white w-7 h-7 cursor-pointer" @click="showModal(item.day)">
                  <UserPlusIcon class="r-10"></UserPlusIcon>
                </button>
              </div>

              <!-- close modal  -->
            </div>

            <div class="overflow-x-auto grid">
              <table
                class="overflow-x-auto min-w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                <thead
                  class="text-xs text-center text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                  <tr>
                    <th class="w-5 px-6 py-3">No</th>
                    <th class="w-10 text-center px-6 py-3">Photo</th>
                    <th class="w-1/2 text-left px-6 py-3">Nama</th>
                    <th class="w-48 text-left px-6 py-3">NIP/Nomor Induk</th>
                    <th v-if="canEdit" class="w-10 px-6 py-3"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr
                    class="bg-white text-center border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                    v-for="(schedule, index) in schedules[item.name]" :key="index">
                    <td class="text-center px-6 py-3">{{ index + 1 }}</td>
                    <td class="text-center px-4 py-3">
                      <img class="w-10 h-10 rounded-full"
                        :src="Helper.getTeacherAvatar(schedule.photo ? schedule.photo : '')" alt="" />
                    </td>
                    <td class="text-left px-6 py-3">{{ schedule.teacherName }}</td>
                    <td class="text-left px-6 py-3">{{ schedule.teacherNumber }}</td>
                    <td v-if="canEdit">
                      <button @click="deleteData(schedule)"
                        class="transition-all duration-200 text-red-500 hover:text-red-700 rounded-lg">
                        <DeleteIcon></DeleteIcon>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <fwb-modal class="modal opacity-[99%]" v-if="modal" :size="'3xl'">
        <template #header>
          <FwbHeading tag="h3" class="text-lg text-center pb-3 font-bold">
            Daftar Guru Picket
          </FwbHeading>
        </template>
        <template #body>
          <div class="modal-box h-[65vh] overflow-auto">
            <FwbTable
              class="overflow-x-auto min-w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <FwbTableHead
                class="text-xs text-center text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <FwbTableHeadCell class="px-6 py-3">Teacher</FwbTableHeadCell>
                <FwbTableHeadCell class="px-6 py-3">Email</FwbTableHeadCell>
                <FwbTableHeadCell class="px-6 py-3">Action</FwbTableHeadCell>
              </FwbTableHead>
              <FwbTableBody v-if="data.teachersNotHaveSchedule">
                <FwbTableRow
                  class="bg-white text-center border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                  v-for="(teacher, index) in data.teachersNotHaveSchedule" :key="index">
                  <FwbTableCell scope="row"
                    class="flex items-center px-6 py-4 text-gray-900 whitespace-nowrap dark:text-white">
                    <div class="pl-3 flex justify-center items-center">
                      <img class="w-10 h-10 rounded-full" :src="Helper.getTeacherAvatar(teacher.photo)" alt="" />
                      <div class="text-base font-medium ml-3">
                        {{ teacher.name }}
                      </div>
                    </div>
                  </FwbTableCell>
                  <FwbTableCell class="px-6 py-4">{{ teacher.registerNumber }}</FwbTableCell>
                  <FwbTableCell class="mt-5 w-full flex justify-end">
                    <FwbButton :color="'alternative'" @click="addTeacherToSchedule(teacher)">Pilih</FwbButton>
                  </FwbTableCell>
                </FwbTableRow>
              </FwbTableBody>
            </FwbTable>
          </div>
        </template>
        <template #footer>
          <div class="modal-action w-full flex justify-end ">
            <FwbButton :color="'alternative'" @click="modal = false">Keluar</FwbButton>
          </div>
        </template>
      </fwb-modal>
    </VTCard>
  </AdminLayout>

  <PiketSchedulePrint :schedules="schedules" :school-year="data.schoolYear" v-if="showPrint"></PiketSchedulePrint>

</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { useRoute } from 'vue-router'
import { TeacherService, ScheduleService, SchoolYearService } from '@/services'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { Helper, type Dictionary } from '@/commons'
import type { Schedule, SchoolYear, Teacher } from '@/models'
import type ScheduleRequest from '@/models/Requests/ScheduleRequest'
import { groupBy } from 'lodash'
import DeleteIcon from '@/components/icons/DeleteIcon.vue'
import { UserPlusIcon, PrinterIcon } from '@heroicons/vue/24/solid'
import {
  FwbButton,
  FwbHeading,
  FwbModal,
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'

import PiketSchedulePrint from '@/views/shared/PiketSchedulePrint.vue'
import { VTCard, VTDialogService, VTToastService } from '@ocph23/vtocph23'

const route = useRoute()
const modal = ref(false)
const scheduleId = ref(Number(route.params.id))


if (!scheduleId.value) {
  SchoolYearService.getActive().then((response) => {
    if (response.isSuccess) {
      data.schoolYear = response.data as SchoolYear
      scheduleId.value = data.schoolYear.id
      getData();
    }
  })
} else {
  SchoolYearService.getById(scheduleId.value).then((response) => {
    if (response.isSuccess) {
      data.schoolYear = response.data as SchoolYear
    }
    getData();
  })
}


const schedules = ref<Dictionary<Schedule[]>>({} as Dictionary<Schedule[]>)
const data = reactive({
  teachers: [] as Teacher[],
  schoolYear: {} as SchoolYear,
  teachersNotHaveSchedule: [] as Teacher[],
})

const dayOfWeeks = [
  { day: 1, name: 'Monday', title: 'Senin' },
  { day: 2, name: 'Tuesday', title: 'Selasa' },
  { day: 3, name: 'Wednesday', title: 'Rabu' },
  { day: 4, name: 'Thursday', title: 'Kamis' },
  { day: 5, name: 'Friday', title: 'Jumat' },
  { day: 6, name: 'Saturday', title: 'Sabtu' },
]
// Fungsi untuk mengambil data (GET)

const canEdit = computed(() => {
  return data.schoolYear && data.schoolYear.actived
})

function getData() {
  try {
    (async () => {
      const response = await ScheduleService.getBySchoolYearId(scheduleId.value)
      const dataResponse = response.data as Schedule[]
      schedules.value = groupBy<Schedule>(dataResponse, 'dayOfWeek')
      const teacherResponse = await TeacherService.get()
      data.teachers = teacherResponse.data as Teacher[]
    })()
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}


const addTeacherToSchedule = async (teacher: Teacher) => {
  model.teacherId = teacher.id
  const response = await ScheduleService.post(model)
  if (response.isSuccess) {
    const week = dayOfWeeks.find((x) => x.day == model.dayOfWeek)
    const weekName = week ? week.name : dayOfWeeks[0].name
    if (!schedules.value[weekName]) {
      schedules.value[weekName] = []
    }

    const schedule = response.data as Schedule
    schedule.teacherId = teacher.id
    schedule.teacherName = teacher.name
    schedule.teacherNumber = teacher.registerNumber
    schedule.photo = teacher.photo
    schedules.value[weekName].push(schedule)
    modal.value = false
    VTToastService.success('Data berhasil disimpan !')
  } else {
    VTToastService.error('Data gagal disimpan !')
  }
}

//
const model = reactive({
  dayOfWeek: 0,
  teacherId: 0,
  teacherName: '',
} as ScheduleRequest)

const showModal = (dayOfWeek: number) => {
  data.teachersNotHaveSchedule = [] as Teacher[]
  model.dayOfWeek = dayOfWeek
  const teacherExists = [] as Schedule[]
  dayOfWeeks.forEach((s) => {
    const datas = schedules.value[s.name]
    if (datas) {
      datas.forEach((element) => {
        teacherExists.push(element)
      })
    }
  })

  data.teachersNotHaveSchedule = data.teachers.filter((el) => {
    return !teacherExists.find((obj) => obj.teacherId === el.id)
  })

  setTimeout(() => {
    modal.value = true
  }, 500)
}

const deleteData = (schedule: Schedule) => {
  VTDialogService.asyncShowDialog('Perhatian', 'Yakin Hapus Data ?', schedule.id, 'danger').then(() => {
    ScheduleService.delete(schedule.id).then((deleteResponse) => {
      if (deleteResponse.isSuccess) {
        VTToastService.success('Data berhasil dihapus')
        const index = schedules.value[schedule.dayOfWeek].indexOf(schedule)
        schedules.value[schedule.dayOfWeek].splice(index, 1)
      } else {
        VTToastService.error('Data gagal dihapus')
      }
    })
  })
}


const showPrint = ref(false)

const print = () => {
  // printStore.setStudentClassMember(classroom.value.students)
  showPrint.value = true
  setTimeout(() => {
    window.print()
    showPrint.value = false
  }, 1000)
}

</script>
