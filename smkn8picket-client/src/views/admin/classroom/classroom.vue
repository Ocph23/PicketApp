<template>
  <AdminLayout>
    <div>
      <PageHeader title="Data Kelas" />
      <div class="flex justify-between items-center mb-4">
        <div class="flex items-center gap-2">
          <AddIcon class="w-7 h-7" @click="showModalx" />
        </div>
        <div class="flex items-center gap-2">
          <div>
            <div class="flex justify-between gap-2">
              <div class="mb-4">
                <FwbSelect :options="schoolYears" label="Tahun Ajaran" v-model="schoolYearSelected" @change="showData()"
                  required />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Classroom Table -->
      <div class=" shadow-md sm:rounded-lg mt-1">
        <fwb-table>
          <fwb-table-head>
            <fwb-table-head-cell>No</fwb-table-head-cell>
            <fwb-table-head-cell>Nama Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>Tingkat</fwb-table-head-cell>
            <fwb-table-head-cell>Nama Jurusan</fwb-table-head-cell>
            <fwb-table-head-cell>Ketua Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>Wali Kelas</fwb-table-head-cell>
            <fwb-table-head-cell>TA</fwb-table-head-cell>
            <fwb-table-head-cell>Action</fwb-table-head-cell>
          </fwb-table-head>
          <fwb-table-body v-if="classrooms.length > 0">
            <fwb-table-row v-for="(classroom, index) in classrooms.sort((a, b) => a.level - b.level)" :key="index"
              class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
              <fwb-table-cell>{{ index + 1 }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.className }}-{{ classroom.departmentInitial }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.level }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.departmentName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.classLeaderName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.homeRoomTeacherName }}</fwb-table-cell>
              <fwb-table-cell>{{ classroom.year }}/{{ classroom.year + 1 }}</fwb-table-cell>
              <fwb-table-cell class="flex gap-2 items-center justify-start">
                <button v-if="schoolYearActive" @click="editClassroom(classroom)"
                  class="text-black rounded-lg hover:text-slate-500 transition-all duration-200">
                  <EditIcon></EditIcon>
                </button>

                <button v-if="schoolYearActive" @click="deleteData(classroom.id)"
                  class="transition-all duration-200 text-red-500 hover:text-red-700 rounded-lg">
                  <DeleteIcon></DeleteIcon>
                </button>
                <button v-if="!schoolYearActive && classroom.level < 3" @click="confirmCreateNewClassRoom(classroom)"
                  class="text-black rounded-lg hover:text-slate-500 transition-all duration-200">
                  <AddIcon class="w-4 h-4" />
                </button>
                <router-link :to="`/admin/classroom/${classroom.id}`">
                  <InformationCircleIcon class="w-5 h-5 text-blue-500" />
                </router-link>
                <router-link :to="`/admin/classroom/absen/${classroom.id}`">
                  <DetailIcon />
                </router-link>
              </fwb-table-cell>
            </fwb-table-row>
          </fwb-table-body>
        </fwb-table>
      </div>



    </div>

  </AdminLayout>
  <fwb-modal-custome position="center" size="md" not-escapable persistent class="opacity-[99%]" v-if="isShowModal"
    @close="closeModal" ref="mymodal">
    <template #header>
      <div class="flex items-center text-lg text-gray-700 dark:text-gray-100">Tambah Kelas</div>
    </template>
    <template #body>
      <div class="modal-box">
        <form>
          <div class="mb-4">
            <VTInput v-model="form.className" label="Nama Kelas" />
          </div>
          <div class="mb-4">
            <VTInput v-model="form.level" type="number" min="1" max="2" label="Tingkat" />
          </div>
          <div class="mb-4">
            <fwb-select v-model="form.departmentId" label="Jurusan" :options="data.departments" />
          </div>

          <div class="mb-4">
            <AutoComplete placeholder="cari siswa" label="Ketua Kelas" :service="'student'"
              :query="form.classLeaderName" v-model="form.classLeader">
            </AutoComplete>
          </div>
          <div class="mb-4">
            <AutoComplete @on-search="OnSearchTeacher" placeholder="cari wali kelas" label="Wali Kelas"
              :service="'teacher'" :query="form.homeRoomTeacherName" v-model="form.homeRoomTeacher">
            </AutoComplete>
          </div>
        </form>
      </div>
    </template>
    <template #footer>
      <div class="flex justify-end gap-2">
        <fwb-button @click="closeModal" color="alternative"> Batal </fwb-button>
        <fwb-button :disabled="diSableButton" @click="addClassroom" color="green">
          Simpan
        </fwb-button>
      </div>
    </template>
  </fwb-modal-custome>

  <fwb-modal-custome position="center" size="md" not-escapable persistent class="opacity-[99%]"
    v-if="isShowCreateNewClassRoomModal" @close="isShowCreateNewClassRoomModal = false">
    <template #header>
      <div class="flex items-center text-lg text-gray-700 dark:text-gray-100">Tambah Kelas</div>
    </template>
    <template #body>
      <div class="modal-box">
        <form>
          <div class="mb-4">
            <VTInput v-model="form.className" label="Nama Kelas" />
          </div>
          <div class="mb-4">
            <VTInput v-model="form.level" :disabled="true" type="number" min="1" max="2" label="Tingkat" />
          </div>
        </form>
      </div>
    </template>
    <template #footer>
      <div class="flex justify-end gap-2">
        <fwb-button @click="isShowCreateNewClassRoomModal = false" color="alternative"> Batal </fwb-button>
        <fwb-button :disabled="form.className === ''" @click="createClassRoom" color="green">
          Simpan
        </fwb-button>
      </div>
    </template>
  </fwb-modal-custome>
</template>

<style lang="css" scoped>
.h-modal-box {
  max-height: 80vh;
  overflow-y: auto;
  justify-content: center !important;
}
</style>


<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import AdminLayout from '@/components/layouts/AdminLayout.vue'
import FwbModalCustome from '@/components/FwbModalCustome.vue'
import {
  ToastService,
  ClassRoomService,
  DepartmentService,
  TeacherService,
  DialogService,
  SchoolYearService,
} from '@/services'
import { Helper, type ErrorResponse, type RequestResponse } from '@/commons'
import AutoComplete from '@/components/AutoComplete.vue'
import { DeleteIcon, EditIcon, AddIcon, DetailIcon } from '@/components/icons'
import PageHeader from '@/components/PageHeader.vue'
import type { ClassRoomRequest, ClassRoom, Department, Teacher, AutoCompleteSuggestion, SchoolYear } from '@/models'
import AutoCompleteStore from '@/stores/AutoCompleteStore'

import {
  FwbButton,
  FwbInput, FwbSelect,
  FwbTable,
  FwbTableBody,
  FwbTableCell,
  FwbTableHead,
  FwbTableHeadCell,
  FwbTableRow,
} from 'flowbite-vue'
import { InformationCircleIcon } from '@heroicons/vue/24/solid'
import type ClassRoomFromLastClassRequest from '@/models/Requests/ClassRoomFromLastClassRequest'
import { VTInput } from '@ocph23/vtocph23'


const data = reactive({
  errors: [],
  ketuaText: '',
  waliText: '',
  teachers: [],
  students: [],
  departments: [] as { value: string; name: string }[],
})

const autoCompleteStore = AutoCompleteStore();
const schoolYearSelected = ref();
const schoolYearActive = ref();
const schoolYears = ref<{
  name: string;
  value: string;
  actived?: boolean;
}[]>([]);

SchoolYearService.get().then((response: RequestResponse) => {
  const data = response.data as SchoolYear[];
  schoolYears.value = data.map((item) => {
    return {
      value: item.id.toString(),
      name: `${item.name} ${item.semesterName}`,
      actived: item.actived,
    }
  }).reverse();
  const schoolYearActive = data.find((item) => item.actived);
  schoolYearSelected.value = schoolYearActive?.id.toString() || '';
  showData();
})

DepartmentService.get().then((response) => {
  const departments = response.data as Department[]
  data.departments = departments.map((item: Department) => {
    return { value: item.id.toString(), name: item.name }
  })
})


const showData = () => {

  const schoolYear = schoolYears.value.find((item) => item.value == schoolYearSelected.value)
  if (schoolYear) {
    schoolYearActive.value = schoolYear.actived;
  } else {
    schoolYearActive.value = false;
  }

  ClassRoomService.getBySchoolYearId(Number(schoolYearSelected.value)).then((response) => {
    if (response.isSuccess) {
      classrooms.value = response.data as ClassRoom[]
    } else {
      ToastService.dangerToast(Helper.readDetailError(response.error as ErrorResponse))
    }
  })
}


const OnSearchTeacher = (queryText: string) => {
  TeacherService.search(queryText).then((response) => {
    const data = response.data as Teacher[];
    const suggestions = data.map((item) => {
      return { id: item.id, name: item.name, data: item } as AutoCompleteSuggestion
    });
    autoCompleteStore.setTeacher(suggestions);
  })
}

const classrooms = ref([] as ClassRoom[])
const form = ref({
  id: 0,
  className: '',
  departmentName: '',
  departmentInitial: '',
  classLeaderName: '',
  homeRoomTeacherName: '',
  level: 1,
  departmentId: '',
  classLeader: { id: 0 },
  homeRoomTeacher: { id: 0 },
})
const isShowModal = ref(false)
const isShowCreateNewClassRoomModal = ref(false)
const mymodal = ref(null)

function closeModal() {
  isShowModal.value = false
}
function showModalx() {
  isShowModal.value = true
}


const addClassroom = async () => {
  try {
    const requestBody: ClassRoomRequest = {
      id: form.value.id,
      name: form.value.className,
      level: form.value.level,
      departmentId: Number(form.value.departmentId),
      classRommLeaderId: form.value.classLeader.id,
      homeRoomTeacherId: form.value.homeRoomTeacher.id,
    }

    if (!form.value.id) {
      ClassRoomService.post(requestBody).then((response) => {
        if (response.isSuccess) {
          classrooms.value.push(response.data as ClassRoom)
          isShowModal.value = false
          resetForm()
          ToastService.successToast('Data berhasil tambahkan')
        } else {
          ToastService.dangerToast(Helper.readDetailError(response.error as ErrorResponse))
        }
      })
    } else {
      ClassRoomService.put(requestBody.id, requestBody).then((response) => {
        if (response.isSuccess) {
          isShowModal.value = false
          selectedClassRoom.className = form.value.className
          selectedClassRoom.level = form.value.level
          selectedClassRoom.departmentName = form.value.departmentName
          selectedClassRoom.departmentInitial = form.value.departmentInitial
          selectedClassRoom.departmentId = Number(form.value.departmentId)
          selectedClassRoom.classLeaderId = form.value.classLeader.id
          selectedClassRoom.homeRoomTeacherId = form.value.homeRoomTeacher.id
          ToastService.successToast('Data berhasil diubah')
          resetForm()
        } else {
          const err = response.error as ErrorResponse
          ToastService.dangerToast(Helper.readDetailError(err))
        }
      })
    }
  } catch (error) {
    console.error('Error adding classroom:', error)
  }
}

// Function to reset the form
const resetForm = () => {
  form.value = {
    id: 0,
    level: 1,
    className: '',
    departmentName: '',
    departmentInitial: '',
    classLeaderName: '',
    homeRoomTeacherName: '',
    departmentId: '',
    classLeader: { id: 0 },
    homeRoomTeacher: { id: 0 },
  }
}

const diSableButton = computed(() => {
  if (
    !form.value.className ||
    !form.value.departmentId ||
    !form.value.classLeader.id ||
    !form.value.homeRoomTeacher.id
  ) {
    return true
  }
  return false
})


let selectedClassRoom = {} as ClassRoom
const editClassroom = (classRoom: ClassRoom) => {
  selectedClassRoom = classRoom
  isShowModal.value = true
  form.value.id = classRoom.id
  form.value.className = classRoom.className
  form.value.departmentInitial = classRoom.departmentInitial
  form.value.departmentId = classRoom.departmentId.toString()
  form.value.classLeaderName = classRoom.classLeaderName
  form.value.homeRoomTeacherName = classRoom.homeRoomTeacherName
  form.value.classLeader = { id: classRoom.classLeaderId }
  form.value.homeRoomTeacher = { id: classRoom.homeRoomTeacherId }
}

const deleteData = (id: number) => {
  DialogService.showDialog('Yakin hapus data ? ', null, 'danger').then(async () => {
    const deleted = await ClassRoomService.delete(id)
    if (deleted) {
      ToastService.successToast('Data berhasil di hapus.')
    } else {
      ToastService.dangerToast('Data gagal berhasil di hapus.')
    }
  })
}


const confirmCreateNewClassRoom = (classRoom: ClassRoom) => {
  DialogService.showDialog(
    `Apakah Anda yakin membuat kelas baru untuk ${classRoom.className}-${classRoom.departmentInitial} | ${classRoom.departmentName} ?`,
    classRoom,
    'warning',
    3000,
    'Batal',
    'Yakin',
  ).then(() => {
    isShowCreateNewClassRoomModal.value = true
    form.value.id = classRoom.id
    form.value.level = classRoom.level + 1
  })
}

const createClassRoom = () => {
  const formData: ClassRoomFromLastClassRequest = {
    classRoomId: form.value.id,
    name: form.value.className,
    level: form.value.level,
  }

  ClassRoomService.createNewClassFromClassRoom(formData).then((response) => {
    if (response.isSuccess) {
      ToastService.successToast('Kelas baru berhasil dibuat.')
      showData()
      isShowCreateNewClassRoomModal.value = false
      resetForm()
    } else {
      ToastService.dangerToast(Helper.readDetailError(response.error as ErrorResponse))
    }
  })
}

</script>
