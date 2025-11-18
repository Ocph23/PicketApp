<template>
  <AdminLayout>
    <div class="w-full grid grid-cols-1 md:!grid-cols-4 gap-4">
      <div class="flex flex-col gap-4">
        <DashboardCard
          :total="data?.totalDepartments || 0"
          picture="/department.png"
          title="Total Jurusan"
        />
        <DashboardCard
          :total="data?.totalClassrooms || 0"
          picture="/classroom1.png"
          title="Total Kelas"
        />
        <DashboardCard
          :has-gender="true"
          :man="data?.totalMaleTeachers"
          :women="data?.totalFemaleTeachers"
          :total="data?.totalTeachers || 0"
          picture="/teacher.png"
          title="Total Guru"
        />
        <DashboardCard
          :has-gender="true"
          :man="data?.totalMaleStudents"
          :women="data?.totalFemaleStudents"
          :total="data?.totalStudents || 0"
          picture="/students.png"
          title="Total Siswa"
        />
      </div>
      <div class="md:col-span-3 gap-4">
        <FwbCard class="p-5 !min-w-full">
          <div>
            <h1 class="text-center dark:text-white text-xl xl:text-2xl font-bold">
              GRAFIK TINGKAT KEHADIRAN SISWA
            </h1>
            <h1 class="text-center dark:text-white text-xl xl:text-2xl font-bold">
              Tahun Ajaran {{ data?.schoolYearName }} {{ data?.semesterName }}
            </h1>
          </div>
          <ApexCharts type="bar" :options="chartOptions" :series="series"> </ApexCharts>
        </FwbCard>
      </div>
    </div>
  </AdminLayout>
</template>

<script setup lang="ts">
import DashboardCard from '@/components/DashboardCard.vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import type DashboardResponse from '@/models/Responses/DashboardResponse'
import DashboardService from '@/services/DashboardService'
import type { ApexOptions } from 'apexcharts'
import { FwbCard } from 'flowbite-vue'
import { reactive, ref } from 'vue'
import ApexCharts from 'vue3-apexcharts'

const data = ref<DashboardResponse>()

const chartOptions = reactive({
  chart: {
    id: 'vuechart-example',
    stacked: true,
    stackType: 'normal',
  },
  xaxis: {
    categories: [] as string[],
  },
} as ApexOptions)

const series = ref<{ name: string; data: number[] }[]>([
  { name: 'hadir', data: [] as number[] },
  { name: 'sakit', data: [] },
  { name: 'izin', data: [] },
  { name: 'alpha', data: [] },
])

DashboardService.get()
  .then((response) => {
    data.value = response.data as DashboardResponse
    data.value.kehadirans.forEach((value) => {
      chartOptions.xaxis?.categories.push(value.groupName)
      const jumlahMasuk = value.data.length
      const hadir = value.data.filter((x) => x.status == 1 || x.status == 2).length
      const alpha = value.data.filter((x) => x.status == 3).length
      const sakit = value.data.filter((x) => x.status == 4).length
      const izin = value.data.filter((x) => x.status == 5).length

      series.value
        .filter((x) => x.name == 'hadir')[0]
        ?.data.push(Number(((hadir / jumlahMasuk) * 100).toFixed(2)))
      series.value
        .filter((x) => x.name == 'sakit')[0]
        ?.data.push(Number(((sakit / jumlahMasuk) * 100).toFixed(2)))
      series.value
        .filter((x) => x.name == 'izin')[0]
        ?.data.push(Number(((izin / jumlahMasuk) * 100).toFixed(2)))
      series.value
        .filter((x) => x.name == 'alpha')[0]
        ?.data.push(Number(((alpha / jumlahMasuk) * 100).toFixed(2)))
    })
    console.log(series.value)
    console.log(chartOptions.xaxis?.categories)
  })
  .catch((error) => {
    console.error('Error fetching dashboard data:', error)
  })
</script>
