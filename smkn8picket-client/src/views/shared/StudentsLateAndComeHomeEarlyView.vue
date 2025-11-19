<template>
  <PageHeader title="Siswa Pulang Lebih Cepat" class="mt-5">
    <button v-if="props.canAdd">
      <AddIcon @click="createNew" class="w-7 h-7" />
    </button>
  </PageHeader>

  <VTTableNew ref="vtTable" method="All" :source="dataTable" :columns="tableColumns" :hovered="false">
    <template #no="item">
      {{ item.index + 1 }}
    </template>
    <template #time="item">
      {{ item.data.time }}
    </template>
    <template #studentName="item">
      {{ item.data.studentName }}
    </template>
    <template #classDepartment="item">
      {{ item.data.classRoomName }}-{{ item.data.departmentName }}
    </template>
    <template #attendanceStatus="item">
      {{ Helper.getAttendanceStatus(item.data.attendanceStatus) }}
    </template>
    <template #description="item">
      {{ item.data.description }}
    </template>
    <template #actions="item">
      <div class="flex items-center">
        <button @click="confirmDelete(item.data)" class="text-white flex">
          <DeleteIcon />
        </button>
      </div>
    </template>
  </VTTableNew>


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

import { FwbSelect, FwbTextarea, FwbButton, FwbHeading, FwbModal } from 'flowbite-vue'

import { Helper } from '@/commons'
import { onMounted, reactive, ref } from 'vue';
import type LateAndComeHomeEarlyRequest from '@/models/LateAndComeHomeEarly';
import AutoComplete from '@/components/AutoComplete.vue';
import { PicketService } from '@/services';
import AddIcon from '@/components/icons/AddIcon.vue';
import type { LateAndComeHomeEarlyResponse } from '@/models/LateAndComeHomeEarly';
import type { Student } from '@/models';
import DeleteIcon from '@/components/icons/DeleteIcon.vue';
import { DateTime } from 'luxon';
import VTInput from '@/components/VTInput/VTInput.vue';
import { VTDialogService, VTTableNew, VTToastService } from '@ocph23/vtocph23';
import type { VTTableColumn, VTTableSource } from '@ocph23/vtocph23/components/VTTable/index.js';

const props = defineProps({ data: Array<LateAndComeHomeEarlyResponse>, canAdd: Boolean })
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



const vtTable = ref<InstanceType<typeof VTTableNew> | null>();

const dataTable = reactive<VTTableSource<LateAndComeHomeEarlyResponse>>({
  totalRecords: 0,
} as VTTableSource<LateAndComeHomeEarlyResponse>);

onMounted(() => {
  dataTable.data = props.data as LateAndComeHomeEarlyResponse[];
});


const tableColumns = [
  { title: 'No', name: 'no', type: 'Custome', headerClass: 'w-10 text-center', rowClass: 'text-center' },

  { title: 'Waktu', name: 'time', type: 'Custome' },
  { title: 'Nama Siswa', name: 'studentName', type: 'Custome' },
  { title: 'Kelas', name: 'classDepartment', type: 'Custome' },
  { title: 'Status Absen', name: 'attendanceStatus', type: 'Custome' },
  { title: 'Catatan/Alasan', name: 'description', type: 'Custome' },
  { title: 'Aksi', name: 'actions', type: 'Custome', headerClass: 'text-center h-5' },
] as VTTableColumn[];


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
    dataTable.data.push(data);
    modal.value = false;
    VTToastService.success("Berhasil Menambahkan Catatan Kejadian");
  } else {
    VTToastService.error("Gagal Menambahkan Catatan Kejadian")
  }
}


const confirmDelete = (attendance: LateAndComeHomeEarlyResponse) => {
  VTDialogService.asyncShowDialog('Perhatian', "Yakin hapus data  ? ", attendance, 'danger').then(async () => {
    const response = await PicketService.deleteLateOrComeHomeEarly(attendance.id);
    if (response.isSuccess) {
      const i = dataTable.data?.indexOf(attendance);
      if (i !== undefined && i !== -1) {
        dataTable.data?.splice(i, 1);
      }
      VTToastService.success("Berhasil Menghapus Data ")
    } else {
      VTToastService.error("Gagal Menghapus Data ")
    }
  })
}

</script>
<style lang=""></style>
