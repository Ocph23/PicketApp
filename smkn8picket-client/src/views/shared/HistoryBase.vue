<template>
  <div class="no-print">
    <PageHeader title="Riwayat Pikets">
      <router-link :to="{ name: 'addSiswa' }">
        <AddIcon class="w-7 h-7" />
      </router-link>
    </PageHeader>
    <div class="my-2 flex items-center sm:justify-between"></div>
    <VTTableNew :method="'Paginate'" :columns="columns" :source="dataTable" v-on:on-change="onTableChange">
      <template #nomor="item">
        {{ item.index + 1 }}
      </template>
      <template #tanggal="item">
        {{ DateTime.fromISO(item.data.picket.date).setLocale('id').toFormat('cccc, dd LLLL yyyy') }}
      </template>
      <template #cuaca="item">
        {{ Helper.getWeartherString(item.data.weather) }}
      </template>
      <template #actions="item">
        <router-link :to="isPiket ? `/piket/history/${item.data.id}` : `/admin/history/${item.data.id}`">
          <button class="text-white flex">
            <DetailIcon />
          </button>
        </router-link>
      </template>

    </VTTableNew>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { PicketService } from '@/services'
import { Helper } from '@/commons'
import { DetailIcon } from '@/components/icons'
import type { Pagination } from '@/models'
import type { PaginateResponse } from '@/models/Responses'
import PageHeader from '@/components/PageHeader.vue'
import AuthService from '@/services/AuthService'
import { VTTableNew } from '@ocph23/vtocph23'
import type {
  VTTableColumn,
  VTTablePagination,
  VTTableSource,
} from '@ocph23/vtocph23/components/VTTable/index.js'
import { DateTime } from 'luxon'

const dataTable = ref<VTTableSource<History>>({
  data: [],
  totalRecords: 0,
  paginate: {
    currentPage: 1,
    pageSize: 10,
    searchTerm: '',
    sortOrder: 'asc',
    columnOrder: '',
  } as VTTablePagination,
})

const columns = [
  { title: 'no', name: 'nomor', type: 'Custome' },
  { title: 'Tanggal', name: 'tanggal', type: 'Custome' },
  { title: 'Cuaca', name: 'cuaca', type: 'Custome' },
  { title: 'Jam Mulai', propName: 'startDate' },
  { title: 'Jam Selesai', propName: 'endDate' },
  { title: 'Aksi', name: 'actions', type: 'Custome' },
] as VTTableColumn[]


onMounted(() => {
  getData(dataTable.value.paginate as VTTablePagination)
})

const onTableChange = (paginate: VTTablePagination) => {
  dataTable.value.paginate = paginate
  getData(paginate)
}


const isPiket = ref(false)

AuthService.isPiket().then((x) => {
  isPiket.value = x as boolean
})

const getData = async (vtPaginate: VTTablePagination) => {
  const paginate = {
    page: vtPaginate.currentPage,
    pageSize: String(vtPaginate.pageSize),
    searchTerm: vtPaginate.searchTerm,
    sortOrder: vtPaginate.sortOrder,
    columnOrder: vtPaginate.columnOrder,
  } as Pagination
  const result = await PicketService.Pageninate(paginate)
  if (result.isSuccess) {
    const paginateResult = result.data as PaginateResponse
    dataTable.value.data = paginateResult.data as History[]
    dataTable.value.totalRecords = paginateResult.totalRecords
  }
}
</script>

<style lang="css" scoped>
.active {
  font-weight: bold;
  background-color: #007bff;
  color: white;
}
</style>
