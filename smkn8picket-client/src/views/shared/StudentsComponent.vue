<template>
  <div>
    <PageHeader title="Data Siswa">
      <router-link :to="{ name: 'addSiswa' }">
        <AddIcon class="w-7 h-7" />
      </router-link>
    </PageHeader>
    <div class="shadow-md sm:rounded-lg mt-1">
      <div class="p-5 flex sm:flex-row flex-col items-center sm:justify-between">
        <fwb-select :options="data.pageSizes" placeholder="per halaman" v-model="data.paginate.pageSize"
          @change="changePageSize">
          <template #prefix>
            <svg aria-hidden="true" class="w-5 h-5 text-gray-500 dark:text-gray-400" fill="none" stroke="currentColor"
              viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round"
                stroke-width="2" />
            </svg>
          </template>
        </fwb-select>
        <fwb-input v-model="data.paginate.searchTerm" placeholder="cari siswa..." @change="searchTextChange">
          <template #prefix>
            <svg aria-hidden="true" class="w-5 h-5 text-gray-500 dark:text-gray-400" fill="none" stroke="currentColor"
              viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round"
                stroke-width="2" />
            </svg>
          </template>
        </fwb-input>
      </div>
      <fwb-table class="w-full p-5">
        <fwb-table-head>
          <fwb-table-head-cell>No</fwb-table-head-cell>
          <fwb-table-head-cell>Nama</fwb-table-head-cell>
          <fwb-table-head-cell>NIS/NISN</fwb-table-head-cell>
          <fwb-table-head-cell class="w-5">Kelamin</fwb-table-head-cell>
          <fwb-table-head-cell>Tempat, Tanggal Lahir</fwb-table-head-cell>
          <fwb-table-head-cell>Hp Orang Tua</fwb-table-head-cell>
          <fwb-table-head-cell>Status</fwb-table-head-cell>
          <fwb-table-head-cell>
            <span class="sr-only"></span>
          </fwb-table-head-cell>
        </fwb-table-head>
        <fwb-table-body>
          <fwb-table-row v-for="(student, index) in paginateState.paginateResult.data as Student[]" :key="student.id">
            <fwb-table-cell class="w-[20px]">{{ index + 1 }}</fwb-table-cell>
            <fwb-table-cell class="flex">
              <img class="w-8 h-8 rounded-full" :src="Helper.getStudentAvatar(student.photo)" />
              <span class="ml-2">{{ student.name }}</span></fwb-table-cell>
            <fwb-table-cell>
              {{ student.nis }} <span v-if="student.nisn"> / {{ student.nisn }}</span>
            </fwb-table-cell>
            <fwb-table-cell>
              {{ student.gender === 0 ? 'Laki-laki' : 'Perempuan' }}</fwb-table-cell>
            <fwb-table-cell>{{ student.placeOfBorn }},
              {{ student.dateOfBorn == null ? '' : DateTime.fromJSDate(new
                Date(student.dateOfBorn)).toFormat('dd-MM-yyyy')
              }}</fwb-table-cell>
            <fwb-table-cell>{{ student.parentPhoneNumber }}</fwb-table-cell>
            <fwb-table-cell>{{ Helper.studentStatus(student.status) }}</fwb-table-cell>
            <fwb-table-cell class="flex gap-1">
              <router-link :to="isPiket ? `/piket/siswa/${student.id}/detail` : `/admin/siswa/${student.id}/detail`">

                <DetailIcon></DetailIcon>
              </router-link>
              <router-link :to="`/admin/siswa/${student.id}/edit`" v-if="!isPiket">
                <EditIcon></EditIcon>
              </router-link>
              <button @click="confirmDelete(student)" class="text-white flex" v-if="!isPiket">
                <DeleteIcon />
              </button>
            </fwb-table-cell>
          </fwb-table-row>
          <fwb-table-row>
            <fwb-table-cell colspan="8" class="!px-0 !py-0">
              <PaginationView v-if="paginateState.paginateResult" :paginate="data.paginate" @onChangePage="getData">
              </PaginationView>
            </fwb-table-cell>
          </fwb-table-row>
        </fwb-table-body>
      </fwb-table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { DialogService, ToastService, StudentService } from '@/services'
import { Helper } from '@/commons'
import { AddIcon, EditIcon, DeleteIcon } from '@/components/icons'
import PaginationView from '@/components/PaginationView.vue'
import PaginationStore from '@/stores/PaginationStore'
import PageHeader from '@/components/PageHeader.vue'
import { type Pagination, type Student } from '@/models'

import {
  FwbInput,
  FwbSelect,
  FwbTable,
  FwbTableCell,
  FwbTableRow,
  FwbTableBody,
  FwbTableHeadCell,
  FwbTableHead,
} from 'flowbite-vue'

import type { PaginateResponse } from '@/models/Responses'
import { DateTime } from 'luxon'
import DetailIcon from '@/components/icons/DetailIcon.vue'
import AuthService from '@/services/AuthService'

const paginateState = PaginationStore()

const data = reactive({
  paginateResult: {} as PaginateResponse,
  students: [] as Student[],
  pageSizes: [
    { value: '10', name: '10' },
    { value: '20', name: '20' },
    { value: '50', name: '50' },
    { value: '100', name: '100' },
  ],
  paginate: {
    page: 1,
    pageSize: '10',
    searchTerm: '',
    sortOrder: 'asc',
    columnOrder: 'date',
  } as Pagination,
})

const isPiket = ref(false)
AuthService.isPiket().then((x) => {
  if (x) {
    isPiket.value = x as boolean
  }
})


// Fungsi untuk mengambil data siswa (GET)p
const getData = async (paginate: Pagination) => {
  const result = await StudentService.Pageninate(paginate)
  if (result.isSuccess) {
    data.paginateResult = result.data as PaginateResponse
    paginateState.setPaginateResult(result.data as PaginateResponse)
  }
}

// Fungsi untuk menghapus data siswa (DELETE)
const deleteData = async (student: Student) => {
  try {
    const response = await StudentService.delete(student.id)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil dihapus.')
      const index = data.students.indexOf(student)
      data.students.splice(index, 1)
    }
  } catch (error) {
    console.error('Error deleting data:', error)
  }
}

const confirmDelete = (student: Student) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus ${student.name} ?`,
    student,
    'danger',
    3000,
    'Batal',
    'Hapus',
  ).then(() => {
    deleteData(student)
  })
}

const changePageSize = () => {
  paginateState.setPaginateResult({} as PaginateResponse)
  data.paginate.page = 1
  getData(data.paginate)
}

const searchTextChange = () => {
  data.paginate.page = 1
  getData(data.paginate)
}

onMounted(() => {
  paginateState.setPaginateResult({} as PaginateResponse)
  getData(data.paginate)
})
</script>
