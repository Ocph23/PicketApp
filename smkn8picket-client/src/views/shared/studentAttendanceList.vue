<template>
  <div>

    <PageHeader title="Data Absen" class="mt-5">
      <div class="flex justify-between gap-2">
        <div class="mb-4">
          <FwbSelect :options="schoolYears" label="Tahun Ajaran" v-model="schoolYearSelected"
            @change="onChangeSelectSchoolYear()" required />
        </div>
      </div>
    </PageHeader>
    <fwb-table>
      <fwb-table-head>
        <fwb-table-head-cell>No</fwb-table-head-cell>
        <fwb-table-head-cell>Tanggal</fwb-table-head-cell>
        <fwb-table-head-cell>Status</fwb-table-head-cell>
        <fwb-table-head-cell>Masuk</fwb-table-head-cell>
        <fwb-table-head-cell>Pulang</fwb-table-head-cell>
        <fwb-table-head-cell></fwb-table-head-cell>
      </fwb-table-head>
      <fwb-table-body>
        <fwb-table-row v-for="(absen, index) in datas" :key="absen.id">
          <fwb-table-cell>
            {{ index + 1 }}
          </fwb-table-cell>
          <fwb-table-cell>
            {{ Helper.getIndonesiaDay(new Date(absen.picketDate).getDay()) }},
            {{ Helper.getDateTimeString(new Date(absen.picketDate), 'dd-MM-yyyy') }}
          </fwb-table-cell>
          <fwb-table-cell>{{ Helper.getAttendanceStatus(absen.status) }}</fwb-table-cell>
          <fwb-table-cell> <span v-if="absen.status == 1 || absen.status == 2">{{ DateTime.fromJSDate(new
            Date(absen.timeIn)).toFormat("HH:mm:ss") }}</span>
          </fwb-table-cell>
          <fwb-table-cell>{{ absen.timeOut ? DateTime.fromJSDate(new Date(absen.timeOut)).toFormat("HH:mm:ss") : "-"
          }}</fwb-table-cell>
          <fwb-table-cell>

          </fwb-table-cell>

        </fwb-table-row>
      </fwb-table-body>
    </fwb-table>

  </div>
</template>

<script setup lang="ts">
import type { RequestResponse } from '@/commons';
import type { SchoolYear, StudentAttendance } from '@/models';
import { SchoolYearService, StudentAttendanceService } from '@/services';
import { ref } from 'vue';
import PageHeader from '@/components/PageHeader.vue'
import { DateTime } from 'luxon';
import { Helper } from '@/commons';
import {
  FwbSelect,
  FwbTable,
  FwbTableCell,
  FwbTableRow,
  FwbTableBody,
  FwbTableHeadCell,
  FwbTableHead
} from 'flowbite-vue'
import _ from 'lodash';


const props = defineProps({
  studentId: {
    type: Number,
    required: true,
  },
});

const schoolYearSelected = ref();

const schoolYears = ref<{ name: string, value: string }[]>([]);
const datas = ref<StudentAttendance[]>([])


SchoolYearService.get().then((response: RequestResponse) => {
  const data = response.data as SchoolYear[];
  schoolYears.value = data.map((item) => {
    return {
      value: item.id.toString(),
      name: `${item.name} ${item.semesterName}`,
    }
  });

  const schoolYearActive = data.find((item) => item.actived);
  schoolYearSelected.value = schoolYearActive?.id.toString() || '';
  showData();
})


const onChangeSelectSchoolYear = () => {
  showData();
}

const showData = () => {
  StudentAttendanceService.getByStudentId(schoolYearSelected.value, props.studentId).then((attendances) => {
    const response = attendances as RequestResponse;
    const result = response.data as StudentAttendance[];
    // setMonthYear
    const lastresult = result.filter(x => x.schoolYearId == schoolYearSelected.value)
      .map((item) => {
        const date = new Date(item.picketDate.toString());
        const month = date.toLocaleString('default', { month: 'short' });
        item.monthYear = month + ' ' + date.getFullYear();
        return item;
      });
    const bulans = _.groupBy(lastresult, 'monthYear');
    datas.value = lastresult;
    _.forEach(bulans, (value, key) => {

    });

    setTimeout(() => {

    }, 1000)

  })


}



</script>

<style scoped></style>
