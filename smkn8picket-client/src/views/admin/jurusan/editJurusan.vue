<script setup lang="ts">
import { reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ToastService, DepartmentService } from '@/services'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import PageHeader from '@/components/PageHeader.vue'
import { FwbInput, FwbCard, FwbTextarea, FwbButton } from 'flowbite-vue'
import type { Department } from '@/models'
import { Helper, type ErrorDetail } from '@/commons'
import VTInput from '@/components/VTInput/VTInput.vue'

const data = reactive({ form: {} as Department, errors: [] as ErrorDetail[] })
const router = useRouter()
const route = useRoute()

const departmentId = Number(route.params.id)
try {
  const departmentId = Number(route.params.id)
  DepartmentService.getById(departmentId).then((response) => {
    if (response.isSuccess) {
      data.form = response.data as Department
    }
  })
} catch (error) {
  console.error('Error fetching department data:', error)
}

// Fungsi untuk memperbarui data jurusan

const updateDepartment = async () => {
  try {
    const response = await DepartmentService.put(departmentId, data.form)
    if (response.isSuccess) {
      ToastService.successToast('Data berhasil disimpan.')
      router.push({ path: '/admin/Jurusan' })
    } else {
      data.errors = response.error?.errors as ErrorDetail[]
      ToastService.dangerToast(Helper.readError(data.errors, 'Message'))
    }
  } catch (error) {
    const err = error as ErrorDetail
    ToastService.dangerToast(err.description)
  }
}
</script>

<template>
  <AdminLayout>
    <div class="grid grid-cols-2 gap-4">
      <fwb-card class="p-6">
        <PageHeader title="Edit Jurusan"></PageHeader>
        <!-- Form Edit Jurusan -->
        <form @submit.prevent="updateDepartment">
          <div class="mb-4">
            <fwb-input v-model="data.form.name" placeholder="nama jurusan" label="Nama Jurusan" />
          </div>

          <div class="mb-4">
            <VTInput v-model="data.form.initial" placeholder="kode jurusan" label="kode jurusan" />
          </div>

          <div class="mb-4">
            <fwb-textarea v-model="data.form.description" placeholder="deskripsi jurusan" label="deskripsi jurusan" />
          </div>

          <!-- Tombol Submit -->
          <div class="flex justify-end">
            <fwb-button color="default" type="submite">Simpan</fwb-button>

          </div>
        </form>
      </fwb-card>
    </div>
  </AdminLayout>
</template>
