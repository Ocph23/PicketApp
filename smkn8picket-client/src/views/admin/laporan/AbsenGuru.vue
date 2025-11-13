<template>
  <AdminLayout>
    <PageHeader title="Absen Guru">
      <div class="flex justify-end gap-1">
        <VTSelect label="Bulan" :options="bulans"></VTSelect>
        <VTSelect label="Tahun" :options="tahuns"></VTSelect>
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
import { DateTime } from 'luxon';


const bulans = Helper.getIndonesiaMonth().map((item, index) => {
  return { name: item, value: index + 1 } as SelectOption
});

const tahuns = range(DateTime.now().year - 4, DateTime.now().year).map((item => {
  return { name: item+1, value: item+1 }
}))



function range(start, end, step = 1) {
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
