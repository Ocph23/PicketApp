<template>
  <VTCard title="Data Siswa">
    <template #right-side>
      <router-link :to="{ name: 'addSiswa' }">
        <AddIcon class="w-7 h-7" />
      </router-link>
    </template>
    <div class="mt-1">
      <VTTableNew
        :method="'Paginate'"
        :columns="columns"
        :source="dataTable"
        v-on:on-change="onTableChange"
      >
        <template #no="row">
          {{ row.index + 1 }}
        </template>
        <template #student="row">
          <div class="flex items-center gap-1">
            <img class="w-8 h-8 rounded-full" :src="Helper.getStudentAvatar(row.data.photo)" />
            <span class="ml-2">{{ row.data.name }}</span>
          </div>
        </template>
        <template #nis="row">
          {{ row.data.nis }} <span v-if="row.data.nisn"> / {{ row.data.nisn }}</span>
        </template>
        <template #gender="row">
          {{ Helper.genders.find((g) => g.value === row.data.gender)?.name }}
        </template>
        <template #status="row">
          {{ Helper.studentStatus(row.data.status) }}
        </template>
        <template #ttl="row">
          {{ row.data.placeOfBorn }},
          {{
            row.data.dateOfBorn == null
              ? ''
              : DateTime.fromJSDate(new Date(row.data.dateOfBorn)).toFormat('dd-MM-yyyy')
          }}
        </template>
        <template #actions="row">
          <div class="flex gap-1">
            <router-link
              :to="
                isPiket
                  ? `/piket/siswa/${row.data.id}/detail`
                  : `/admin/siswa/${row.data.id}/detail`
              "
            >
              <DetailIcon></DetailIcon>
            </router-link>
            <router-link :to="`/admin/siswa/${row.data.id}/edit`" v-if="!isPiket">
              <EditIcon></EditIcon>
            </router-link>
            <button @click="confirmDelete(row.data)" class="text-white flex" v-if="!isPiket">
              <DeleteIcon />
            </button>
          </div>
        </template>
      </VTTableNew>
    </div>
  </VTCard>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { DialogService, ToastService, StudentService } from '@/services'
import { Helper } from '@/commons'
import { AddIcon, EditIcon, DeleteIcon } from '@/components/icons'
import PaginationStore from '@/stores/PaginationStore'
import { type Pagination, type Student } from '@/models'

import type { PaginateResponse } from '@/models/Responses'
import { DateTime } from 'luxon'
import DetailIcon from '@/components/icons/DetailIcon.vue'
import AuthService from '@/services/AuthService'
import { VTCard, VTTableNew } from '@ocph23/vtocph23'
import type {
  VTTableColumn,
  VTTablePagination,
  VTTableSource,
} from '@ocph23/vtocph23/components/VTTable/index.js'

const paginateState = PaginationStore()

const dataTable = reactive<VTTableSource>({
  data: [],
  totalRecords: 0,
  paginate: {
    currentPage: 1,
    pageSize: 10,
    searchTerm: '',
    sortOrder: 'asc',
    columnOrder: 'date',
  },
})

const columns = [
  { title: 'No', name: 'no', type: 'Custome', headerClass: 'w-[20px]' },
  { title: 'Nama', name: 'student', propName: 'name', type: 'Custome', isMobileHeader: true },
  { title: 'NIPD/NISN', name: 'nis', type: 'Custome', propName: 'nisn' },
  { title: 'Kelamin', name: 'gender', type: 'Custome', headerClass: 'w-5' },
  { title: 'Tempat, Tanggal Lahir', name: 'ttl', type: 'Custome' },
  { title: 'Hp Orang Tua', propName: 'parentPhoneNumber' },
  { title: 'Status', name: 'status', type: 'Custome' },
  { title: 'Aksi', name: 'actions', type: 'Custome', headerClass: 'w-10' },
] as VTTableColumn[]

const onTableChange = (paginate: VTTablePagination) => {
  dataTable.paginate = paginate
  getData(paginate)
}

const isPiket = ref(false)
AuthService.isPiket().then((x) => {
  if (x) {
    isPiket.value = x as boolean
  }
})

// Fungsi untuk mengambil data siswa (GET)p
const getData = async (vtPaginate: VTTablePagination) => {
  const paginate = {
    page: vtPaginate.currentPage,
    pageSize: String(vtPaginate.pageSize),
    searchTerm: vtPaginate.searchTerm,
    sortOrder: vtPaginate.sortOrder,
    columnOrder: vtPaginate.columnOrder,
  } as Pagination
  const result = await StudentService.Pageninate(paginate)
  if (result.isSuccess) {
    const paginateResult = result.data as PaginateResponse
    dataTable.data = paginateResult.data as Student[]
    dataTable.totalRecords = paginateResult.totalRecords
    paginateState.setPaginateResult(result.data as PaginateResponse)
  }
}

// Fungsi untuk menghapus data siswa (DELETE)
const deleteData = async (student: Student) => {
  try {
    const response = await StudentService.delete(student.id)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil dihapus.')
      const index = dataTable.data.indexOf(student)
      dataTable.data.splice(index, 1)
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

onMounted(() => {
  getData(
    dataTable.paginate ||
      ({
        currentPage: 1,
        pageSize: 5,
        searchTerm: '',
        sortOrder: 'asc',
        columnOrder: 'date',
      } as VTTablePagination),
  )
})
</script>
