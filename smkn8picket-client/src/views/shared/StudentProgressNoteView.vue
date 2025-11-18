<template>
  <div>
    <PageHeader title="Catatan Perkembangan Siswa">
      <div class="flex justify-between items-center">
        <h1 class="text-2xl font-semibold"></h1>
        <div class="flex flex-column justify-center items-center">
          <button @click="showModal = true" v-if="canAdd"
            class="transition rounded text-black m-3 mr-5 w-6 items-center text-center">
            <AddIcon class="w-7 h-7" />
          </button>
          <FwbButton v-if="showPrint" :color="'yellow'" type="submit"
            class="flex flex-row items-center justify-center p-2">
            <PrinterIcon class="w-3 h-3"></PrinterIcon>
          </FwbButton>
        </div>
      </div>
    </PageHeader>

    <VTTableNew ref="vtTable" :method="'All'" :columns="columns" :source="dataTable">
      <template #nomor="item">
        {{ item.index + 1 }}
      </template>
      <template #progressType="item">
        <span :class="getStudentProgressNoteTypeClass(item.data.progressType)">
          {{ Helper.studentProgressNoteType(item.data.progressType) }}
        </span>
      </template>
      <template #update="item">
        {{
          item.data.updatedAt
            ? DateTime.fromJSDate(new Date(item.data.updatedAt)).toFormat('dd-MM-yyyy HH:mm:ss')
            : '-'
        }}
      </template>
      <template #tanggal="item">
        {{
          item.data.createdAt
            ? DateTime.fromJSDate(new Date(item.data.createdAt)).toFormat('dd-MM-yyyy HH:mm:ss')
            : '-'
        }}
      </template>
      <template #actions="item">
        <div class="flex">
          <VTButtonAction :style="'warning'" :type="'edit'" @click="edit(item.data)"> </VTButtonAction>
        </div>
      </template>
    </VTTableNew>

    <form @submit.prevent="addStudentProgress">
      <VTModal v-if="showModal" :size="'lg'" not-escapable persistent
        class="no-print modal modal-open opacity-[99%] z-50" @close="showModal = false">
        <template #header>
          <fwb-heading tag="h5">Tambah Perkembangan Siswa</fwb-heading>
        </template>
        <template #body>
          <div class="mb-5">
            <VTInput :disabled="form.id > 0" v-model="form.createdAt" type="datetime-local" label="Tanggal" />
          </div>
          <div class="mb-5">
            <VTSelect v-model="form.progressType" label="Jenis Perkembangan" placeholder="Pilih Jenis Perkembangan"
              :options="dataStudentProgressNoteTypes" />
          </div>
          <div class="mb-5">
            <VTTextArea :disabled="form.id > 0" v-model="form.note" type="text"
              placeholder="Masukkan Catatan Perkembangan" label="Catatan Perkembangan" />
          </div>
        </template>
        <template #footer>
          <div class="flex justify-end gap-2">
            <VTButton :color="'alternative'" type="button" @click="showModal = false">Cancel</VTButton>
            <VTButtonSave type="submit">Simpan</VTButtonSave>
          </div>
        </template>
      </VTModal>
    </form>
  </div>
</template>

<script setup lang="ts">
import { Helper, type ErrorResponse, type RequestResponse } from '@/commons'
import { ClassRoomService } from '@/services'
import { ref } from 'vue'
import PageHeader from '@/components/PageHeader.vue'
import { DateTime } from 'luxon'
import _ from 'lodash'
import StudentProgressNoteService from '@/services/StudentProgressNoteService'
import type StudentProgressNoteResponse from '@/models/Responses/StudentProgressNoteResponse'
import AddIcon from '@/components/icons/AddIcon.vue'
import { PrinterIcon } from '@heroicons/vue/24/solid'
import type StudentProgressNoteRequest from '@/models/Requests/StudentProgressNoteRequest'
import AuthService from '@/services/AuthService'
import {
  VTSelect,
  VTInput,
  VTModal,
  VTButtonSave,
  VTButton,
  VTTextArea,
  VTTableNew,
  VTButtonAction,
  VTToastService,
} from '@ocph23/vtocph23'
import type { VTTableColumn, VTTableSource } from '@ocph23/vtocph23/components/VTTable/index.js'

const props = defineProps({
  studentId: {
    type: Number,
    required: true,
  },
  classroomId: {
    type: Number,
    required: true,
  },
})
const vtTable = ref<InstanceType<typeof VTTableNew> | null>(null)
const dataTable = ref<VTTableSource<StudentProgressNoteResponse>>({
  data: [],
  totalRecords: 0,
  paginate: {
    currentPage: 1,
    pageSize: 10,
    sortOrder: 'asc',
    columnOrder: '',
    searchTerm: '',
  },
})

const columns = [
  {
    title: 'No',
    name: 'nomor',
    type: 'Custome',
    headerClass: 'w-5'
  },
  {
    title: 'Jenis',
    name: 'progressType',
    type: 'Custome',
  },
  {
    title: 'Catatan',
    propName: 'note',
  },
  {
    title: 'Pemberi Catatan',
    propName: 'teacherName',
  },
  {
    title: 'Tahun Ajaran',
    propName: 'schoolYearName',
  },
  {
    title: 'Tanggal',
    name: 'tanggal',
    type: 'Custome',
  },
  {
    title: 'Update',
    name: 'update',
    type: 'Custome',
  },
  {
    title: 'Aksi',
    name: 'actions',
    type: 'Custome',
    headerClass: 'w-10',
  },
] as VTTableColumn[]

const showModal = ref(false)
const showPrint = ref(false)

const datas = ref<StudentProgressNoteResponse[]>([])

const dataStudentProgressNoteTypes: { name: string; value: number }[] =
  Helper.studentProgressNoteTypes

const teacherId = AuthService.getTeacherId()
const canAdd = ref(false)

const form = ref<StudentProgressNoteRequest>({
  id: 0,
  teacherId: teacherId,
  studentId: props.studentId,
} as StudentProgressNoteRequest)

StudentProgressNoteService.getByStudentId(props.studentId).then((response: RequestResponse) => {
  if (response.isSuccess) {
    const result = response.data as StudentProgressNoteResponse[]
    datas.value = result.reverse()
    dataTable.value.data = result.reverse()
    vtTable.value?.refresh()
    if (!teacherId) return

    ClassRoomService.getById(props.classroomId).then((response: RequestResponse) => {
      if (response.isSuccess) {
        const classroom = response.data as {
          homeRoomTeacherId: number
          classRoomId: number
        }
        if (classroom.homeRoomTeacherId == teacherId) {
          canAdd.value = true
        }
      }
    })
  }
})

const edit = (item: StudentProgressNoteResponse) => {
  form.value.id = item.id
  form.value.note = item.note
  form.value.createdAt = DateTime.fromJSDate(new Date(item.createdAt)).toFormat(
    'yyyy-MM-dd HH:mm:ss',
  )
  form.value.updatedAt = new Date(item.createdAt)
  form.value.studentId = item.studentId
  showModal.value = true
}

const addStudentProgress = async () => {
  try {
    if (form.value.id <= 0) {
      const response: RequestResponse = await StudentProgressNoteService.create(
        form.value as StudentProgressNoteRequest,
      )
      if (response.isSuccess) {
        datas.value.push(response.data as StudentProgressNoteResponse)
        showModal.value = false
        VTToastService.success('Data berhasil ditambahkan')
      } else {
        VTToastService.error('Data gagal ditambahkan')
      }
    } else {
      const response: RequestResponse = await StudentProgressNoteService.put(
        form.value.id,
        form.value as StudentProgressNoteRequest,
      )
      if (response.isSuccess) {
        const index = _.findIndex(datas.value, { id: form.value.id })
        const item = datas.value[index]
        item.note = form.value.note
        item.updatedAt = new Date()
        showModal.value = false
        VTToastService.success('Data berhasil ubah')
      } else {
        VTToastService.error(Helper.readDetailError(response.error!))
      }
    }
  } catch (error) {
    const errorResult = error as ErrorResponse
    VTToastService.error(errorResult.message)
  }
}

const getStudentProgressNoteTypeClass = (progressType: number) => {
  const className = 'text-white rounded-xl px-3 py-1 '

  switch (progressType) {
    case 0:
      return className + 'bg-blue-400'

    case 1:
      return className + 'bg-teal-400'

    case 2:
      return className + 'bg-red-400'

    case 3:
      return className + 'bg-amber-400'

    default:
      break
  }
}
</script>

<style scoped></style>
