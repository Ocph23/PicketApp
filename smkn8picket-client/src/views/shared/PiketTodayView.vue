<template>
  <VTCard :title="picketId ? 'Detail Piket ' : 'Piket Hari Ini'" class="no-print">
    <template #rightSide>
      <div class="flex items-center gap-3">
        <VTInput
          v-if="isAdminOrPicket && !picketId"
          v-model="selectedDate"
          type="date"
          label="Tanggal"
          class="w-52"
        />
        <VTButton v-if="isAdminOrPicket && !picketId" color="alternative" @click="() => openPicketByDate()">
          Buka Piket
        </VTButton>
        <PrinterIcon class="w-8 h-8 cursor-pointer text-amber-400" @click="print"></PrinterIcon>
      </div>
    </template>


    <div v-if="!picketId && showMessage">
      <div class="alert alert-warning">
        <div>{{ systemMessage }}</div>
      </div>
      <div class="flex justify-center" v-if="isAdminOrPicket">
        <button @click="() => openPicketByDate()"
          class="w-1/2 btn-close text-white  bg-amber-500  m-2 mt-5 p-5 rounded-lg shadow-md cursor-pointer"
          aria-label="Close">
          Buka Piket
        </button>
      </div>
    </div>
    <div v-if="hasPicketData">
      <div class="flex justify-between rounded-lg shadow-md">
        <div class="space-y-3 p-4 ">
          <div class="flex">
            <label class="labelTitle dark:text-white">Tanggal</label>
            <label class="labelValue dark:text-white">:
              {{ DateTime.fromISO(data.picket.date).setLocale('id').toFormat('cccc, dd LLLL yyyy') }}
            </label>
          </div>
          <div class="flex">
            <label class="labelTitle dark:text-white">Cuaca</label>
            <label class="labelValue dark:text-white">: {{Helper.wheatherOptions.find(x => x.value ===
              data.picket.weather)?.name
              }}</label>
          </div>
          <div class="flex">
            <label class="labelTitle dark:text-white">Mulai Jam</label>
            <label class="labelValue dark:text-white">: {{ data.picket.startAt }} </label>
          </div>
          <div class="flex">
            <label class="labelTitle dark:text-white">Berakhir Jam</label>
            <label class="labelValue dark:text-white">: {{ data.picket.endAt }} </label>
          </div>
          <!-- <div class="flex">
            <label class="labelTitle dark:text-white">Jumlah Guru</label>
            <label class="labelValue dark:text-white">: </label>
          </div>
          <div class="flex">
            <label class="labelTitle dark:text-white">Jumlah Siswa</label>
            <label class="labelValue dark:text-white">: </label>
          </div> -->
          <div class="flex">
            <label class="labelTitle dark:text-white">Picket Dibuka oleh</label>
            <label class="labelValue dark:text-white">: {{ data.picket.createdName }}</label>
          </div>
        </div>
        <EditIcon class="w-5 h-5 m-2 cursor-pointer text-amber-400" @click="edit"></EditIcon>
      </div>
      <div class="mt-10">
        <fwb-tabs v-model="activeTab" class="p-3">
          <fwb-tab name="kejadian" title="Catatan Kejadian">
            <PicketDailyJournalView :canAdd="isNaN(picketId) ? true : false" :data="data.picket.dailyJournal">
            </PicketDailyJournalView>
          </fwb-tab>
          <fwb-tab name="absen" title="Absen">
            <PicketAbsen :picket="data.picket" :canAdd="isNaN(picketId) ? true : false"></PicketAbsen>
          </fwb-tab>
          <fwb-tab name="third" title="Siswa Pulang Lebih Cepat">
            <StudentsLateAndComeHomeEarlyView :canAdd="isNaN(picketId) ? true : false"
              :data="data.picket.studentsLateAndComeHomeEarly">
            </StudentsLateAndComeHomeEarlyView>
          </fwb-tab>
        </fwb-tabs>
      </div>
    </div>

  </VTCard>

  <VTModal v-if="showModal" :size="'md'" persistent>
    <template #header> Edit Piket </template>
    <template #body>
      <VTInput class="mb-2" label="Tanggal" v-model="editModel.date" :type="'date'" :disabled="true"></VTInput>
      <VTSelect class="mb-2" label="Cuaca" v-model="editModel.weather" :options="Helper.wheatherOptions">
      </VTSelect>
      <VTInput class="mb-2" label="Jam Mulai" v-model="editModel.startAt" :type="'time'"></VTInput>
      <VTInput class="mb-2" label="Jam Selesai" v-model="editModel.endAt" :type="'time'"></VTInput>
    </template>
    <template #footer>
      <div class="flex justify-end gap-2">
        <VTButton :color="'alternative'" @click="showModal = false">
          Batal
        </VTButton>
        <VTButtonSave @click="savePicket">
          Simpan
        </VTButtonSave>
      </div>
    </template>
  </VTModal>

  <PiketTodayViewPrint v-if="showPrint" :data="data.picket"></PiketTodayViewPrint>

</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import { computed, onMounted, reactive, ref } from 'vue'
import { Helper } from '@/commons'

import { PicketService } from '@/services'
import type { Picket } from '@/models'
import type { RequestResponse } from '@/models/Responses'
import {
  FwbTab,
  FwbTabs,
} from 'flowbite-vue'
import PicketAbsen from './PicketAbsen.vue'
import PicketDailyJournalView from './PicketDailyJournalView.vue'
import AuthService from '@/services/AuthService'
import { PrinterIcon } from '@heroicons/vue/24/solid'
import PiketTodayViewPrint from './PiketTodayViewPrint.vue'
import StudentsLateAndComeHomeEarlyView from './StudentsLateAndComeHomeEarlyView.vue'
import type { LateAndComeHomeEarlyResponse } from '@/models/LateAndComeHomeEarly'
import { VTButton, VTButtonSave, VTCard, VTModal, VTSelect, VTToastService } from '@ocph23/vtocph23'
import { DateTime } from 'luxon'
import VTInput from '@/components/VTInput/VTInput.vue'
import EditIcon from '@/components/icons/EditIcon.vue'

const activeTab = ref('kejadian')
const systemMessage = ref('')
const showMessage = ref(false)
const showModal = ref(false)
const data = reactive({ picket: { studentsLateAndComeHomeEarly: [] as LateAndComeHomeEarlyResponse[] } as Picket })
const showPrint = ref(false)
const isAdminOrPicket = ref(false);
const route = useRoute()
const picketId: number = Number(route.params.id)
const routeDate = computed(() => {
  const value = route.query.date
  return typeof value === 'string' && value.length > 0 ? value : ''
})
const selectedDate = ref(routeDate.value || DateTime.now().toISODate() || '')
const hasPicketData = computed(() => !!data.picket?.date)

const setPicketNotOpenedMessage = () => {
  const readableDate = DateTime.fromISO(selectedDate.value || DateTime.now().toISODate() || '')
    .setLocale('id')
    .toFormat('cccc, dd LLLL yyyy')
  systemMessage.value = `Piket pada ${readableDate} belum dibuka.`
  showMessage.value = true
}

const loadPicketByDate = async (date = selectedDate.value) => {
  if (!date) {
    VTToastService.error('Tanggal belum dipilih')
    return
  }

  selectedDate.value = date
  const response: RequestResponse = await PicketService.getByDate(date)
  if (!response.isSuccess) {
    setPicketNotOpenedMessage()
    return
  }

  data.picket = response.data as Picket
  showMessage.value = false
}

const openPicketByDate = async () => {
  const date = selectedDate.value
  if (!date) {
    VTToastService.error('Tanggal belum dipilih')
    return
  }

  selectedDate.value = date
  const response: RequestResponse = await PicketService.openPicketByDate(date)
  if (response.isSuccess) {
    data.picket = response.data as Picket
    showMessage.value = false;
  } else {
    VTToastService.error(response.error != null ? response.error.detail : '')
    setPicketNotOpenedMessage()
  }
}

onMounted(async () => {
  isAdminOrPicket.value = await AuthService.isAdminOrPicket();
  let response: RequestResponse = { isSuccess: false, data: null, error: null }
  if (picketId) {
    response = await PicketService.getById(picketId)
  } else if (selectedDate.value) {
    response = await PicketService.getByDate(selectedDate.value)
  } else {
    response = await PicketService.get()
  }

  if (!response.isSuccess) {
    setPicketNotOpenedMessage()
  } else {
    data.picket = response.data as Picket
    showMessage.value = false
  }
})


let editModel = reactive({} as Picket)
const edit = () => {
  editModel = { ...data.picket }
  showModal.value = true
}

const savePicket = async () => {
  const response: RequestResponse = await PicketService.putPicket(editModel.id, editModel)
  if (response.isSuccess) {
    showModal.value = false
    data.picket.weather = editModel.weather
    data.picket.startAt = editModel.startAt
    data.picket.endAt = editModel.endAt
    VTToastService.success('Data Piket berhasil disimpan')
  } else {
    VTToastService.error(response.error != null ? response.error.detail : '')
  }
}



const print = () => {
  showPrint.value = true
  setTimeout(() => {
    window.print()
    showPrint.value = false
  }, 1000)
}



</script>

<style lang="css" scoped>
.alert {
  padding: 1em;
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
  border-radius: 0.25rem;
}

.labelTitle {
  margin-right: 10px;
  width: 150px;
}

.labelValue dark:text-white {
  font-weight: normal;
}
</style>
