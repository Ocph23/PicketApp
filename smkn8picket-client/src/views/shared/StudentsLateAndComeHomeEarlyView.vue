<template>
  <PageHeader title="Siswa Pulang Lebih Cepat" class="mt-5">
    <button v-if="props.canAdd">
      <AddIcon @click="createNew" class="w-7 h-7" />
    </button>
  </PageHeader>
  <fwb-table>
    <fwb-table-head>
      <fwb-table-head-cell>No</fwb-table-head-cell>
      <fwb-table-head-cell>Waktu</fwb-table-head-cell>
      <fwb-table-head-cell>Nama</fwb-table-head-cell>
      <fwb-table-head-cell>Kelas/Jurusan</fwb-table-head-cell>
      <fwb-table-head-cell>Status</fwb-table-head-cell>
      <fwb-table-head-cell>Catatan/Alasan</fwb-table-head-cell>
    </fwb-table-head>
    <fwb-table-body>
      <fwb-table-row v-for="(absen, index) in datas" :key="absen.studentId">
        <fwb-table-cell>
          {{ index + 1 }}
        </fwb-table-cell>
        <fwb-table-cell>{{ absen.time }}</fwb-table-cell>
        <fwb-table-cell>{{ absen.studentName }}</fwb-table-cell>
        <fwb-table-cell>{{ absen.classRoomName }}-{{ absen.departmentName }}</fwb-table-cell>
        <fwb-table-cell>{{ Helper.getAttendanceStatus(absen.attendanceStatus) }}</fwb-table-cell>
        <fwb-table-cell>{{ absen.description }}</fwb-table-cell>
        <fwb-table-cell>
          <button @click="confirmDelete(absen)" class="text-white flex">
            <DeleteIcon />
          </button>
        </fwb-table-cell>
      </fwb-table-row>
    </fwb-table-body>
  </fwb-table>

  <FwbModal class="modal opacity-[99%]" v-if="modal" :size="'3xl'" :persistent="true">
    <template #header>
      <FwbHeading tag="h3" class="text-lg pb-3 font-bold">
        Tambah Siswa Pulang Lebih Cepat
      </FwbHeading>
    </template>

    <template #body>
      <form class="flex flex-col gap-3" @submit.prevent="save(model)">
        <VTInput v-model="model.AtTime" label="Waktu" :type="'time'" required></VTInput>
        <div class="form-control">
          <AutoComplete placeholder="cari siswa" label="Nama Siswa" :service="'student'" v-model="studentSelected">
          </AutoComplete>
        </div>

        <FwbSelect v-model="attendanceStatusSelect" label="Status" :options="attendanceStatus" required></FwbSelect>

        <FwbTextarea v-model="model.Description" label="Catatan/Alasan" required></FwbTextarea>
        <div class="flex justify-end gap-2">
          <FwbButton :color="'alternative'" @click="modal = false">Batal</FwbButton>
          <FwbButton type="submit">Simpan</FwbButton>
        </div>
      </form>
    </template>
  </FwbModal>


</template>
<script setup lang="ts">

import PageHeader from '@/components/PageHeader.vue';

import { FwbTable, FwbSelect, FwbTableRow, FwbInput, FwbTextarea, FwbButton, FwbHeading, FwbTableHead, FwbTableBody, FwbTableCell, FwbTableHeadCell, FwbModal } from 'flowbite-vue'

import { Helper } from '@/commons'
import { ref } from 'vue';
import type LateAndComeHomeEarlyRequest from '@/models/LateAndComeHomeEarly';
import AutoComplete from '@/components/AutoComplete.vue';
import { DialogService, PicketService, ToastService } from '@/services';
import AddIcon from '@/components/icons/AddIcon.vue';
import type { LateAndComeHomeEarlyResponse } from '@/models/LateAndComeHomeEarly';
import type { Student } from '@/models';
import DeleteIcon from '@/components/icons/DeleteIcon.vue';
import { DateTime } from 'luxon';
import VTInput from '@/components/VTInput/VTInput.vue';

const props = defineProps({ data: Array<LateAndComeHomeEarlyResponse>, canAdd: Boolean })
const datas = props.data as LateAndComeHomeEarlyResponse[];
const modal = ref(false);
const model = ref<LateAndComeHomeEarlyRequest>({} as LateAndComeHomeEarlyRequest);
const studentSelected = ref({} as Student);
const attendanceStatusSelect = ref('1');


const attendanceStatus = [
  { value: '4', name: 'Izin' },
  { value: '5', name: 'Sakit' },
  { value: '6', name: 'Bolos' },
  { value: '7', name: 'Lainnya' },
] as { value: string; name: string }[];




const createNew = () => {
  model.value = {} as LateAndComeHomeEarlyRequest;
  model.value.LateAndGoHomeEarlyStatus = 1;
  model.value.AtTime = DateTime.fromJSDate(new Date()).toFormat("HH:mm");
  modal.value = true;
}

const save = async (request: LateAndComeHomeEarlyRequest) => {
  request.AttendanceStatus = Number(attendanceStatusSelect.value);
  request.StudentId = studentSelected.value.id;
  const response = await PicketService.addLateOrComeHomeEarly(request);
  if (response.isSuccess) {
    const data = response.data as LateAndComeHomeEarlyResponse;
    datas?.push(data);
    modal.value = false;
    ToastService.successToast("Berhasil Menambahkan Catatan Kejadian");
  } else {
    ToastService.dangerToast("Gagal Menambahkan Catatan Kejadian")
  }
}


const confirmDelete = (attendance: LateAndComeHomeEarlyResponse) => {
  DialogService.showDialog("Yakin hapus data  ? ", attendance, 'danger').then(async () => {
    const response = await PicketService.deleteLateOrComeHomeEarly(attendance.id);
    if (response.isSuccess) {
      const i = datas?.indexOf(attendance);
      if (i !== undefined && i !== -1) {
        datas?.splice(i, 1);
      }
      ToastService.successToast("Berhasil Menghapus Data ")
    } else {
      ToastService.dangerToast("Gagal Menghapus Data ")
    }
  })
}

</script>
<style lang=""></style>
