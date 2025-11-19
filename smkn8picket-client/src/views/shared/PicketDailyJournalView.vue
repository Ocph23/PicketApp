<template>
  <PageHeader title="Catatan Piket" class="mt-5">
    <button v-if="props.canAdd">
      <AddIcon @click="createNew" class="w-7 h-7" />
    </button>
  </PageHeader>

  <VTTableNew method="All" :source="dataTable" :columns="tableColumns" :hovered="false">
    <template #no="item">
      {{ item.index + 1 }}
    </template>
    <template #waktu="item">
      {{ DateTime.fromISO(item.data.createAt).toFormat('HH:mm:ss') }}
    </template>
    <template #actions="item">
      <div class="flex items-center" v-if="item.data.teacherId === teacherLoginId">
        <button class="text-white flex" @click="edit(item.data)">
          <EditIcon />
        </button>
        <button @click="confirmDelete(item.data)" class="text-white flex">
          <DeleteIcon />
        </button>
      </div>
    </template>
  </VTTableNew>

  <VTModal class="modal opacity-[99%]" v-if="modal" :size="'3xl'" @close="modal = false" :persistent="true">
    <template #header>
      <h3 tag="h3" class="text-lg pb-3 font-bold">
        {{ formTitle }} Catatan Kejadian
      </h3>
    </template>

    <template #body>
      <form class="flex flex-col gap-3" @submit.prevent="saveJurnal(model)">
        <VTInput v-model="model.createAt" label="Waktu" :type="'datetime-local'" required />
        <VTInput v-model="model.title" label="Judul" type="text" required></VTInput>
        <VTTextArea v-model="model.content" label="Kejadian" required></VTTextArea>
        <div class="flex justify-end gap-2">
          <VTButton :color="'alternative'" @click="modal = false">Batal</VTButton>
          <VTButton type="submit">Simpan</VTButton>
        </div>
      </form>
    </template>
  </VTModal>

</template>

<script setup lang="ts">
import PageHeader from '@/components/PageHeader.vue'
import { onMounted, reactive, ref } from 'vue';
import AddIcon from '@/components/icons/AddIcon.vue';
import type DailyJournal from '@/models/DailyJournal';
import { PicketService } from '@/services';
import { EditIcon, DeleteIcon } from '@/components/icons';
import Helper from '@/commons/helper';
import AuthService from '@/services/AuthService';
import { DateTime } from 'luxon';
import VTInput from '@/components/VTInput/VTInput.vue';
import { VTButton, VTDialogService, VTModal, VTTableNew, VTTextArea, VTToastService } from '@ocph23/vtocph23';
import type { VTTableColumn, VTTableSource } from '@ocph23/vtocph23/components/VTTable/index.js';

const props = defineProps({ data: Array<DailyJournal>, canAdd: Boolean })
const datas = ref<DailyJournal[]>();
const model = ref<DailyJournal>({} as DailyJournal);

const formTitle = ref('Tambah');

const modal = ref(false);
datas.value = props.data || [];


const dataTable = reactive<VTTableSource<DailyJournal>>({
  totalRecords: datas.value ? datas.value.length : 0,
} as VTTableSource<DailyJournal>);



onMounted(() => {
  dataTable.data = datas.value as DailyJournal[];
});


const tableColumns = [
  { title: 'No', name: 'no', type: 'Custome', headerClass: 'w-10 text-center', rowClass: 'text-center' },
  { title: 'Waktu', name: 'waktu', type: 'Custome' },
  { title: 'Judul', propName: 'title' },
  { title: 'Kejadian', propName: 'content' },
  { title: 'Dibuat Oleh', propName: 'teacherName' },
  { title: 'Aksi', name: 'actions', type: 'Custome', headerClass: 'text-center h-5' },
] as VTTableColumn[];


const teacherLoginId = AuthService.getTeacherId();


if (teacherLoginId) {
  model.value.teacherId = teacherLoginId;
}


const saveJurnal = async (journal: DailyJournal) => {
  if (journal.id) {
    const response = await PicketService.putJournal(journal.id, journal);
    if (response.isSuccess) {
      modal.value = false;
      VTToastService.success("Berhasil mengubah Catatan Kejadian");
    } else {
      VTToastService.error("Gagal Mengubah Catatan Kejadian")
    }
  } else {
    const response = await PicketService.postJournal(journal);
    if (response.isSuccess) {
      const data = response.data as DailyJournal;
      datas.value?.push(data);
      modal.value = false;
      VTToastService.success("Berhasil Menambahkan Catatan Kejadian");
    } else {
      VTToastService.error("Gagal Menambahkan Catatan Kejadian")
    }
  }
}


const confirmDelete = (journal: DailyJournal) => {
  VTDialogService.asyncShowDialog('Perhatian', "Yakin hapus data catatan/kejadian ini ? ", journal, 'danger').then(async () => {
    const response = await PicketService.deleteJournal(journal.id);
    if (response.isSuccess) {
      const i = datas.value?.indexOf(journal);
      if (i !== undefined && i !== -1) {
        datas.value?.splice(i, 1);

      }
      VTToastService.success("Berhasil Menghapus Catatan Kejadian")
    } else {
      VTToastService.error("Gagal Menghapus Catatan Kejadian")
    }
  })
}

const edit = (journal: DailyJournal) => {
  model.value = journal;
  model.value.createAt = DateTime.fromJSDate(new Date(journal.createAt)).toFormat("yyyy-MM-dd'T'HH:mm");
  modal.value = true;
  formTitle.value = 'Edit';
}

const createNew = () => {
  const date = DateTime.fromJSDate(new Date()).toFormat("yyyy-MM-dd'T'HH:mm");
  model.value = { createAt: date } as DailyJournal;
  modal.value = true;
  formTitle.value = 'Tambah';
}


</script>

<style scoped></style>
