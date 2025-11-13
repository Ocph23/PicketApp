<template>
  <AdminLayout>
    <PageHeader title="Absen Guru">
      <div class="flex justify-end items-center gap-1">
        <VTSelect :size="'sm'" label="Bulan" :options="bulans"></VTSelect>
        <VTSelect :size="'sm'" label="Tahun" :options="tahuns"></VTSelect>
        <PrinterIcon class="w-10 h-auto cursor-pointer text-amber-400 hover:text-amber-600 bg-gray-200"></PrinterIcon>
      </div>
    </PageHeader>
  </AdminLayout>
</template>

<script setup lang="ts">
import { Helper } from '@/commons';
import AdminLayout from '@/components/layouts/AdminLayout.vue';
import PageHeader from '@/components/PageHeader.vue';
import type { SelectOption } from '@/components/VTSelect/types';
import VTSelect from '@/components/VTSelect/VTSelect.vue';
import { PrinterIcon } from '@heroicons/vue/24/solid';
import { DateTime } from 'luxon';


const bulans = Helper.getIndonesiaMonth().map((item, index) => {
  return { name: item, value: index + 1 } as SelectOption
});

const tahuns = range(DateTime.now().year - 4, DateTime.now().year).map((item => {
  return { name: item+1, value: item+1 }
}))



function range(start:number, end:number, step = 1) {
  const result = [];
  if (step === 0) throw new Error("Step cannot be zero");
  const increasing = step > 0;

  for (let i = start; increasing ? i < end : i > end; i += step) {
    result.push(i);
  }

  return result;
}



</script>

<style scoped></style>
