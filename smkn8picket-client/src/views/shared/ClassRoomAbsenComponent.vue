<template>
  <div class="no-print relative overflow-x-auto shadow-md sm:rounded-lg mt-1 p-3">
    <div class="">
      <PageHeader title="DATA ABSEN"> </PageHeader>

      <fwb-card class="!max-w-1/2  bg-white p-4 rounded-lg shadow-md">
        <div class="flex gap-1 flex-col grid-cols-2">
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

    <div class="flex justify-between mb-3">
      <div class="flex">
        <FwbHeading :tag="'h6'"></FwbHeading>
      </div>
      <div class="flex  gap-2 mt-2">
        <div class="flex gap-2">
          <FwbSelect placeholder="Bulan" label="Bulan" :options="months" v-model="selectedMontAndYear.month">
          </FwbSelect>
          <FwbSelect placeholder="Tahun" label="Tahun" :options="years" v-model="selectedMontAndYear.year">
          </FwbSelect>
        </div>
        <FwbButton @click="cariAbsen">Cari Absens</FwbButton>
        <FwbButton v-if="dataSiswa.length > 0" :color="'yellow'" type="submit"
          class="flex flex-row items-center justify-center p-2 w-28" @click="print">
          <PrinterIcon class="w-5 h-5"></PrinterIcon>
        </FwbButton>
      </div>
    </div>
    <fwb-table>
      <fwb-table-row>
        <fwb-table-head-cell class="w-5" rowspan="2">No</fwb-table-head-cell>
        <fwb-table-head-cell rowspan="2">Nama</fwb-table-head-cell>
        <fwb-table-head-cell class="text-center">Tanggal</fwb-table-head-cell>
      </fwb-table-row>
      <fwb-table-row>
        <fwb-table-head-cell class="text-center" v-for="item in dataTanggal" :key="item">{{
          new Date(item).getDate()
        }}</fwb-table-head-cell>
        <fwb-table-head-cell class="w-1"></fwb-table-head-cell>
      </fwb-table-row>
      <fwb-table-body>
        <fwb-table-row v-for="(data, index) in dataSiswa" :key="index">
          <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
          <fwb-table-cell>{{ data.name }}</fwb-table-cell>
          <fwb-table-cell class="text-center" v-for="item in data.absen" :key="item">{{
            item
          }}</fwb-table-cell>
          <fwb-table-head-cell class="w-1">
            <router-link :to="`/Siswa/${data.studentId}/detail`">
              <DetailIcon></DetailIcon>
            </router-link>
          </fwb-table-head-cell>
        </fwb-table-row>
      </fwb-table-body>
    </fwb-table>
  </div>


  <classroomabsenprint :classroom="classroom" v-if="showPrint"></classroomabsenprint>
</template>

<script setup lang="ts">
import { Helper } from '@/commons'
import type { ClassRoom, StudentAttendance } from '@/models'
import { ClassRoomService, StudentAttendanceService } from '@/services'
import { orderBy, groupBy, forEach, type Dictionary } from 'lodash'
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import PageHeader from '@/components/PageHeader.vue'
import { FwbCard } from 'flowbite-vue'
import classroomabsenprint from './classroomabsenprint.vue'


import {
  FwbButton,
  FwbHeading,
  FwbSelect,
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import { PrinterIcon } from '@heroicons/vue/24/solid'
import PrintStore from '@/stores/PrintModelStore';
import type PrintAbsenModel from '@/models/AbsenPrintModel'
import DetailIcon from '@/components/icons/DetailIcon.vue'
import { VTToastService } from '@ocph23/vtocph23'
const printStore = PrintStore();



const route = useRoute()

const dataSiswa = ref([] as Array<PrintAbsenModel>)
const dataTanggal = ref([] as Array<string>)
const showPrint = ref(false)


const months = [
  { name: 'Januari', value: '1' },
  { name: 'Februari', value: '2' },
  { name: 'Maret', value: '3' },
  { name: 'April', value: '4' },
  { name: 'Mei', value: '5' },
  { name: 'Juni', value: '6' },
  { name: 'Juli', value: '7' },
  { name: 'Agustus', value: '8' },
  { name: 'September', value: '9' },
  { name: 'Oktober', value: '10' },
  { name: 'Nopember', value: '11' },
  { name: 'Desember', value: '12' },
];

const years = [] as { name: string; value: string }[]

const year = new Date().getFullYear()
for (let y = year; y > year - 3; y--) {
  const opt = { name: y.toString(), value: y.toString() }
  years.push(opt)
}

const selectedMontAndYear = ref({ month: '', year: '' })
const classroom = ref({} as ClassRoom)

ClassRoomService.getById(Number(route.params.id)).then((response) => {
  if (!response.isSuccess) {
    VTToastService.error('Data tidak ditemukan')
    return
  }
  classroom.value = response.data as ClassRoom
})

const cariAbsen = () => {
  if (!selectedMontAndYear.value.month || !selectedMontAndYear.value.year) {
    VTToastService.warning('Anda Harus Memilih Bulan dan Tahun')
    return
  }

  StudentAttendanceService.get(
    Number(route.params.id),
    Number(selectedMontAndYear.value.month),
    Number(selectedMontAndYear.value.year),
  ).then((response) => {
    dataSiswa.value = []
    dataTanggal.value = []
    const datasource = response.data as StudentAttendance[]
    const students = groupBy(datasource, (item) => `${item.studentName}`) as Dictionary<
      StudentAttendance[]
    >
    const dates = groupBy(datasource, (item) => `${item.picketDate}`) as Dictionary<
      StudentAttendance[]
    >

    const xx = [] as Array<PrintAbsenModel>;
    forEach(students, (data, key) => {
      const absen = data.map((item) => Helper.getAttendanceStatus(item.status, true))
      const d = { name: key, absen: absen, studentId: data[0].studentId } as PrintAbsenModel;
      xx.push(d)
    })

    dataSiswa.value = orderBy(xx, ['name'], ['asc'])
    forEach(dates, (data, key) => {
      dataTanggal.value.push(key)
    })
  })
}



const print = () => {
  printStore.setAbsen(dataSiswa.value, dataTanggal.value)
  showPrint.value = true
  setTimeout(() => {
    window.print()
    showPrint.value = false
  }, 1000)
}



</script>
