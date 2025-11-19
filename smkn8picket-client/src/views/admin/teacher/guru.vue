<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <AdminLayout>
    <VTCard title="Data Guru & Staff">
      <template #rightSide>
        <AddIcon class="w-7 h-7" @click="showModal = true" />
      </template>
      <VTTable ref="vtTable" :table-name="'tabel-guru'" :bordered="true" :source="data.teachers" :columns="[
        { title: 'No', name: 'no', sortable: false, type: 'Custome' },
        { title: 'Nama', propName: 'name', isMobileHeader: true },
        { title: 'NUPTK', propName: 'registerNumber' },
        {
          title: 'Jenis Kelamin',
          propName: 'gender',
          name: 'gender',
          type: 'Custome',
        },
        { title: 'TTL', propName: 'dateOfBorn' },
        { title: 'Email', propName: 'email' },
        { title: 'Jabatan', propName: 'job', name: 'job', type: 'Custome' },
        {
          title: 'Jenis Pegawai',
          propName: 'jobStatus',
          name: 'jobStatus',
          type: 'Custome',
        },
        { title: 'Aksi', name: 'actions', sortable: false, type: 'Custome' },
      ] as VTTableColumn[]
        ">
        <template #no="row">
          {{ row.index + 1 }}
        </template>
        <template #actions="row">
          <div class="flex items-center gap-2">
            <button @click="editTeacher(row.data)" class="text-white flex">
              <EditIcon></EditIcon>
            </button>
            <button @click="confirmDelete(row.data)" class="text-white flex cursor-pointer">
              <DeleteIcon />
            </button>
            <button @click="selectTeacher(row.data)" class="text-white flex cursor-pointer">
              <LockClosedIcon class="w-5 h-5 text-sky-600 hover:text-sky-900" />
            </button>
          </div>
        </template>
        <template #gender="row">
          {{Helper.genders.find((x) => x.value === row.data.gender)?.name}}
        </template>
        <template #job="row">
          {{Helper.jobs.find((x) => x.value === row.data.job)?.name}}
        </template>
        <template #jobStatus="row">
          {{Helper.jobStatus.find((x) => x.value === row.data.jobStatus)?.name}}
        </template>
      </VTTable>
    </VTCard>
  </AdminLayout>

  <VTModal class="modal opacity-[99%]" v-if="modal" :size="'xl'" @close="modal = false" v-model="modal"
    :persistent="true">
    <template #header>
      <h3 class="text-lg pb-3 font-bold">Account</h3>
    </template>

    <template #body>
      <table class="w-full text-sm text-left">
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
              <FwbCheckbox v-model:model-value="teacher.activated" @change="lockConfirm($event)" />
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Admin</td>
            <td class="p-2">
              <FwbCheckbox v-model="teacher.isAdmin" @change="setAdminConfirm($event)">
              </FwbCheckbox>
            </td>
          </tr>
          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Roles</td>
            <td class="p-2">
              {{ teacher.roles }}
            </td>
          </tr>

          <tr class="!border-0 !border-b-1 !border-b-gray-200">
            <td class="p-2">Password</td>
            <td class="p-2">
              <FwbButton @click="confirmresetPassword" class="bg-red-400">
                Reset Password</FwbButton>
            </td>
          </tr>
        </tbody>
      </table>

      <form class="flex justify-end mt-5 gap-3">
        <FwbButton :color="'alternative'" @click="modal = false">Keluar</FwbButton>
      </form>
    </template>
  </VTModal>

  <VTModal v-if="showModal" :size="'5xl'" @close="() => (showModal = false)" persistent>
    <template #header>
      <h3 class="text-lg font-medium leading-6 text-gray-900">Tambah/Edit Jurusan</h3>
    </template>
    <template #body>
      <div class="overflow-y-auto max-h-[80vh]">
        <form @submit.prevent="saveData">
          <div class="grid gap-6 mb-6 md:grid-cols-2">
            <div class="mb-4">
              <VTInput label="NUPT/NIP" type="text" v-model="data.form.registerNumber" required />
            </div>
            <div class="mb-4">
              <VTInput label="Nama" type="text" v-model="data.form.name" required />
            </div>
            <div class="mb-4">
              <VTSelect placeholder="pilih jenis kelamin" :options="Helper.genders" label="Jenis Kelamin"
                v-model="data.form.gender" required />
            </div>

            <div class="mb-4">
              <VTInput label="Tempat Lahir" type="text" v-model="data.form.placeOfBorn" required />
            </div>

            <div class="mb-4">
              <VTInput v-model="data.form.dateOfBorn" :type="'date'" label="Tanggal Lahir" required></VTInput>
            </div>
            <div class="mb-4">
              <VTInput label="Email" type="email" v-model="data.form.email" required />
            </div>
            <div class="mb-4">
              <VTSelect :options="Helper.jobs" placeholder="pilih jabatan" label="Jabatan" v-model="data.form.job"
                required />
            </div>

            <div class="mb-4">
              <VTSelect :options="Helper.jobStatus" label="Jenis Pegawai" placeholder="pilih jenis pegawai"
                v-model="data.form.jobStatus" required />
            </div>
          </div>
          <div class="mb-4">
            <VTTextArea label="Alamat" placeholder="Alamat" v-model="data.form.address" />
          </div>
          <div class="mb-4 flex justify-end gap-2">
            <VTButton color="red" @click="
              () => {
                showModal = false
              }
            ">Batal</VTButton>
            <VTButtonSave type="submit">Simpan</VTButtonSave>
          </div>
        </form>
      </div>
    </template>
  </VTModal>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import { TeacherService } from '@/services'
import { Helper, type ErrorResponse, type RequestResponse } from '@/commons'
import { EditIcon, DeleteIcon, AddIcon } from '@/components/icons'
import { FwbButton, FwbCheckbox } from 'flowbite-vue'
import { LockClosedIcon } from '@heroicons/vue/20/solid'
import AuthService from '@/services/AuthService'
import {
  VTButton,
  VTButtonSave,
  VTCard,
  VTDialogService,
  VTInput,
  VTModal,
  VTSelect,
  VTTable,
  VTTextArea,
  VTToastService,
  type VTTableColumn,
} from '@ocph23/vtocph23'
import type { Teacher } from '@/models'

const vtTable = ref<InstanceType<typeof VTTable> | null>(null)
const modal = ref(false)
const showModal = ref(false)
const teacher = reactive<{
  profile: Teacher
  activated: boolean
  isAdmin: boolean
  roles: string[]
}>(
  {} as {
    profile: Teacher
    activated: boolean
    isAdmin: boolean
    roles: string[]
  },
)

const data = reactive({
  teachers: [] as Teacher[],
  form: {} as Teacher,
})

const selectTeacher = (item: Teacher) => {
  teacher.profile = item

  AuthService.getStatus(item.userId).then((response: RequestResponse) => {
    const data = response.data as {
      activated: boolean
      isAdmin: boolean
      roles: string[]
    }
    teacher.activated = data.activated
    teacher.isAdmin = data.isAdmin
    teacher.roles = data.roles
    modal.value = true
  })
}

// Fungsi untuk mengambil data guru/ (GET)
const getData = async () => {
  try {
    const response = await TeacherService.get()
    if (response.isSuccess) {
      data.teachers = response.data as Teacher[]
      vtTable.value?.refresh()
    }
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}

// Fungsi untuk menghapus data mahasiswa (DELETE)
const deleteData = async (teacher: Teacher) => {
  try {
    const response = await TeacherService.delete(teacher.id)
    if (response.isSuccess) {
      VTToastService.success('Data berhasil dihapus.')
      const index = data.teachers.indexOf(teacher)
      data.teachers.splice(index, 1)
      vtTable.value?.refresh()
    }
  } catch (error) {
    console.error('Error deleting data:', error)
  }
}

const confirmDelete = (teacher: Teacher) => {
  VTDialogService.asyncShowDialog(
    'Perhatian',
    `Apakah Anda yakin ingin menghapus ${teacher.name} ?`,
    teacher,
    'danger',
    'Batal',
    'Hapus',
  ).then(() => {
    deleteData(teacher)
  })
}

const setAdminConfirm = async (target: unknown) => {
  const tg = target as { target: { _modelValue: boolean } }
  try {
    if (tg.target._modelValue === true) {
      const dialogResult = await VTDialogService.asyncShowDialog(
        'Perhatian',
        `Apakah Anda yakin mengubah status admin ?`,
        teacher,
        'warning',
        'Batal',
        'Ya',
      )
      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.setAdmin(teacher.profile.userId)
        if (response.isSuccess) {
          teacher.roles.push('Admin')
          VTToastService.success(`${teacher.profile.name} telah menjadi admin`)
          return
        }
      }
    } else if (tg.target._modelValue === false) {
      const dialogResult = await VTDialogService.asyncShowDialog(
        'Perhatian',
        `Apakah Anda yakin akan menghapus status admin ?`,
        teacher,
        'warning',
        'Batal',
        'Ya',
      )

      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.removeAsAdmin(teacher.profile.userId)
        if (response.isSuccess) {
          teacher.roles.splice(teacher.roles.indexOf('Admin'), 1)
          VTToastService.success(`${teacher.profile.name} telah menjadi admin`)
          return
        }
      }
    }
  } catch {
    teacher.isAdmin = !teacher.isAdmin
  }
}

const lockConfirm = async (target: unknown) => {
  const tg = target as { target: { _modelValue: boolean } }
  try {
    if (tg.target._modelValue === true) {
      const dialogResult = await VTDialogService.asyncShowDialog(
        'Perhatian',
        `Apakah Anda yakin mengaktifkan user  ?`,
        teacher,
        'warning',
        'Batal',
        'Ya',
      )
      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.unlockUser(teacher.profile.userId)
        if (response.isSuccess) {
          VTToastService.success(`${teacher.profile.name} telah aktif`)
          return
        }
      }
    } else if (tg.target._modelValue === false) {
      const dialogResult = await VTDialogService.asyncShowDialog(
        'Perhatian',
        `Apakah Anda yakin akan menonaktifkan user ?`,
        teacher,
        'warning',
        'Batal',
        'Ya',
      )

      if (dialogResult as unknown as Teacher) {
        const response = await AuthService.lockUser(teacher.profile.userId)
        if (response.isSuccess) {
          VTToastService.success(`${teacher.profile.name} telah dinonaktifkan`)
          return
        }
      }
    }
  } catch {
    teacher.activated = !teacher.activated
  }
}

// const setAdmin = async (teacher: Teacher) => {
//   const response = await AuthService.setAdmin(teacher.userId)
//   if (response.isSuccess) {
//     VTToastService.success(`${teacher.name} telah menjadi admin`)
//   }
// }

const confirmresetPassword = () => {
  VTDialogService.asyncShowDialog(
    'Perhatian',
    `Apakah Anda yakin ingin mereset password untuk user ${teacher.profile.name} ?`,
    teacher,
    'warning',
    'Batal',
    'Reset',
  ).then(() => {
    AuthService.resetpasswordByAdmin(teacher.profile.email).then((response) => {
      if (response.isSuccess) {
        VTToastService.success('Password telah direset')
      } else {
        VTToastService.error('Gagal reset password')
      }
    })
  })
}

const editTeacher = (teacher: Teacher) => {
  data.form = teacher
  showModal.value = true
}

const saveData = async () => {
  try {
    if (data.form.id) {
      const response = await TeacherService.put(data.form.id, data.form)
      if (response.isSuccess) {
        VTToastService.success('Data berhasil diubah')
        vtTable.value?.refresh()
        data.form = {} as Teacher
        showModal.value = false
      } else {
        const err = response.error as ErrorResponse
        VTToastService.error(Helper.readDetailError(err))
      }
    } else {
      const response = await TeacherService.post(data.form)
      if (response.isSuccess) {
        VTToastService.success('Data berhasil ditambahkan')
        data.teachers.push(response.data as Teacher)
        vtTable.value?.refresh()
        data.form = {} as Teacher
        showModal.value = false
      } else {
        const err = response.error as ErrorResponse
        VTToastService.error(Helper.readDetailError(err))
      }
    }
  } catch {
    VTToastService.error('Terjadi kesalahan saat menambahkan data')
  }
}

onMounted(getData)
</script>
