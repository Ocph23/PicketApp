<template>
  <div class="no-print">
    <PageHeader :title="picketId ? 'Detail Piket ' : 'Piket Hari Ini'">
      <FwbButton :color="'yellow'" @click="print">
        <PrinterIcon class="w-5 h-5"></PrinterIcon>
      </FwbButton>
    </PageHeader>

    <div v-if="!picketId && showMessage">
      <div class="alert alert-warning">
        <div>{{ systemMessage }}</div>
      </div>
      <div class="flex justify-center" v-if="isAdminOrPicket">
        <button @click="openPicket" class="w-1/2 btn-close text-white  bg-amber-500  m-2 mt-5 p-5 rounded-lg shadow-md"
          aria-label="Close">
          Buka Piket
        </button>
      </div>
    </div>
    <div v-if="data.picket.id">
      <div class="space-y-3 p-4 rounded-lg shadow-md">
        <div class="flex">
          <label class="labelTitle dark:text-white">Tanggal</label>
          <label class="labelValue dark:text-white">: {{ Helper.getDateTimeString(new Date(data.picket.date),
            'dd-mm-yyyy') }}
          </label>
        </div>
        <div class="flex">
          <label class="labelTitle dark:text-white">Cuaca</label>
          <label class="labelValue dark:text-white">: {{ Helper.getWeartherString(data.picket.weather) }}</label>
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

  </div>

  <PiketTodayViewPrint v-if="showPrint" :data="data.picket"></PiketTodayViewPrint>


</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import { ref, onMounted, reactive } from 'vue'
import { Helper } from '@/commons'

import { PicketService, ToastService } from '@/services'
import type { Picket } from '@/models'
import type { RequestResponse } from '@/models/Responses'
import PageHeader from '@/components/PageHeader.vue'
import {
  FwbTab,
  FwbTabs,
  FwbButton,
} from 'flowbite-vue'
import PicketAbsen from './PicketAbsen.vue'
import PicketDailyJournalView from './PicketDailyJournalView.vue'
import AuthService from '@/services/AuthService'
import { PrinterIcon } from '@heroicons/vue/24/solid'
import PiketTodayViewPrint from './PiketTodayViewPrint.vue'
import StudentsLateAndComeHomeEarlyView from './StudentsLateAndComeHomeEarlyView.vue'
import type { LateAndComeHomeEarlyResponse } from '@/models/LateAndComeHomeEarly'

const activeTab = ref('kejadian')
const systemMessage = ref('')
const showMessage = ref(false)
const data = reactive({ picket: { studentsLateAndComeHomeEarly: [] as LateAndComeHomeEarlyResponse[] } as Picket })
const showPrint = ref(false)
const isAdminOrPicket = ref(false);
const route = useRoute()
const picketId: number = Number(route.params.id)

const openPicket = async () => {
  const response: RequestResponse = await PicketService.openPicket()
  if (response.isSuccess) {
    data.picket = response.data as Picket
    showMessage.value = false;
  } else {
    ToastService.dangerToast(response.error != null ? response.error.detail : '')
  }
}

onMounted(async () => {
  isAdminOrPicket.value = await AuthService.isAdminOrPicket();
  let response: RequestResponse = { isSuccess: false, data: null, error: null }
  if (picketId) {
    response = await PicketService.getById(picketId)
  } else {
    response = await PicketService.get()
  }

  if (!response.isSuccess) {
    systemMessage.value = `Piket hari ini,  ${Helper.getIndonesiaDay(new Date().getDay())}, ${Helper.getDateTimeString(new Date(), 'dd-MM-yyyy')} belum dibuka.`
    showMessage.value = true
  } else {
    data.picket = response.data as Picket
    showMessage.value = false
  }
})



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
