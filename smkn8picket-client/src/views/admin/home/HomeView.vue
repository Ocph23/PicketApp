<template>
  <AdminLayout class="gap-4">
    <div class="grid grid-cols-1 gap-4 lg:grid-cols-4">
      <div class="flex flex-col gap-4">
        <FwbCard class="col-6 p-5 !min-w-full ">
          <div class="flex flex-col items-start justify-start">
            <div class="w-full flex  items-start justify-between ">
              <h1 class="text-center text-2xl xl:text-4xl font-bold">{{ data?.totalDepartments || 0 }}</h1>
              <img src="/department.png" class="text-primary w-10 h-10" />
            </div>
            <h1 class="text-center text-xl xl:text-2xl text-teal-200">Total Jurusan</h1>
          </div>
        </FwbCard>
        <FwbCard class="col-6 p-5 !min-w-full ">
          <div class="flex flex-col items-start justify-start">
            <div class="w-full flex  items-start justify-between ">
              <h1 class="text-center text-2xl xl:text-4xl font-bold">{{ data?.totalClassrooms || 0 }}</h1>
              <img src="/classroom1.png" class="text-primary w-10 h-10" />
            </div>
            <h1 class="text-center text-xl xl:text-2xl text-teal-200">Total Kelas</h1>
          </div>
        </FwbCard>
        <FwbCard class="col-6 p-5 !min-w-full ">
          <div class="flex flex-col items-start justify-start">
            <div class="w-full flex  items-start justify-between ">
              <h1 class="text-center text-2xl xl:text-4xl font-bold">{{ data?.totalTeachers || 0 }}</h1>

              <img src="/teacher.png" class="text-primary w-10 h-10" />
            </div>
            <div class="flex justify-between w-full">
              <h1 class="text-center text-xl xl:text-2xl text-teal-200">Total Guru</h1>
              <div class="flex gap-4">
                <div class="flex items-center ">
                  <img src="/male.png" class="text-primary w-5 h-5" />
                  <h1 class="text-center text-2xl ">{{ data?.totalMaleTeachers || 0 }}</h1>
                </div>
                <div class="flex items-center ">
                  <img src="/female.png" class="text-primary w-5 h-5" />
                  <h1 class="text-center text-2xl ">{{ data?.totalFemaleTeachers || 0 }}</h1>
                </div>
              </div>
            </div>
          </div>
        </FwbCard>
        <FwbCard class="col-6 p-5 !min-w-full ">
          <div class="flex flex-col items-start justify-start">
            <div class="w-full flex  items-start justify-between ">
              <h1 class="text-center text-2xl xl:text-4xl font-bold">{{ data?.totalStudents || 0 }}</h1>
              <img src="/students.png" class="text-primary w-10 h-10" />
            </div>
            <div class="flex justify-between w-full">
              <h1 class="text-center text-xl xl:text-2xl text-teal-200">Total Siswa</h1>
              <div class="flex gap-4">
                <div class="flex items-center ">
                  <img src="/male.png" class="text-primary w-5 h-5" />
                  <h1 class="text-center text-2xl ">{{ data?.totalMaleStudents || 0 }}</h1>
                </div>
                <div class="flex items-center ">
                  <img src="/female.png" class="text-primary w-5 h-5" />
                  <h1 class="text-center text-2xl ">{{ data?.totalFemaleStudents || 0 }}</h1>
                </div>
              </div>
            </div>
          </div>
        </FwbCard>



      </div>
      <FwbCard class="col-span-3 !min-w-full p-5">
        <div>
          <h1 class="text-center text-xl xl:text-2xl font-bold">GRAFIK TINGKAT KEHADIRAN SISWA</h1>
          <h1 class="text-center text-xl xl:text-2xl font-bold">Tahun Ajaran {{ data?.schoolYearName }} {{
            data?.semesterName }}</h1>
        </div>
        <ApexCharts type="bar" :options="chartOptions" :series="series">
        </ApexCharts>
      </FwbCard>

    </div>
    <!-- <div class="flex flex-col justify-center  items-center !min-w-full">
        <img class="m-1 w-[5%]" :src="Helper.infoSekolah.logo" alt="">
        <p class="text-lg dark:text-white">APLIKASI PIKET </p>
        <p class="text-lg dark:text-white"> {{ Helper.infoSekolah.nama_sekolah }}</p>
        <p class="text-lg dark:text-white"> {{ Helper.infoSekolah.kota }}</p>
        <p class="text-lg dark:text-white">{{ year }}</p>
      </div> -->
  </AdminLayout>
</template>

<script setup lang="ts">
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import type DashboardResponse from '@/models/Responses/DashboardResponse';
import DashboardService from '@/services/DashboardService';
import { FwbCard } from 'flowbite-vue';
import { ref } from 'vue'
import ApexCharts from 'vue3-apexcharts';

const year = ref(new Date().getFullYear())
const data = ref<DashboardResponse>();

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
);

const series = ref<{ name: string, data: number[] }[]>([
  { name: "hadir", data: [] as number[] },
  { name: "sakit", data: [] },
  { name: "izin", data: [] },
  { name: "alpha", data: [] },
])

chartOptions.value.xaxis.categories.length = 0;
DashboardService.get()
  .then((response) => {
    data.value = response.data as DashboardResponse;
    data.value.kehadirans.forEach(value => {
      chartOptions.value.xaxis.categories.push(value.groupName);
      const jumlahMasuk = value.data.length;
      const hadir = value.data.filter(x => x.status == 1 || x.status == 2).length;
      const alpha = value.data.filter(x => x.status == 3).length;
      const sakit = value.data.filter(x => x.status == 4).length;
      const izin = value.data.filter(x => x.status == 5).length;

      series.value.filter(x => x.name == 'hadir')[0]?.data.push(Number((hadir / jumlahMasuk * 100).toFixed(2)));
      series.value.filter(x => x.name == 'sakit')[0]?.data.push(Number((sakit / jumlahMasuk * 100).toFixed(2)));
      series.value.filter(x => x.name == 'izin')[0]?.data.push(Number((izin / jumlahMasuk * 100).toFixed(2)));
      series.value.filter(x => x.name == 'alpha')[0]?.data.push(Number((alpha / jumlahMasuk * 100).toFixed(2)));
    });
    console.log(series.value);
    console.log(chartOptions.value.xaxis.categories);
  })
  .catch((error) => {
    console.error("Error fetching dashboard data:", error);
  });

</script>
