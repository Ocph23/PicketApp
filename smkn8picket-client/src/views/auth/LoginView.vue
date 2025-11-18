<template>
  <div class="min-h-screen w-full bg-white shadow-md dark:bg-gray-600 flex justify-center items-center">
    <div class="w-full p-5 m-5">
      <div class="w-full grid grid-cols-1 md:!grid-cols-3 items-center gap-4">
        <div class="col-span-2 hidden md:block">
          <div class="flex flex-col justify-center items-center">
            <img :src="Helper.infoSekolah.logo" class="w-1/5 m-5" alt="SMK Logo" />
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
              SITEM INFORMASI SEKOLAH
            </FwbHeading>
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
              SEKOLAH MENENGAH KEJURUAN NEGERI 8
            </FwbHeading>
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
              TEKNOLOGI INFORMASI DAN KOMUNIKASI
            </FwbHeading>
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2"> KOTA JAYAPURA </FwbHeading>
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2">
              @{{ new Date().getFullYear() }}
            </FwbHeading>
          </div>
        </div>
        <VTCard class="col-span-1 px-5">
          <form @submit.prevent="login" class="w-full">
            <div class="flex justify-center md:hidden">
              <img :src="Helper.infoSekolah.logo" class="w-1/3" alt="SMK Logo" />
            </div>
            <FwbHeading tag="h3" class="w-auto text-xl font-bold mb-2"> Login </FwbHeading>
            <div class="mb-8">
              <VTInput label="User Name" v-model="loginRequest.username" required></VTInput>
            </div>
            <div class="mb-4">
              <VTInput :type="'password'" label="Password" v-model="loginRequest.password" required></VTInput>
            </div>
            <div class="mb-4">
              <FwbSelect :placeholder="'Select Role'" label="Login As" v-model="loginAs" :options="[
                { value: 'Piket', name: 'Piket' },
                { value: 'WaliKelas', name: 'Wali Kelas' },
                { value: 'Administrator', name: 'Administrator' },
              ]" required class="mb-4"></FwbSelect>
            </div>

            <div class="flex items-center mb-6">
              <VTInput id="remember" type="checkbox" class="h-4 w-4" />
              <FwbHeading tag="h6" for="remember" class="ml-2 block text-sm">Remember me</FwbHeading>
            </div>
            <button type="submit"
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
              Login
            </button>
            <p v-if="errorMessage" class="text-red-500 text-center mt-4">
              {{ errorMessage }}
            </p>
          </form>
        </VTCard>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import AuthService from '@/services/AuthService'
import axios from 'axios'
import type { LoginRequest } from '@/models/Requests'
import type { AuthResponse, Schedule } from '@/models'
import { FwbHeading, FwbSelect } from 'flowbite-vue'
import { ScheduleService } from '@/services'
import { Helper, type ErrorResponse } from '@/commons'
import VTInput from '@/components/VTInput/VTInput.vue'
import { VTCard, VTToastService } from '@ocph23/vtocph23'

const loginRequest = ref({} as LoginRequest)
const errorMessage = ref('')
const router = useRouter()

const loginAs = ref('Piket') // Default login type

const login = async () => {
  try {
    const response = await AuthService.login(loginRequest.value)
    if (response.isSuccess) {
      const auth = response.data as AuthResponse
      auth.loginAs = loginAs.value // Store the selected login type in auth
      axios.defaults.headers.common['Authorization'] = auth ? 'Bearer ' + auth.token : ''
      if (loginAs.value === 'Piket') {
        if (auth.profile == null || auth.profile.id == null) {
          VTToastService.error('Maaf anda belum terdaftar sebagai guru')
          return
        }
        const response = await ScheduleService.get()
        if (response.isSuccess) {
          const schedules = response.data as Schedule[]
          const date = new Date()
          const dayOfWeek = new Intl.DateTimeFormat('en-US', { weekday: 'long' }).format(date)
          if (schedules.find((x) => x.teacherId == auth.profile.id && x.dayOfWeek === dayOfWeek)) {
            auth.isTeacherPicket = true
            localStorage.setItem('authToken', JSON.stringify(auth))
            router.push('/piket')
          } else {
            VTToastService.error('Maaf anda tidak sedang piket hari ini')
            return
          }
        }
      }

      if (loginAs.value === 'WaliKelas') {
        if (auth.roles.includes('HomeRoomTeacher')) router.push('/walikelas')
        else {
          VTToastService.error('Maaf anda belum terdaftar sebagai wali kelas')
          return
        }
      }

      if (loginAs.value === 'Administrator') {
        if (auth.roles.includes('Admin')) {
          auth.isAdmin = true
          localStorage.setItem('authToken', JSON.stringify(auth))
          router.push('/')
        } else {
          VTToastService.error('Maaf anda bukan administrator')
          return
        }
      }
      localStorage.setItem('authToken', JSON.stringify(auth))
    } else {
      const err = response.error as ErrorResponse

      VTToastService.error(err.detail || 'Periksa kembali username dan password')
    }
  } catch (error) {
    const err = error as ErrorResponse
    VTToastService.error(err?.detail || err?.message || 'Login failed')
  }
}
</script>
