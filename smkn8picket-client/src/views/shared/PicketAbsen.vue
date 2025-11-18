<template>
  <PageHeader title="Data Absen" class="mt-5">
    <div class="flex justify-between gap-2">
      <fwb-select placeholder="Pilih Kelas" :options="dataKelasOptions"
        @change="handleClassChange($event)"></fwb-select>
      <button v-if="props.canAdd">
        <AddIcon @click="createNew" class="w-7 h-7" />
      </button>
    </div>
  </PageHeader>
  <fwb-table>
    <fwb-table-head>
      <fwb-table-head-cell>No</fwb-table-head-cell>
      <fwb-table-head-cell>Nama</fwb-table-head-cell>
      <fwb-table-head-cell>Kelas/Jurusan</fwb-table-head-cell>
      <fwb-table-head-cell>Masuk</fwb-table-head-cell>
      <fwb-table-head-cell>Pulang</fwb-table-head-cell>
      <fwb-table-head-cell>Status</fwb-table-head-cell>
      <fwb-table-head-cell></fwb-table-head-cell>
    </fwb-table-head>
    <fwb-table-body>
      <fwb-table-row v-for="(absen, index) in datas" :key="absen.id">
        <fwb-table-cell>
          {{ index + 1 }}
        </fwb-table-cell>
        <fwb-table-cell>{{ absen.studentName }}</fwb-table-cell>
        <fwb-table-cell>{{ absen.className }} - {{ absen.departmentName }}</fwb-table-cell>
        <fwb-table-cell>{{ DateTime.fromJSDate(new Date(absen.timeIn)).toFormat("HH:mm:ss") }}</fwb-table-cell>
        <fwb-table-cell>{{ absen.timeOut ? DateTime.fromJSDate(new Date(absen.timeOut)).toFormat("HH:mm:ss") : "-"
        }}</fwb-table-cell>
        <fwb-table-cell>{{ Helper.getAttendanceStatus(absen.status) }}</fwb-table-cell>
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
        Tambah Absen
      </FwbHeading>
    </template>

    <template #body>
      <form class="flex flex-col gap-3" @submit.prevent="save(model)">
        <VTInput v-model="timeIn" label="Waktu" :type="'datetime-local'" required></VTInput>
        <AutoComplete placeholder="cari siswa" label="Nama Siswa" :service="'student'" v-model="studentSelected">
        </AutoComplete>
        <FwbSelect v-model="attendanceStatusSelect" label="Status" :options="attendanceStatus" required></FwbSelect>
        <FwbTextarea v-model="model.description" label="Keterangan"></FwbTextarea>
        <div class="flex justify-end gap-2">
          <FwbButton :color="'alternative'" @click="modal = false">Batal</FwbButton>
          <FwbButton type="submit">Simpan</FwbButton>
        </div>
      </form>
    </template>
  </FwbModal>

</template>

<script setup lang="ts">
import PageHeader from '@/components/PageHeader.vue'
import { Helper } from '@/commons';
import type { Student, StudentAttendance, Picket } from '@/models';

import {
  FwbSelect,
  FwbTable,
  FwbTableCell,
  FwbTableRow,
  FwbTableBody,
  FwbTableHeadCell,
  FwbTableHead,
  FwbModal,
  FwbHeading, FwbTextarea, FwbButton
} from 'flowbite-vue'
import { forEach, groupBy } from 'lodash';
import { ref } from 'vue';
import AddIcon from '@/components/icons/AddIcon.vue';
import AutoComplete from '@/components/AutoComplete.vue';
import type StudentAttendanceRequest from '@/models/Requests/StudentAttendanceRequest';
import { DialogService, StudentAttendanceService } from '@/services';
import DeleteIcon from '@/components/icons/DeleteIcon.vue';
import { DateTime } from 'luxon';
import VTInput from '@/components/VTInput/VTInput.vue';
import { VTToastService } from '@ocph23/vtocph23';

const props = defineProps<{ picket: Picket, canAdd: boolean }>()
const model = ref<StudentAttendanceRequest>({} as StudentAttendanceRequest);
const modal = ref(false);
const studentSelected = ref({} as Student);
const timeIn = ref('');
const attendanceStatusSelect = ref('4');

const source = props.picket?.studentAttendance as StudentAttendance[];

const attendanceStatus = [
  // { value: '3', name: 'Alpha' },
  { value: '4', name: 'Sakit' },
  { value: '5', name: 'Izin' },
  { value: '2', name: 'Terlambat' },
  { value: '1', name: 'Hadir' },
]


const dataKelas = groupBy(source, item => `${item.className}-${item.departmentName}`);

const dataKelasOptions = [] as { name: string, value: string }[];

forEach(dataKelas, (value, key) => {
  dataKelasOptions.push({ name: key, value: key })
});

const datas = ref<StudentAttendance[]>([])
const handleClassChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  const x = source?.map(item => {
    if (`${item.className}-${item.departmentName}` === target.value) { return item }
  })

  if (x) {
    datas.value = x.filter(item => item !== undefined) as StudentAttendance[]
  }

}
const createNew = () => {
  model.value = { description: '', timeIn: DateTime.fromJSDate(new Date()).toFormat("yyyy-MM-dd'T'HH:mm") } as StudentAttendance;
  modal.value = true;
}

const save = async (request: StudentAttendanceRequest) => {
  request.status = Number(attendanceStatusSelect.value);
  request.picketId = props.picket.id;
  request.studentId = studentSelected.value.id;
  request.timeIn = timeIn.value;
  const response = await StudentAttendanceService.create(request);
  if (response.isSuccess) {
    const data = response.data as StudentAttendance;
    datas.value?.push(data);
    modal.value = false;
    VTToastService.success("Berhasil Menambahkan Absen");
  } else {
    VTToastService.error(response.error?.detail ?? 'Gagal Menambahkan Absen');
  }
}


const confirmDelete = (attendance: StudentAttendance) => {
  DialogService.showDialog("Yakin hapus data absen ini ? ", attendance, 'danger').then(async () => {
    const response = await StudentAttendanceService.delete(attendance.id);
    if (response.isSuccess) {
      const i = datas.value?.indexOf(attendance);
      if (i !== undefined && i !== -1) {
        datas.value?.splice(i, 1);
      }
      VTToastService.success("Berhasil Menghapus Data Absen")
    } else {
      VTToastService.error("Gagal Menghapus Data Absen")
    }
  })
}
</script>

<style scoped></style>
