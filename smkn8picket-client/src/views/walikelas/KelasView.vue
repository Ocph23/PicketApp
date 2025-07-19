<template>
  <WaliKelasLayout>
    <div class="">
      <PageHeader title="Data Kelas" />

      <!-- Classroom Table -->
      <div class="relative overflow-x-auto shadow-md sm:rounded-lg mt-1">
        <fwb-table>
          <fwb-table-head>
            <fwb-table-head-cell>No</fwb-table-head-cell>
            <fwb-table-head-cell>Nama Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>Tingkat</fwb-table-head-cell>
            <fwb-table-head-cell>Nama Jurusan</fwb-table-head-cell>
            <fwb-table-head-cell>Ketua Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>Wali Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>TA</fwb-table-head-cell>
            <fwb-table-head-cell>Action</fwb-table-head-cell>
          </fwb-table-head>
          <fwb-table-body v-if="classrooms.length > 0">
            <fwb-table-row v-for="(classroom, index) in classrooms.sort((a, b) => a.level - b.level)" :key="index"
              class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
              <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.className }}-{{ classroom.departmentInitial }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.level }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.departmentName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.classLeaderName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.homeRoomTeacherName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.year }}/{{ classroom.year + 1 }}</fwb-table-cell>
              <fwb-table-cell class="flex gap-2 items-center justify-start">
                <router-link :to="`/walikelas/kelas/${classroom.id}`">
                  <InformationCircleIcon class="w-5 h-5 text-blue-500" />
                </router-link>
                <router-link :to="`/walikelas/kelas/absen/${classroom.id}`">
                  <DetailIcon />
                </router-link>
              </fwb-table-cell>
            </fwb-table-row>
          </fwb-table-body>
        </fwb-table>
      </div>
    </div>
  </WaliKelasLayout>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import {
  ToastService,
  ClassRoomService,
  DepartmentService,
  SchoolYearService,
} from '@/services'
import { Helper, type ErrorResponse, type RequestResponse } from '@/commons'
import { DetailIcon } from '@/components/icons'
import PageHeader from '@/components/PageHeader.vue'
import type { ClassRoom, Department, SchoolYear } from '@/models'
// import AutoCompleteStore from '@/stores/AutoCompleteStore'

import {
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import { InformationCircleIcon } from '@heroicons/vue/24/solid'
import WaliKelasLayout from '@/components/layouts/WaliKelasLayout.vue'


const data = reactive({
  errors: [],
  ketuaText: '',
  waliText: '',
  teachers: [],
  students: [],
  departments: [] as { value: string; name: string }[],
})

// const autoCompleteStore = AutoCompleteStore();
const schoolYearSelected = ref();
const schoolYearActive = ref();
const schoolYears = ref<{
  name: string;
  value: string;
  actived?: boolean;
}[]>([]);

SchoolYearService.get().then((response: RequestResponse) => {
  const data = response.data as SchoolYear[];
  schoolYears.value = data.map((item) => {
    return {
      value: item.id.toString(),
      name: `${item.name} ${item.semesterName}`,
      actived: item.actived,
    }
  }).reverse();
  const schoolYearActive = data.find((item) => item.actived);
  schoolYearSelected.value = schoolYearActive?.id.toString() || '';
  showData();
})

DepartmentService.get().then((response) => {
  const departments = response.data as Department[]
  data.departments = departments.map((item: Department) => {
    return { value: item.id.toString(), name: item.name }
  })
})


const showData = () => {

  const schoolYear = schoolYears.value.find((item) => item.value == schoolYearSelected.value)
  if (schoolYear) {
    schoolYearActive.value = schoolYear.actived;
  } else {
    schoolYearActive.value = false;
  }

  ClassRoomService.getByTeacher().then((response) => {
    if (response.isSuccess) {
      classrooms.value = response.data as ClassRoom[]
    } else {
      ToastService.dangerToast(Helper.readDetailError(response.error as ErrorResponse))
    }
  })
}


const classrooms = ref([] as ClassRoom[])

</script>
