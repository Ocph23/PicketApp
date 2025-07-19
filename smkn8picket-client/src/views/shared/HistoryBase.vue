<template>
  <div class="no-print">
    <PageHeader title="Riwayat Pikets">
      <router-link :to="{ name: 'addSiswa' }">
        <AddIcon class="w-7 h-7" />
      </router-link>
    </PageHeader>
    <div class="my-2 flex sm:flex-row flex-col items-center sm:justify-between">
      <fwb-select :options="data.pageSizes" placeholder="per halaman" v-model="data.paginate.pageSize"
        @change="changePageSize">
        <template #prefix>
          <svg aria-hidden="true" class="w-5 h-5 pr-10 text-gray-500 dark:text-gray-400" fill="none"
            stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round"
              stroke-width="2" />
          </svg>
        </template>
      </fwb-select>

      <fwb-input v-model="data.paginate.searchTerm" placeholder="cari siswa..." @change="getData(data.paginate)">
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
        <fwb-table-head-cell>Tanggal</fwb-table-head-cell>
        <fwb-table-head-cell>Cuaca</fwb-table-head-cell>
        <fwb-table-head-cell>Jam Mulai</fwb-table-head-cell>
        <fwb-table-head-cell>Jam Selesai</fwb-table-head-cell>
        <fwb-table-head-cell>Dibuka Oleh</fwb-table-head-cell>
        <fwb-table-head-cell>
          <span class="sr-only"></span>
        </fwb-table-head-cell>
      </fwb-table-head>
      <fwb-table-body>
        <fwb-table-row v-for="(department, index) in data.paginateResult.data as Picket[]" :key="index">
          <fwb-table-cell class="w-[20px]">{{ index + 1 }}</fwb-table-cell>
          <fwb-table-cell>
            {{ Helper.getIndonesiaDay(new Date(department.date).getDay()) }},
            {{ Helper.getDateTimeString(new Date(department.date), 'dd-MM-yyyy') }}
          </fwb-table-cell>
          <fwb-table-cell>
            {{ Helper.getWeartherString(department.weather) }}
          </fwb-table-cell>
          <fwb-table-cell>{{ department.startAt }}</fwb-table-cell>
          <fwb-table-cell>{{ department.endAt }}</fwb-table-cell>
          <fwb-table-cell>{{ department.createdName }}</fwb-table-cell>
          <fwb-table-cell>
            <router-link :to="isPiket ? `/piket/history/${department.id}` : `/admin/history/${department.id}`">
              <button class="text-white flex">
                <DetailIcon />
              </button>
            </router-link>
          </fwb-table-cell>
        </fwb-table-row>
        <fwb-table-row>
          <fwb-table-cell colspan="7" class="!px-0 !py-0">
            <PaginationView v-if="paginateState.paginateResult" :paginate="data.paginate" @onChangePage="getData">
            </PaginationView>
          </fwb-table-cell>
        </fwb-table-row>
      </fwb-table-body>
    </fwb-table>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { PicketService } from '@/services'
import { Helper } from '@/commons'
import { DetailIcon } from '@/components/icons'
import type { Pagination, Picket } from '@/models'
import type { PaginateResponse } from '@/models/Responses'
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
import PaginationView from '@/components/PaginationView.vue'
import PaginationStore from '@/stores/PaginationStore'
import PageHeader from '@/components/PageHeader.vue'
import AuthService from '@/services/AuthService'


const paginateState = PaginationStore()

const data = reactive({
  pageSizes: [
    { value: '10', name: '10' },
    { value: '20', name: '20' },
    { value: '50', name: '50' },
    { value: '100', name: '100' },
  ],
  paginateResult: { data: [] as Picket[], totalRecords: 0 } as PaginateResponse,
  paginate: {
    page: 1,
    pageSize: '10',
    searchTerm: '',
    sortOrder: 'desc',
    columnOrder: 'date',
  } as Pagination,
})

const isPiket = ref(false)

AuthService.isPiket().then((x) => {
  isPiket.value = x as boolean
})


const getData = async (paginate: Pagination) => {
  const result = await PicketService.Pageninate(paginate)
  if (result.isSuccess) {
    data.paginateResult = result.data as PaginateResponse
    pageNumbers.value = getPageNumbers()
  }
}

const changePageSize = () => {
  getData(data.paginate)
}

getData(data.paginate)

///pagehinaton

const totalPages = () => {
  return Math.ceil(data.paginateResult.totalRecords / Number(data.paginate.pageSize))
}

const pageNumbers = ref()

const getPageNumbers = () => {
  let startPage = data.paginate.page - 1
  let endPage = data.paginate.page + 1

  if (data.paginate.page === 1) {
    startPage = 1
    endPage = 3
  } else if (data.paginate.page === totalPages()) {
    startPage = totalPages() - 2
    endPage = totalPages()
  }

  // Ensure that page numbers are within valid range
  startPage = Math.max(startPage, 1)
  endPage = Math.min(endPage, totalPages())

  const pages = []
  for (let i = startPage; i <= endPage; i++) {
    pages.push(i)
  }
  return pages
}

// const goToPage = (page: number) => {
//   if (page < 1 || page > totalPages()) return
//   data.paginate.page = page
//   getData(data.paginate)
// }
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
