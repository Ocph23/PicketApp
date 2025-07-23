<template>
  <div class="print-only p-2">
    <div class="flex items-center">
      <LogoAppp class="w-12"></LogoAppp>
      <div>
        <FwbHeading :tag="'h6'">{{ Helper.infoSekolah.nama_sekolah }}</FwbHeading>
        <FwbHeading :tag="'h6'">JADWAL PIKET GURU </FwbHeading>
      </div>
    </div>
    <hr class="border-1 border-b-gray-400 my-2 w-full">
    <div class="flex justify-center items-center">
      <div>
        <FwbHeading :tag="'h6'">Tahun Ajaran {{ schoolYear.name }} {{ schoolYear.semesterName }}</FwbHeading>
      </div>
    </div>
    <div class="flex justify-end">
      <h6 class="text-right text-xs">Tanggal Cetak : {{ new Date().toLocaleDateString('id-ID') }}</h6>
    </div>
    <div class="my-1">
      <table class="table-auto w-full">
        <thead>
          <tr>
            <th class="border px-4 py-2">Hari</th>
            <th class="border px-4 py-2">Nama</th>
            <th class="border px-4 py-2">Keterangan</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item) in dayOfWeeks" :key="item.day">
            <td class="border px-4 ">
              <div> {{ item.title }}</div>
            </td>
            <td class="border px-4">
              <ul>
                <li v-for="(teacher, index1) in props.schedules[item.name]" :key="index1">
                  <span>{{ index1 + 1 }}. {{ teacher.teacherName }}</span>
                </li>
              </ul>
            </td>
            <td></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Schedule, SchoolYear } from '@/models';
import LogoAppp from '@/components/LogoApp.vue';
import { FwbHeading } from 'flowbite-vue';
import { Helper, type Dictionary } from '@/commons';
import _ from 'lodash';


// const printStore = PrintStore();
// const items: item[] = printStore.itemClassMemberPrint;

const props = defineProps<{
  schedules: Dictionary<Schedule[]>,
  schoolYear: SchoolYear
}>();



const dayOfWeeks = [
  { day: 1, name: 'Monday', title: 'Senin' },
  { day: 2, name: 'Tuesday', title: 'Selasa' },
  { day: 3, name: 'Wednesday', title: 'Rabu' },
  { day: 4, name: 'Thursday', title: 'Kamis' },
  { day: 5, name: 'Friday', title: 'Jumat' },
  { day: 6, name: 'Saturday', title: 'Sabtu' },
]


</script>

<style scoped>
label,
td,
th {
  font-size: 13px;
  padding: 3px;
}
</style>
