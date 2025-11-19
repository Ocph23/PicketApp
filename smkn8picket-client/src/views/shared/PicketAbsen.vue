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

  <VTTableNew ref="vtTable" :source="dataTable" :columns="tableColumns" :hovered="false">
    <template #no="item">
      {{ item.index + 1 }}
    </template>
    <template #classDepartment="item">
      {{ item.data.className }} - {{ item.data.departmentName }}
    </template>
    <template #timeIn="item">
      {{ DateTime.fromJSDate(new Date(item.data.timeIn)).toFormat("HH:mm:ss") }}
    </template>
    <template #timeOut="item">
      {{ item.data.timeOut ? DateTime.fromJSDate(new Date(item.data.timeOut)).toFormat("HH:mm:ss") : "-" }}
    </template>
    <template #status="item">
      {{ Helper.getAttendanceStatus(item.data.status) }}
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
  FwbModal,
  FwbHeading, FwbTextarea, FwbButton
} from 'flowbite-vue'
import { forEach, groupBy } from 'lodash';
import { onMounted, reactive, ref } from 'vue';
import AddIcon from '@/components/icons/AddIcon.vue';
import type StudentAttendanceRequest from '@/models/Requests/StudentAttendanceRequest';
import { StudentAttendanceService } from '@/services';
import DeleteIcon from '@/components/icons/DeleteIcon.vue';
import { DateTime } from 'luxon';
import VTInput from '@/components/VTInput/VTInput.vue';
import { VTDialogService, VTTableNew, VTToastService } from '@ocph23/vtocph23';
import type { VTTableColumn, VTTableSource } from '@ocph23/vtocph23/components/VTTable/index.js';
import AutoComplete from '@/components/AutoComplete.vue';

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

const vtTable = ref<InstanceType<typeof VTTableNew> | null>();

const dataTable = reactive<VTTableSource<StudentAttendance>>({
  totalRecords: 0,
} as VTTableSource<StudentAttendance>);



onMounted(() => {
  dataTable.data = source as StudentAttendance[];
});


const tableColumns = [
  { title: 'No', name: 'no', type: 'Custome', headerClass: 'w-10 text-center', rowClass: 'text-center' },

  { title: 'Nama', propName: 'studentName' },
  { title: 'Kelas/Jurusan', name: 'classDepartment', type: 'Custome' },
  { title: 'Masuk', name: 'timeIn', type: 'Custome' },
  { title: 'Pulang', name: 'timeOut', type: 'Custome' },
  { title: 'Status', name: 'status', type: 'Custome' },
  { title: 'Aksi', name: 'actions', type: 'Custome', headerClass: 'text-center h-5' },
] as VTTableColumn[];



const dataKelas = groupBy(source, item => `${item.className}-${item.departmentName}`);

const dataKelasOptions = [] as { name: string, value: string }[];

forEach(dataKelas, (value, key) => {
  dataKelasOptions.push({ name: key, value: key })
});

const handleClassChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  const x = source?.map(item => {
    if (`${item.className}-${item.departmentName}` === target.value) { return item }
  })

  if (x) {
    dataTable.data = x.filter(item => item !== undefined) as StudentAttendance[]
    vtTable.value?.refresh()
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
    dataTable.data?.push(data);
    modal.value = false;
    VTToastService.success("Berhasil Menambahkan Absen");
  } else {
    VTToastService.error(response.error?.detail ?? 'Gagal Menambahkan Absen');
  }
}


const confirmDelete = (attendance: StudentAttendance) => {
  VTDialogService.asyncShowDialog('Perhatian', "Yakin hapus data absen ini ? ", attendance, 'danger').then(async () => {
    const response = await StudentAttendanceService.delete(attendance.id);
    if (response.isSuccess) {
      const i = dataTable.data?.indexOf(attendance);
      if (i !== undefined && i !== -1) {
        dataTable.data?.splice(i, 1);
      }
      VTToastService.success("Berhasil Menghapus Data Absen")
    } else {
      VTToastService.error("Gagal Menghapus Data Absen")
    }
  })
}
</script>

<style scoped></style>
