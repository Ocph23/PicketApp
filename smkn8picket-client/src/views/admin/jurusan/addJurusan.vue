<script setup lang="ts">
import { useRouter } from "vue-router";
import AdminLayout from "@/components/layouts/AdminLayout.vue";
import PageHeader from "@/components/PageHeader.vue";
import { reactive } from "vue";
import { DepartmentService } from "@/services";
import { Helper, type ErrorDetail } from "@/commons";
import { FwbCard, FwbTextarea, FwbButton } from 'flowbite-vue'
import type { Department } from "@/models";
import VTInput from '@/components/VTInput/VTInput.vue'
import { VTToastService } from "@ocph23/vtocph23";

const router = useRouter();

const data = reactive({
  form: {} as Department,
  errors: [] as ErrorDetail[],
});

const addData = async () => {
  try {
    const response = await DepartmentService.post(data.form);
    if (response.isSuccess) {
      VTToastService.success("Data berhasil disimpan.");
      router.push({ path: "/admin/Jurusan" });
    } else {
      const error = response.error;
      if (error && error.errors) {
        data.errors = error.errors;
        VTToastService.error(Helper.readError(error.errors, "Message"));
      }
    }
  } catch (error) {
    console.error("Error adding data:", error);
  }
};

</script>

<template>
  <AdminLayout>

    <div class="grid grid-cols-2 gap-4">
      <fwb-card class="p-6">
        <PageHeader title="Tambah Jurusan"></PageHeader>
        <form @submit.prevent="addData">
          <div class="mb-4">
            <VTInput v-model="data.form.name" placeholder="nama jurusan" label="Nama Jurusan"
              :validation-status="Helper.getErrorStatus(data.errors, 'Name')">
              <template #validationMessage>
                <span class="text-xs"> {{ Helper.readError(data.errors, 'Name') }}</span>
              </template>
            </VTInput>
          </div>

          <div class="mb-4">
            <VTInput v-model="data.form.initial" placeholder="kode jurusan" label="kode jurusan"
              :validation-status="Helper.getErrorStatus(data.errors, 'Initial')">
              <template #validationMessage>
                <span class="text-xs"> {{ Helper.readError(data.errors, 'Initial') }}</span>
              </template>


            </VTInput>
          </div>

          <div class="mb-4">
            <fwb-textarea v-model="data.form.description" placeholder="deskripsi jurusan" label="deskripsi jurusan">
            </fwb-textarea>
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
