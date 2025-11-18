<template>
  <div class="print-only">
    <div class="flex items-center">
      <LogoAppp class="w-12"></LogoAppp>
      <div>
        <FwbHeading :tag="'h5'">DAFTAR ABSEN GURU & STAFF</FwbHeading>
        <FwbHeading :tag="'h6'"> {{ Helper.infoSekolah.nama_sekolah }} </FwbHeading>
      </div>
    </div>
    <hr class="border-1 border-b-gray-400 my-2 w-full">

    <div class="flex justify-between items-center">
      <div class="flex  flex-col">
        <div class="flex">
          <label class="w-32 ">Bulan & Tahun</label>
          <label for="className" class=" capitalize">: {{ bulan }} {{tahun}}</label>
        </div>
        <div class="flex">
          <label class="w-32 ">Jenis Pegawai  </label>
          <label for="className" class=" capitalize">: {{jenis}}</label>
        </div>
      </div>
    </div>
    <div class="mt-4">
      <table class="table-auto w-full">
        <thead>
          <tr>
            <th class="border px-4 py-2" rowspan="2">No</th>
            <th class="border px-4 py-2" rowspan="2">Nama</th>
            <th class="border px-4 py-2">Tanggal</th>
          </tr>
          <tr>
            <th class="text-center" v-for="item in props.pickets" :key="item.id ?? 0">
              {{ new Date(item.date).getDate() }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(data, index) in props.attendances" :key="index">
            <td class="w-10 text-center">{{ index + 1 }}</td>
            <td class="text-left">{{ data.teacherName }}</td>
            <td class="text-center w-10" v-for="(item) in data.items" :key="item.picketId"> {{
              Helper.getAttendanceStatus(item.status, true)
              }}</td>
          </tr>
        </tbody>
      </table>
      <h5 class="mt-4 text-sm font-semibold">Catatan : </h5>
      <div class="flex flex-col">
        <label class="text-sm">- : Alpa/Tidak Hadir; </label>
        <label class="text-sm">H : Hadir; </label>
        <label class="text-sm">T : Terlambat; </label>
        <label class="text-sm">I : Izin; </label>
        <label class="text-sm">S : Sakit; </label>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import LogoAppp from '@/components/LogoApp.vue';
import { FwbHeading } from 'flowbite-vue';
import { Helper } from '@/commons';


const props = defineProps<{
  bulan?: string,
  tahun?: string,
  jenis?: string,
  pickets?: {
    id: null
    schoolYearId: number
    date: Date
    endAt: string
  }[],
  attendances?: {
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
  }[];
}>();


</script>

<style scoped>

td,th{
  border: 1px solid rgb(222, 222, 222) !important;
  padding: 5px !important;
}

label {
  font-size: 12px;
}
</style>
