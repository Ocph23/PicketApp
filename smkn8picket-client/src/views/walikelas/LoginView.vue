<template>
  <div class="min-h-screen w-full bg-white shadow-md dark:bg-gray-600 flex justify-center items-center">
    <FwbCard class="p-5 w-[50%] md:w-[75%] sm:w-full">
      <form @submit.prevent="login" class="max-w-sm w-full p-6 rounded-lg">
        <div class="flex flex-col items-center justify-center mb-6">
          <img :src="Helper.infoSekolah.logo" class="h-28 w-auto" alt="SMK Logo" />
          <h1 class="mt-2 font-bold">APLIKASI WALI KELAS</h1>
        </div>
        <div class="mb-8">
          <FwbInput label="User Name" v-model="loginRequest.username" required></FwbInput>
        </div>
        <div class="mb-4">
          <FwbInput :type="'password'" label="Password" v-model="loginRequest.password" required></FwbInput>
        </div>
        <div class="flex items-center mb-6">
          <VTInput id="remember" type="checkbox" class="h-4 w-4 " />
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
    </FwbCard>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import AuthService from '@/services/AuthService'
import axios from 'axios'
import type { LoginRequest } from '@/models/Requests'
import type { AuthResponse, Schedule } from '@/models'
import { FwbCard, FwbHeading, FwbInput } from 'flowbite-vue'
import VTInput from '@/commons/VTInput/VTInput.vue'
import { ScheduleService, ToastService } from '@/services'
import { Helper } from '@/commons'

const loginRequest = ref({} as LoginRequest)
const errorMessage = ref('')
const router = useRouter()

const login = async () => {
  try {
    const response = await AuthService.login(loginRequest.value)
    if (response.isSuccess) {
      const auth = response.data as AuthResponse
      auth.isAdmin = false;
      auth.isTeacherPicket = false;
      axios.defaults.headers.common['Authorization'] = auth ? 'Bearer ' + auth.token : ''
      if (auth.roles.includes('HomeRoomTeacher')) {
        auth.isHomeRoomTeacher = true;
        localStorage.setItem('authToken', JSON.stringify(auth))
        router.push('/walikelas/home')
      } else {
        ToastService.dangerToast("Maaf anda belum pernah menjadi wali kelas, silahkan hubungi admin sekolah untuk mengaktifkan akun anda sebagai wali kelas.")
        router.push('/walikelas/login')
      }

    }
  } catch (error) {
    errorMessage.value = 'Login failed. Please check your credentials.'
    console.error(error)
  }
}
</script>
