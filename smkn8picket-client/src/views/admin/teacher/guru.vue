<template>
  <AdminLayout>
    <div>
      <PageHeader title="Data Guru & Staff">
        <router-link :to="{ name: 'addGuru' }">
          <AddIcon class="w-7 h-7" />
        </router-link>
      </PageHeader>
      <div class="relative overflow-x-auto  shadow-md sm:rounded-lg mt-1">
        <fwb-table>
          <fwb-table-head>
            <fwb-table-head-cell>No</fwb-table-head-cell>
            <fwb-table-head-cell>Nama</fwb-table-head-cell>
            <fwb-table-head-cell>NIP/Nomo Induk</fwb-table-head-cell>
            <fwb-table-head-cell>Jenis Kelamin</fwb-table-head-cell>
            <fwb-table-head-cell>TTL</fwb-table-head-cell>
            <fwb-table-head-cell>Email</fwb-table-head-cell>
            <fwb-table-head-cell class="w-5">
              <span class="sr-only"></span>
            </fwb-table-head-cell>
          </fwb-table-head>
          <fwb-table-body class="overscroll-y-auto !max-h-[50vh]">
            <fwb-table-row v-for="(teacher, index) in data.teachers" :key="index">
              <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
              <fwb-table-cell class="flex items-center gap-2">
                <img class="w-8 h-8 rounded-full" :src="Helper.getTeacherAvatar(teacher.photo)">
                {{ teacher.name }}</fwb-table-cell>
              <fwb-table-cell>{{ teacher.registerNumber }}</fwb-table-cell>
              <fwb-table-cell> {{ teacher.gender === 0 ? "Laki-laki" : "Perempuan" }}</fwb-table-cell>
              <fwb-table-cell>
                {{ teacher.placeOfBorn }}, {{ !teacher.dateOfBorn ? "" :
                  Helper.getDateTimeString(new Date(teacher.dateOfBorn), 'dd-MM-yyyy') }}
              </fwb-table-cell>
              <fwb-table-cell>{{ teacher.email }}</fwb-table-cell>
              <fwb-table-cell>
                <div class="flex items-center">
                  <router-link :to="`/admin/guru/${teacher.id}/edit`">
                    <button class="text-white flex">
                      <EditIcon></EditIcon>
                    </button>
                  </router-link>
                  <button @click="confirmDelete(teacher)" class="text-white flex cursor-pointer">
                    <DeleteIcon />
                  </button>
                  <button @click="setAdminConfirm(teacher)" class="text-white flex cursor-pointer">
                    <LockClosedIcon class="w-5 h-5 text-sky-600 hover:text-sky-900" />
                  </button>
                </div>
              </fwb-table-cell>
            </fwb-table-row>

          </fwb-table-body>
        </fwb-table>
      </div>
    </div>
  </AdminLayout>
</template>


<script setup lang="ts">
import { onMounted, reactive } from "vue";
import AdminLayout from "@/components/layouts/AdminLayout.vue";
import { TeacherService, ToastService, DialogService } from "@/services";
import { Helper } from "@/commons";
import { EditIcon, DeleteIcon, AddIcon } from "@/components/icons";
import PageHeader from "@/components/PageHeader.vue";
import {
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import type { Teacher } from "@/models";
import { LockClosedIcon } from "@heroicons/vue/20/solid";
import AuthService from "@/services/AuthService";


const data = reactive({
  teachers: [] as Teacher[],
  form: {
    registerNumber: null,
    name: "",
    gender: 0,
    placeOfBorn: "",
    dateOfBorn: { year: 0, month: 0, day: 0, dayOfWeek: 0 },
    email: "",
    description: "",
    userId: "",
  },
});

// Fungsi untuk mengambil data guru/ (GET)
const getData = async () => {
  try {
    const response = await TeacherService.get();
    if (response.isSuccess) {
      data.teachers = response.data as Teacher[];
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

// Fungsi untuk menghapus data mahasiswa (DELETE)
const deleteData = async (teacher: Teacher) => {
  try {
    const response = await TeacherService.delete(teacher.id);
    if (response.isSuccess) {
      ToastService.successToast("Data berhasil dihapus.");
      const index = data.teachers.indexOf(teacher);
      data.teachers.splice(index, 1);
    }
  } catch (error) {
    console.error("Error deleting data:", error);
  }
};

const confirmDelete = (teacher: Teacher) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus ${teacher.name} ?`,
    teacher,
    "danger",
    3000,
    "Batal",
    "Hapus"
  ).then(() => {
    deleteData(teacher);
  });
};

const setAdminConfirm = (teacher: Teacher) => {
  DialogService.showDialog(
    `Apakah Anda yakin akan menjadikan ${teacher.name} sebagai admin ?`,
    teacher,
    "warning",
    3000,
    "Batal",
    "Ya"
  ).then(() => {
    setAdmin(teacher);
  });
};


const setAdmin = async (teacher: Teacher) => {
  const response = await AuthService.setAdmin(teacher.userId);
  if (response.isSuccess) {
    ToastService.successToast(`${teacher.name} telah menjadi admin`);
  }
};


onMounted(getData);
</script>
