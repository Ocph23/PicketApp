<template>
  <AdminLayout>
    <VTCard title="DATA ABSEN GURU & STAF" class="no-print relative overflow-x-auto mt-1 p-3">
      <template #right-side>
        <VTIconPrint :size="'xl'" class="text-orange-300 cursor-pointer " @click="print">

        </VTIconPrint>
      </template>

      <div>
        <div class="flex justify-between mb-3">
          <div class="flex items-center gap-2 mt-2">
            <div class="flex items-end gap-2">
              <FwbSelect placeholder="Bulan" label="Bulan" :options="months" v-model="selectedMontAndYear.month">
              </FwbSelect>
              <FwbSelect placeholder="Tahun" label="Tahun" :options="years" v-model="selectedMontAndYear.year">
              </FwbSelect>
              <FwbButton @click="cariAbsen">Cari Absens</FwbButton>
            </div>
            <FwbButton v-if="attendances.length > 0" :color="'yellow'" type="submit"
              class="flex flex-row items-center justify-center p-2 w-28" @click="print">
              <PrinterIcon class="w-5 h-5"></PrinterIcon>
            </FwbButton>
          </div>
          <VTSelect label="Filter" placeholder="Jenis Pegawai" v-model="jobStatusSelected"
            :options="[{ name: 'Semua', value: 100 }, ...Helper.jobStatus]"></VTSelect>
        </div>
      </div>

      <fwb-table>
        <fwb-table-row>
          <fwb-table-head-cell class="w-5" rowspan="2">No</fwb-table-head-cell>
          <fwb-table-head-cell rowspan="2">Nama</fwb-table-head-cell>
          <fwb-table-head-cell class="text-center"
            :colspan="(source?.pickets?.length ?? 1) + 1">Tanggal</fwb-table-head-cell>
        </fwb-table-row>
        <fwb-table-row>
          <fwb-table-head-cell class="text-center" v-for="item in source?.pickets" :key="item.id ?? 0">
            {{ new Date(item.date).getDate() }}
          </fwb-table-head-cell>
          <fwb-table-head-cell class="w-1"></fwb-table-head-cell>
        </fwb-table-row>
        <fwb-table-body>
          <fwb-table-row v-for="(data, index) in displayData" :key="index">
            <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
            <fwb-table-cell>{{ data.teacherName }}</fwb-table-cell>
            <fwb-table-cell class="text-center" v-for="item in data.items" :key="item.picketId">
              {{ Helper.getAttendanceStatus(item.status, true) }}
            </fwb-table-cell>
            <fwb-table-head-cell class="w-1">

            </fwb-table-head-cell>
          </fwb-table-row>
        </fwb-table-body>
      </fwb-table>
    </VTCard>

    <!-- <classroomabsenprint :classroom="classroom" v-if="showPrint"></classroomabsenprint> -->
  </AdminLayout>
  <TeacherAttendanceReportPrint v-if="displayData?.length" 
    :pickets="source?.pickets"
    :jenis="Helper.jobStatus.find(x=>x.value==jobStatusSelected)?.name ||'semua'"
    :bulan="months.find(x=>x.value==selectedMontAndYear.month)?.name"
     :tahun="selectedMontAndYear.year" :attendances="displayData">
  </TeacherAttendanceReportPrint>
</template>

<script setup lang="ts">
import { Helper } from '@/commons'
import { TeacherAttendanceService, ToastService } from '@/services'
import { ref } from 'vue'

import TeacherAttendanceReportPrint from '@/views/shared/TeacherAttendanceReportPrint.vue'
//import classroomabsenprint from './classroomabsenprint.vue'

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
import type PrintAbsenModel from '@/models/AbsenPrintModel'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import type { TeacherAttendanceReport } from '@/models'
import { VTCard, VTIconPrint, VTSelect } from '@ocph23/vtocph23'
import { helpers } from '@vuelidate/validators'
// import PrintStore from '@/stores/PrintModelStore'
// const printStore = PrintStore()


const attendances = ref([] as Array<PrintAbsenModel>)
const source = ref<TeacherAttendanceReport>()
const displayData = ref<{
  teacherId: number
  teacherName: string
  jobStatus: number
  items: {
    picketId: number
    status: number
    timeIn: Date
    timeOut: Date
    description: string
    schoolYearId: number
  }[]
}[]>()
const showPrint = ref(false)
const jobStatusSelected = ref(100);

const months = Helper.getIndonesiaMonth().map((item, index) => {
  return { name: item, value: (index + 1).toString() }
})

const years = [] as { name: string; value: string }[]

const year = new Date().getFullYear()
for (let y = year; y > year - 4; y--) {
  const opt = { name: y.toString(), value: y.toString() }
  years.push(opt)
}

const selectedMontAndYear = ref({ month: '', year: '' })

const cariAbsen = () => {
  if (!selectedMontAndYear.value.month || !selectedMontAndYear.value.year) {
    ToastService.warningToast('Anda Harus Memilih Bulan dan Tahun')
    return
  }

  TeacherAttendanceService.get(
    Number(selectedMontAndYear.value.month),
    Number(selectedMontAndYear.value.year),
  ).then((response) => {
    source.value = response.data as TeacherAttendanceReport
    if (jobStatusSelected.value == 100) {
      displayData.value = source.value?.attendances
    } else {
      displayData.value = source.value?.attendances.filter(x => x.jobStatus == jobStatusSelected.value);
    }
  })
}

const print = () => {
  // printStore.setAbsen(attendances.value, dataTanggal.value)
  showPrint.value = true
  setTimeout(() => {
    window.print()
    showPrint.value = false
  }, 1000)
}

const filterPegawai = (data: any) => {
  displayData.value = source.value?.attendances.filter(x => x.jobStatus == data);
}

</script>
