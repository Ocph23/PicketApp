<template>
  <div ref="myDiv" style="width: 100%; height: 100%;">
    <div class="mb-4">
      <FwbSelect :options="schoolYears" label="Tahun Ajaran" v-model="schoolYearSelected"
        @change="onChangeSelectSchoolYear()" required />
    </div>
    <VueApexCharts type="bar" :options="chartOptions" :series="series"></VueApexCharts>
  </div>
</template>

<script setup lang="ts">
import type { RequestResponse } from '@/commons';
import type StudentAttendanceRequest from '@/models/Requests/StudentAttendanceRequest';
import { SchoolYearService, StudentAttendanceService } from '@/services';
import { ref } from 'vue';
import _ from 'lodash'
import type { SchoolYear } from '@/models';
import { FwbSelect } from 'flowbite-vue';
import VueApexCharts from 'vue3-apexcharts';

const props = defineProps({
  studentId: {
    type: Number,
    required: true,
  },
});

const showChart = ref(false);
const myDiv = ref(null);
const schoolYearSelected = ref();

const schoolYears = ref<{ name: string, value: string }[]>([]);



const chartOptions = ref({
  chart: {
    id: "vuechart-example",
    stacked: true,
    stackType: "100%"

  },
  xaxis: {
    categories: [] as string[],
  },
}
)


SchoolYearService.get().then((response: RequestResponse) => {
  const data = response.data as SchoolYear[];
  schoolYears.value = data.map((item) => {
    return {
      value: item.id.toString(),
      name: `${item.name} ${item.semesterName}`,
    }
  });

  const schoolYearActive = data.find((item) => item.actived);
  schoolYearSelected.value = schoolYearActive?.id;
  generateChart();


})

const onChangeSelectSchoolYear = () => {
  showChart.value = false;
  generateChart();
}

const series = ref<{ name: string, data: number[] }[]>([])
const generateChart = () => {
  if (!schoolYearSelected.value) return;

  StudentAttendanceService.getByStudentId(schoolYearSelected.value, props.studentId).then((attendances) => {
    const response = attendances as RequestResponse;
    const result = response.data as StudentAttendanceRequest[];
    // setMonthYear
    if (result) {
      const lastresult = result.filter(x => x.schoolYearId == schoolYearSelected.value)
        .map((item) => {
          const date = new Date(item.picketDate.toString());
          const month = date.toLocaleString('default', { month: 'short' });
          item.monthYear = month + ' ' + date.getFullYear();
          return item;
        });
      const bulan = _.groupBy(lastresult, 'monthYear');

      chartOptions.value.xaxis.categories.length = 0;
      const seriesx = [
        { name: "hadir", data: [] as number[] },
        { name: "sakit", data: [] },
        { name: "izin", data: [] },
        { name: "alpha", data: [] },
        { name: "bolos", data: [] },
        { name: "lainnya", data: [] },
      ];

      _.forEach(bulan, (value, key) => {
        chartOptions.value.xaxis.categories.push(key);
        const jumlahMasuk = value.length;
        const hadir = value.filter(x => x.status == 1 || x.status == 2).length;
        const alpha = value.filter(x => x.status == 3).length;
        const sakit = value.filter(x => x.status == 4).length;
        const izin = value.filter(x => x.status == 5).length;
        const bolos = value.filter(x => x.status == 6).length;
        const lainnya = value.filter(x => x.status == 7).length;

        seriesx.filter(x => x.name == 'hadir')[0]?.data.push(hadir / jumlahMasuk * 100);
        seriesx.filter(x => x.name == 'sakit')[0]?.data.push(sakit / jumlahMasuk * 100);
        seriesx.filter(x => x.name == 'izin')[0]?.data.push(izin / jumlahMasuk * 100);
        seriesx.filter(x => x.name == 'alpha')[0]?.data.push(alpha / jumlahMasuk * 100);
        seriesx.filter(x => x.name == 'bolos')[0]?.data.push(bolos / jumlahMasuk * 100);
        seriesx.filter(x => x.name == 'lainnya')[0]?.data.push(lainnya / jumlahMasuk * 100);
      });

      setTimeout(() => {
        showChart.value = true;
        // console.log(chartOptions.value.xaxis.categories);
        // console.log(series);
        series.value = seriesx;
      }, 1000)
    }

  })
}



generateChart();



</script>

<style scoped></style>
