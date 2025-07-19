<template>
  <PageHeader title="Catatan Piket" class="mt-5">
    <button v-if="props.canAdd">
      <AddIcon @click="createNew" class="w-7 h-7" />
    </button>
  </PageHeader>
  <fwb-table>
    <fwb-table-head>
      <fwb-table-head-cell>No</fwb-table-head-cell>
      <fwb-table-head-cell>Waktu</fwb-table-head-cell>
      <fwb-table-head-cell>Judul</fwb-table-head-cell>
      <fwb-table-head-cell>Kejadian</fwb-table-head-cell>
      <fwb-table-head-cell>Dibuat Oleh</fwb-table-head-cell>
      <fwb-table-head-cell></fwb-table-head-cell>
    </fwb-table-head>
    <fwb-table-body>
      <fwb-table-row v-for="(journal, index) in datas" :key="journal.id">
        <fwb-table-cell>
          {{ index + 1 }}
        </fwb-table-cell>
        <fwb-table-cell>{{ Helper.getDateTimeString(new Date(journal.createAt), 'HH:mm:ss') }}</fwb-table-cell>
        <fwb-table-cell>{{ journal.title }}</fwb-table-cell>
        <fwb-table-cell>{{ journal.content }}</fwb-table-cell>
        <fwb-table-cell>{{ journal.teacherName }}</fwb-table-cell>
        <fwb-table-cell>
          <div class="flex items-center" v-if="journal.teacherId === teacherLoginId">
            <button class="text-white flex" @click="edit(journal)">
              <EditIcon />
            </button>
            <button @click="confirmDelete(journal)" class="text-white flex">
              <DeleteIcon />
            </button>
          </div>
        </fwb-table-cell>
      </fwb-table-row>
    </fwb-table-body>
  </fwb-table>

  <FwbModal class="modal opacity-[99%]" v-if="modal" :size="'3xl'">
    <template #header>
      <FwbHeading tag="h3" class="text-lg pb-3 font-bold">
        {{ formTitle }} Catatan Kejadian
      </FwbHeading>
    </template>

    <template #body>
      <form class="flex flex-col gap-3" @submit.prevent="saveJurnal(model)">
        <FwbInput v-model="model.createAt" label="Waktu" :type="'datetime-local'" required></FwbInput>
        <FwbInput v-model="model.title" label="Judul" type="text" required></FwbInput>
        <FwbTextarea v-model="model.content" label="Kejadian" required></FwbTextarea>
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

import {
  FwbTable,
  FwbTableCell,
  FwbTableRow,
  FwbTableBody,
  FwbTableHeadCell,
  FwbTableHead,
  FwbModal, FwbHeading,
  FwbInput,
  FwbTextarea,
  FwbButton
} from 'flowbite-vue'
import { ref } from 'vue';
import AddIcon from '@/components/icons/AddIcon.vue';
import type DailyJournal from '@/models/DailyJournal';
import { DialogService, PicketService, ToastService } from '@/services';
import { EditIcon, DeleteIcon } from '@/components/icons';
import Helper from '@/commons/helper';
import AuthService from '@/services/AuthService';


const props = defineProps({ data: Array<DailyJournal>, canAdd: Boolean })
const datas = ref<DailyJournal[]>();
const model = ref<DailyJournal>({} as DailyJournal);

const formTitle = ref('Tambah');

const modal = ref(false);
datas.value = props.data || [];


const teacherLoginId = AuthService.getTeacherId();


if (teacherLoginId) {
  model.value.teacherId = teacherLoginId;
}


const saveJurnal = async (journal: DailyJournal) => {
  if (journal.id) {
    const response = await PicketService.putJournal(journal.id, journal);
    if (response.isSuccess) {
      modal.value = false;
      ToastService.successToast("Berhasil mengubah Catatan Kejadian");
    } else {
      ToastService.dangerToast("Gagal Mengubah Catatan Kejadian")
    }
  } else {
    const response = await PicketService.postJournal(journal);
    if (response.isSuccess) {
      const data = response.data as DailyJournal;
      datas.value?.push(data);
      modal.value = false;
      ToastService.successToast("Berhasil Menambahkan Catatan Kejadian");
    } else {
      ToastService.dangerToast("Gagal Menambahkan Catatan Kejadian")
    }
  }
}


const confirmDelete = (journal: DailyJournal) => {
  DialogService.showDialog("Yakin hapus data catatan/kejadian ini ? ", journal, 'danger').then(async () => {
    const response = await PicketService.deleteJournal(journal.id);
    if (response.isSuccess) {
      const i = datas.value?.indexOf(journal);
      if (i !== undefined && i !== -1) {
        datas.value?.splice(i, 1);

      }
      ToastService.successToast("Berhasil Menghapus Catatan Kejadian")
    } else {
      ToastService.dangerToast("Gagal Menghapus Catatan Kejadian")
    }
  })
}

const edit = (journal: DailyJournal) => {
  model.value = journal;
  model.value.createAt = Helper.getDateTimeString(new Date(journal.createAt), 'YYYY-MM-DDTHH:mm:ss');
  modal.value = true;
  formTitle.value = 'Edit';
}

const createNew = () => {
  model.value = { createAt: Helper.getDateTimeString(new Date(), 'YYYY-MM-DDTHH:mm:ss') } as DailyJournal;

  modal.value = true;

  formTitle.value = 'Tambah';
}


</script>

<style scoped></style>
