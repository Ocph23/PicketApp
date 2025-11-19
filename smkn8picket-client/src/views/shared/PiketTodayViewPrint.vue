<template>
  <div class="print-only m-3">
    <div class="flex items-center">
      <LogoApp class="w-12"></LogoApp>
      <div>
        <FwbHeading :tag="'h6'" :custom-size="'1px'">LAPORAN PIKET HARIAN</FwbHeading>
        <FwbHeading :tag="'h6'">{{ Helper.infoSekolah.nama_sekolah }} </FwbHeading>
      </div>
    </div>
    <hr class="border-1 border-b-gray-400 my-2 w-full">

    <div class="">
      <div class="flex">
        <label class="labelTitle dark:text-white">Tanggal</label>
        <label class="labelValue dark:text-white">:
          {{ DateTime.fromISO(piket.date).toFormat('dd-MM-yyyy') }}
        </label>
      </div>
      <div class="flex">
        <label class="labelTitle dark:text-white">Cuaca</label>
        <label class="labelValue dark:text-white">: {{ Helper.getWeartherString(piket.weather) }}</label>
      </div>
      <div class="flex">
        <label class="labelTitle dark:text-white">Jam Pembelajaran</label>
        <label class="labelValue dark:text-white">: {{ piket.startAt }} - {{ piket.endAt }} </label>
      </div>
      <div class="flex">
        <label class="labelTitle dark:text-white">Piket Dibuka oleh</label>
        <label class="labelValue dark:text-white">: {{ piket.createdName }}</label>
      </div>
    </div>
    <hr class="mb-5" />
    <h6 class="mt-5 text-sm">Catatan Peristiwa/Kegiatan</h6>
    <table class="table-print w-full">
      <thead>
        <tr>
          <th>No</th>
          <th>Jam</th>
          <th>Judul</th>
          <th>Keterangan</th>
          <th>Dibuat Oleh</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td colspan="5" v-if="piket.dailyJournal && piket.dailyJournal.length === 0">Tidak Ada Data </td>
        </tr>
        <tr v-for="(journal, index) in piket.dailyJournal" :key="journal.id">
          <td class="w-12 text-center">{{ index + 1 }}</td>
          <td class="min-w-[60px]">{{ new Date(journal.createAt).toLocaleTimeString() }}</td>
          <td class="min-w-[100px]">{{ journal.title }}</td>
          <td>{{ journal.content }}</td>
          <td class="min-w-[100px]">{{ journal.teacherName }}</td>
        </tr>
      </tbody>
    </table>

    <h6 class="mt-5 text-sm"> Kehadiran Siswa</h6>
    <table class="table-print w-full">
      <thead>
        <tr>
          <th>No</th>
          <th>Kelas</th>
          <th>Hadir</th>
          <th>Terlambat</th>
          <th>Sakit</th>
          <th>Izin</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td colspan="6" v-if="kehadiran.length === 0">Tidak Ada Data </td>
        </tr>
        <tr v-for="(absen, index) in kehadiran" :key="index">
          <td>{{ index + 1 }}</td>
          <td v-for="(item, index2) in absen" :key="index2" :class="index2 === 0 ? 'text-left' : 'text-center'">{{ item
          }}</td>
        </tr>
      </tbody>
    </table>


    <h6 class="mt-5 font-bold text-sm"> Siswa Pulang Cepat</h6>
    <table class="table-print w-full">
      <thead>
        <tr>
          <th>No</th>
          <th>Waktu</th>
          <th>Nama Siswa</th>
          <th>Kelas</th>
          <th>Status</th>
          <th>Keterangan/Alasan</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td colspan="6" v-if="piket.studentsLateAndComeHomeEarly && piket.studentsLateAndComeHomeEarly.length === 0">
            Tidak Ada Data </td>
        </tr>
        <tr v-for="(absen, index) in piket.studentsLateAndComeHomeEarly" :key="index">
          <td>{{ index + 1 }}</td>
          <td>{{ absen.time }}</td>
          <td>{{ absen.studentName }}</td>
          <td>{{ absen.classRoomName }} {{ absen.departmentName }}</td>
          <td>{{ Helper.getAttendanceStatus(absen.attendanceStatus) }}</td>
          <td>{{ absen.description }}</td>
        </tr>
      </tbody>
    </table>



  </div>
</template>

<script setup lang="ts">
import { Helper } from '@/commons';
import LogoApp from '@/components/LogoApp.vue';
import type { Picket } from '@/models';
import { FwbHeading } from 'flowbite-vue';
import { forEach, groupBy } from 'lodash';
import { DateTime } from 'luxon';


const props = defineProps<{ data: Picket }>();
const piket = props.data;
const kehadiran = [] as Array<[string, number, number, number, number]>;

const dataKelas = groupBy(props.data.studentAttendance, item => `${item.className}-${item.departmentName}`);

forEach(dataKelas, (value, key) => {
  const hadir = value.filter(item => item.status === 1 || item.status === 2).length;
  const terlambat = value.filter(item => item.status === 2).length;
  const sakit = value.filter(item => item.status === 4).length;
  const izin = value.filter(item => item.status === 5).length;
  kehadiran.push([key, hadir, terlambat, sakit, izin]);
});
;


</script>

<style scoped>
.labelTitle {
  margin-right: 10px;
  width: 150px;
  font-size: 12px;
}

.labelValue {
  font-weight: normal;
  font-size: 12px;
}
</style>
