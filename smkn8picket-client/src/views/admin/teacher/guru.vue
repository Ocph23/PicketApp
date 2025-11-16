<template>
  <AdminLayout>
    <VTCard title="Data Guru & Staff">
      <template #right-side>
        <router-link :to="{ name: 'addGuru' }">
          <AddIcon class="w-7 h-7" />
        </router-link>
      </template>
      <VTTable ref="vtTable" :table-name="'tabel-guru'" :bordered="true" :source="data.teachers" :columns="[
        { title: 'No', name: 'no', sortable: false, type: 'Custome' },
        { title: 'Nama', propName: 'name', isMobileHeader: true, sortable: true },
        { title: 'NUPTK', propName: 'registerNumber', sortable: true },
        { title: 'Jenis Kelamin', propName: 'gender', sortable: true, name: 'gender', type: 'Custome' },
        { title: 'TTL', propName: 'dateOfBorn', sortable: true },
        { title: 'Email', propName: 'email', sortable: true },
        { title: 'Jabatan', propName: 'job', sortable: true, name: 'job', type: 'Custome' },
        { title: 'Jenis Pegawai', propName: 'jobStatus', name: 'jobStatus', type: 'Custome', sortable: true },
        { title: 'Aksi', name: 'actions', sortable: false, type: 'Custome' },
      ] as VTTableColumn[]">
        <template #no="row">
          {{ row.index + 1 }}
        </template>
        <template #actions="row">
          <div class="flex items-center gap-2">
            <router-link :to="`/admin/guru/${row.data.id}/edit`">
              <button class="text-white flex">
                <EditIcon></EditIcon>
              </button>
            </router-link>
            <button @click="confirmDelete(row.data)" class="text-white flex cursor-pointer">
              <DeleteIcon />
            </button>
            <button @click="selectTeacher(row.data)" class="text-white flex cursor-pointer">
              <LockClosedIcon class="w-5 h-5 text-sky-600 hover:text-sky-900" />
            </button>
          </div>
        </template>
        <template #gender="row">
          {{Helper.genders.find(x => x.value === row.data.gender)?.name}}
        </template>
        <template #job="row">
          {{Helper.jobs.find(x => x.value === row.data.job)?.name}}
        </template>
        <template #jobStatus="row">
          {{Helper.jobStatus.find(x => x.value === row.data.jobStatus)?.name}}
        </template>
      </VTTable>
    </VTCard>
  </AdminLayout>

  <FwbModal class="modal opacity-[99%]" v-if="modal" :size="'xl'" @close="modal = false" v-model="modal"
    :persistent="true">
    <template #header>
      <FwbHeading tag="h3" class="text-lg pb-3 font-bold">
        Account
      </FwbHeading>
    </template>

    <template #body>

      <table class="w-full text-sm text-left ">
        <tbody>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Nama</td>
            <td class="p-2">
              {{ teacher.profile.name }}
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Email</td>
            <td class="p-2">
              {{ teacher.profile.email }}
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Status Aktif</td>
            <td class="p-2">
              <FwbCheckbox class="bg-red-400" v-model:model-value="teacher.activated" @change="lockConfirm($event)" />
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Admin </td>
            <td class="p-2">
              <FwbCheckbox class="bg-red-400" v-model="teacher.isAdmin" @change="setAdminConfirm($event)">
              </FwbCheckbox>
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Roles </td>
            <td class="p-2">
              {{ teacher.roles }}
            </td>
          </tr>

          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">
              <FwbButton @click="confirmresetPassword" class="bg-red-400"> Reset Password</FwbButton>
            </td>
          </tr>
        </tbody>

      </table>

      <form class="flex justify-end mt-5 gap-3">
        <FwbButton :color="'alternative'" @click="modal = false">Keluar</FwbButton>
      </form>
    </template>
  </FwbModal>

</template>


<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import AdminLayout from "@/components/layouts/AdminLayout.vue";
import { TeacherService, ToastService, DialogService } from "@/services";
import { Helper, type RequestResponse } from "@/commons";
import { EditIcon, DeleteIcon, AddIcon } from "@/components/icons";
import PageHeader from "@/components/PageHeader.vue";
import {
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow, FwbModal, FwbButton, FwbInput, FwbTextarea, FwbHeading,
  FwbCheckbox
} from 'flowbite-vue'
import type { Teacher } from "@/models";
import { LockClosedIcon } from "@heroicons/vue/20/solid";
import AuthService from "@/services/AuthService";
import { VTCard, VTTable, type VTTableColumn } from "@ocph23/vtocph23";

const vtTable = ref<InstanceType<typeof VTTable> | null>(null);
const modal = ref(false);
const teacher = reactive<{
  profile: Teacher,
  activated: boolean, isAdmin: boolean,
  roles: string[]
}>({} as {
  profile: Teacher,
  activated: boolean, isAdmin: boolean, roles: string[]
});

const data = reactive({
  teachers: [] as Teacher[],
  form: {
    registerNumber: null,
    name: "",
    gender: 0,
    placeOfBorn: "",
    dateOfBorn: { year: 0, month: 0, day: 0, dayOfWeek: 0 },
    email: "",
    description: "",
    userId: "",
  },
});


const selectTeacher = (item: Teacher) => {

  teacher.profile = item;

  AuthService.getStatus(item.userId).then((response: RequestResponse) => {
    const data = response.data as {
      activated: boolean, isAdmin: boolean, roles: string[]
    };
    teacher.activated = data.activated;
    teacher.isAdmin = data.isAdmin;
    teacher.roles = data.roles;
    modal.value = true;
  })


}

// Fungsi untuk mengambil data guru/ (GET)
const getData = async () => {
  try {
    const response = await TeacherService.get();
    if (response.isSuccess) {
      data.teachers = response.data as Teacher[];
      vtTable.value?.refresh();
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

// Fungsi untuk menghapus data mahasiswa (DELETE)
const deleteData = async (teacher: Teacher) => {
  try {
    const response = await TeacherService.delete(teacher.id);
    if (response.isSuccess) {
      ToastService.successToast("Data berhasil dihapus.");
      const index = data.teachers.indexOf(teacher);
      data.teachers.splice(index, 1);
    }
  } catch (error) {
    console.error("Error deleting data:", error);
  }
};

const confirmDelete = (teacher: Teacher) => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin menghapus ${teacher.name} ?`,
    teacher,
    "danger",
    3000,
    "Batal",
    "Hapus"
  ).then(() => {
    deleteData(teacher);
  });
};

const setAdminConfirm = async (target: unknown) => {
  var tg = target as { target: { _modelValue: boolean } }

  try {

    if (tg.target._modelValue === true) {
      const dialogResult = await DialogService.showDialog(`Apakah Anda yakin mengubah status admin ?`,
        teacher, "warning", 3000, "Batal", "Ya");
      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.setAdmin(teacher.profile.userId);
        if (response.isSuccess) {
          teacher.roles.push('Admin');
          ToastService.successToast(`${teacher.profile.name} telah menjadi admin`);
          return;
        }
      }
    } else if (tg.target._modelValue === false) {

      const dialogResult = await DialogService.showDialog(
        `Apakah Anda yakin akan menghapus status admin ?`,
        teacher,
        "warning",
        3000,
        "Batal",
        "Ya"
      );

      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.removeAsAdmin(teacher.profile.userId);
        if (response.isSuccess) {

          teacher.roles.splice(teacher.roles.indexOf('Admin'), 1);
          ToastService.successToast(`${teacher.profile.name} telah menjadi admin`);
          return;
        }
      }
    }
  } catch (error) {
    teacher.isAdmin = !teacher.isAdmin;
  }

};



const lockConfirm = async (target: unknown) => {
  var tg = target as { target: { _modelValue: boolean } }
  try {

    if (tg.target._modelValue === true) {
      const dialogResult = await DialogService.showDialog(`Apakah Anda yakin mengaktifkan user  ?`,
        teacher, "warning", 3000, "Batal", "Ya");
      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.unlockUser(teacher.profile.userId);
        if (response.isSuccess) {
          ToastService.successToast(`${teacher.profile.name} telah aktif`);
          return;
        }
      }
    } else if (tg.target._modelValue === false) {

      const dialogResult = await DialogService.showDialog(
        `Apakah Anda yakin akan menonaktifkan user ?`,
        teacher,
        "warning",
        3000,
        "Batal",
        "Ya"
      );

      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.lockUser(teacher.profile.userId);
        if (response.isSuccess) {

          ToastService.successToast(`${teacher.profile.name} telah dinonaktifkan`);
          return;
        }
      }
    }
  } catch (error) {
    teacher.activated = !teacher.activated;
  }

};


const setAdmin = async (teacher: Teacher) => {
  const response = await AuthService.setAdmin(teacher.userId);
  if (response.isSuccess) {
    ToastService.successToast(`${teacher.name} telah menjadi admin`);
  }
};


const confirmresetPassword = () => {
  DialogService.showDialog(
    `Apakah Anda yakin ingin mereset password untuk user ${teacher.profile.name} ?`,
    teacher,
    "warning",
    3000,
    "Batal",
    "Reset"
  ).then(() => {

    AuthService.resetpasswordByAdmin(teacher.profile.email).then((response) => {
      if (response.isSuccess) {
        ToastService.successToast("Password telah direset");
      } else {
        ToastService.dangerToast("Gagal reset password");
      }
    })

  });
};


onMounted(getData);
</script>
