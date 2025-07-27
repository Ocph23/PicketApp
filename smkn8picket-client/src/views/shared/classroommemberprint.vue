<template>
  <div class="m-5 print-only">
    <div class="flex items-center">
      <LogoAppp class="w-12"></LogoAppp>
      <div>
        <FwbHeading :tag="'h5'">DAFTAR ANGGOTA KELAS</FwbHeading>
        <FwbHeading :tag="'h6'">{{ Helper.infoSekolah.nama_sekolah }}</FwbHeading>
      </div>
    </div>
    <hr class="border-1 border-b-gray-400 my-2 w-full">

    <div class="flex justify-between items-center">
      <div class="flex  flex-col">
        <div class="flex">
          <label class="w-32 ">Nama Kelas</label>
          <label for="className" class=" capitalize">: {{ props.classroom.className }}-{{
            classroom.departmentInitial }}</label>
        </div>
        <div class="flex">
          <label class="w-32 ">Nama Jurusan</label>
          <label for="className" class=" capitalize">: {{ classroom.departmentName }}</label>
        </div>
        <div class="flex">
          <label class="w-32 ">Wali Kelas</label>
          <label for="className" class=" capitalize">: {{ classroom.homeRoomTeacherName }}</label>
        </div>
        <div class="flex">
          <label class="w-32 ">Ketua Kelas</label>
          <label for="className" class=" capitalize">: {{ classroom.classLeaderName }}</label>
        </div>
      </div>
    </div>
    <div class="mt-4">
      <table class="table-print">
        <thead>
          <tr>
            <th>No</th>
            <th>NIS</th>
            <th>Nama</th>
            <th>Jenis Kelamin</th>
            <th>Email</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(student, index) in students" :key="student.id">
            <td>{{ index + 1 }}</td>
            <td>{{ student.nis }}/{{ student.nisn }}</td>
            <td>{{ student.name }}</td>
            <td>{{ student.gender == 0 ? 'Laki-laki' : 'Perempuan' }}</td>
            <td>{{ student.email }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { ClassRoom, Student } from '@/models';
import PrintStore from '@/stores/PrintModelStore';
import LogoAppp from '@/components/LogoApp.vue';
import { FwbHeading } from 'flowbite-vue';
import { Helper } from '@/commons';

const printStore = PrintStore();
const students: Student[] = printStore.studentClassMemberPrint;

const props = defineProps<{
  classroom: ClassRoom;
}>();


</script>

<style scoped>
label {
  font-size: 12px;
}

td {
  font-size: 12px;
}
</style>
