<template>
  <div>
    <PageHeader title="Catatan Perkembangan Siswa">
      <div class="flex justify-between items-center">
        <h1 class="text-2xl font-semibold"></h1>
        <div class="flex flex-column justify-center items-center ">
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
    <fwb-table>
      <fwb-table-head>
        <fwb-table-head-cell>No</fwb-table-head-cell>
        <fwb-table-head-cell>Jenis</fwb-table-head-cell>
        <fwb-table-head-cell>Catatan</fwb-table-head-cell>
        <fwb-table-head-cell>Pemberi Catatan</fwb-table-head-cell>
        <fwb-table-head-cell>TA</fwb-table-head-cell>
        <fwb-table-head-cell>Tanggal</fwb-table-head-cell>
        <fwb-table-head-cell>Update</fwb-table-head-cell>
        <fwb-table-head-cell></fwb-table-head-cell>
      </fwb-table-head>
      <fwb-table-body>
        <fwb-table-row v-for="(item, index) in datas" :key="item.id">
          <fwb-table-cell>
            {{ index + 1 }}
          </fwb-table-cell>
          <fwb-table-cell>
            <span :class="getStudentProgressNoteTypeClass(item.progressType)">
              {{ Helper.studentProgressNoteType(item.progressType) }}
            </span>
          </fwb-table-cell>
          <fwb-table-cell>{{ item.note.length > 50 ? item.note.slice(0, 50) + '...' : item.note }}</fwb-table-cell>
          <fwb-table-cell>{{ item.teacherName }}</fwb-table-cell>

          <fwb-table-cell>{{ item.schoolYearName }}</fwb-table-cell>
          <fwb-table-cell>{{ item.createdAt ? DateTime.fromJSDate(new Date(item.createdAt))
            .toFormat("dd-MM-yyyy HH:mm:ss") :
            "-" }}</fwb-table-cell>
          <fwb-table-cell>{{ item.updatedAt ? DateTime.fromJSDate(new Date(item.updatedAt))
            .toFormat("dd-MM-yyyy HH:mm:ss") :
            "-" }}</fwb-table-cell>
          <fwb-table-cell>
            <button @click="edit(item)">
              <EditIcon class="w-5 h-5"></EditIcon>
            </button>
          </fwb-table-cell>

        </fwb-table-row>
      </fwb-table-body>
    </fwb-table>
    <form @submit.prevent="addStudentProgress">
      <fwb-modal v-if="showModal" :size="'lg'" not-escapable persistent
        class="no-print modal modal-open opacity-[99%] z-50" @close="showModal = false">
        <template #header>
          <fwb-heading tag="h5">Tambah Perkembangan Siswa</fwb-heading>
        </template>
        <template #body>
          <div class="form-control">
            <fwb-input :disabled="form.id > 0" v-model="form.createdAt" type="datetime-local" label="Tanggal" />
          </div>
          <div class="form-control">
            <FwbSelect v-model="form.progressType" label="Jenis Perkembangan"
              :options="Helper.studentProgressNoteTypes" />
          </div>
          <div class="form-control mt-5">
            <FwbTextarea class="min-h-[350px]" :disabled="form.id > 0" v-model="form.note" type="text"
              label="Catatan Perkembangan" />
          </div>

        </template>
        <template #footer>
          <div class="flex justify-end gap-2">
            <fwb-button :color="'alternative'" type="button" @click="showModal = false">Cancel</fwb-button>
            <fwb-button :color="'green'" type="submit">Simpan</fwb-button>
          </div>
        </template>

      </fwb-modal>
    </form>
  </div>
</template>

<script setup lang="ts">
import { Helper, type ErrorResponse, type RequestResponse } from '@/commons';
import { ClassRoomService, ToastService } from '@/services';
import { ref } from 'vue';
import PageHeader from '@/components/PageHeader.vue'
import { DateTime } from 'luxon';
import {
  FwbTable,
  FwbTableCell,
  FwbTableRow,
  FwbTableBody,
  FwbTableHeadCell,
  FwbTableHead,
  FwbButton, FwbModal, FwbHeading, FwbInput,
  FwbTextarea,
  FwbSelect

} from 'flowbite-vue'
import _ from 'lodash';
import StudentProgressNoteService from '@/services/StudentProgressNoteService';
import type StudentProgressNoteResponse from '@/models/Responses/StudentProgressNoteResponse';
import AddIcon from '@/components/icons/AddIcon.vue';
import { PrinterIcon } from '@heroicons/vue/24/solid';
import type StudentProgressNoteRequest from '@/models/Requests/StudentProgressNoteRequest';
import AuthService from '@/services/AuthService';
import EditIcon from '@/components/icons/EditIcon.vue';


const props = defineProps({
  studentId: {
    type: Number,
    required: true,
  },
  classroomId: {
    type: Number,
    required: true,
  },
});

const showModal = ref(false)
const showPrint = ref(false)

const datas = ref<StudentProgressNoteResponse[]>([])

const teacherId = AuthService.getTeacherId();
const canAdd = ref(false);

const form = ref<StudentProgressNoteRequest>({ id: 0, teacherId: teacherId, studentId: props.studentId } as StudentProgressNoteRequest)




StudentProgressNoteService.getByStudentId(props.studentId).then((response: RequestResponse) => {
  if (response.isSuccess) {
    const result = response.data as StudentProgressNoteResponse[];
    datas.value = result.reverse()

    if (!teacherId)
      return;

    ClassRoomService.getById(props.classroomId).then((response: RequestResponse) => {
      if (response.isSuccess) {
        const classroom = response.data as {
          homeRoomTeacherId: number
          classRoomId: number
        }
        if (classroom.homeRoomTeacherId == teacherId) {
          canAdd.value = true;
        }
      }
    })

  }
})

const edit = (item: StudentProgressNoteResponse) => {
  form.value.id = item.id
  form.value.note = item.note
  form.value.createdAt = DateTime.fromJSDate(new Date(item.createdAt)).toFormat('yyyy-MM-dd HH:mm:ss')
  form.value.updatedAt = new Date(item.createdAt)
  form.value.studentId = item.studentId
  showModal.value = true
}


const addStudentProgress = async () => {
  try {

    if (form.value.id <= 0) {
      const response: RequestResponse = await StudentProgressNoteService.create(form.value as StudentProgressNoteRequest);
      if (response.isSuccess) {
        datas.value.push(response.data as StudentProgressNoteResponse);
        showModal.value = false
        ToastService.successToast('Data berhasil ditambahkan')
      } else {
        ToastService.dangerToast('Data gagal ditambahkan')
      }
    } else {
      const response: RequestResponse = await StudentProgressNoteService.put(form.value.id, form.value as StudentProgressNoteRequest);
      if (response.isSuccess) {

        const index = _.findIndex(datas.value, { id: form.value.id })
        const item = datas.value[index];
        item.note = form.value.note
        item.updatedAt = new Date();
        showModal.value = false
        ToastService.successToast('Data berhasil ubah')
      } else {

        ToastService.dangerToast(Helper.readDetailError(response.error!))
      }
    }
  } catch (error) {
    const errorResult = error as ErrorResponse;
    ToastService.dangerToast(errorResult.message)
  }
}


const getStudentProgressNoteTypeClass = (progressType: number) => {

  const className = 'text-white rounded-xl px-3 py-1 ';

  switch (progressType) {
    case 0:

      return className + 'bg-blue-400';

    case 1:

      return className + 'bg-teal-400';

    case 2:

      return className + 'bg-red-400';

    case 3:

      return className + 'bg-amber-400';

    default:
      break;
  }

}

</script>

<style scoped></style>
