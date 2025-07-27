<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <nav class="fixed top-0 z-50 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
    <div class="px-3 py-3 lg:px-5 lg:pl-3">
      <div class="flex items-center justify-between">
        <div class="flex items-center justify-start rtl:justify-end body">
          <button @click="clickMenu" type="button"
            class="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600">
            <span class="sr-only">Open sidebar</span>
            <svg class="w-6 h-6" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg">
              <path clip-rule="evenodd" fill-rule="evenodd"
                d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z">
              </path>
            </svg>
          </button>
          <a href="/admin" class="flex ms-2 md:me-24">
            <LogoApp class=" mr-2 w-8 h-8" />
            <span class="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap dark:text-white">
              {{ Helper.infoSekolah.nama_sekolah }}
            </span>
          </a>
        </div>
        <div class="flex">

          <fwb-dropdown align-to-end text="Bottom" close-inside>
            <template #trigger>
              <FwbHeading class="text-sm  cursor-pointer">Welcome, {{ user.name }}</FwbHeading>
            </template>
            <nav class="w-44 flex flex-col py-2 text-sm text-gray-700 dark:text-gray-200">
              <span @click="showChangePasswordModal"
                class="cursor-pointer px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Ubah
                Password</span>
              <span @click="logout"
                class="cursor-pointer px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Logout</span>
            </nav>
          </fwb-dropdown>

        </div>
      </div>
    </div>
  </nav>


  <FwbModal :size="'md'" @close="changePasswordModal = false" v-if="changePasswordModal" :persistent="true">
    <template #header>
      <FwbHeading tag="h3" class="text-lg pb-3 font-bold">
        Ubah Password
      </FwbHeading>
    </template>

    <template #body>

      <div class="mb-4">
        <VTInput label="Password Baru" type="password" v-model="form.password"></VTInput>
      </div>

      <div class="mb-4">
        <VTInput label="Confirm Password" type="password" v-model="form.password_confirmation"></VTInput>
      </div>


    </template>

    <template #footer>
      <div class="flex justify-end gap-1">
        <FwbButton color="alternative" @click="changePasswordModal = false">Batal</FwbButton>
        <FwbButton @click="changePassword">Simpan</FwbButton>
      </div>
    </template>
  </FwbModal>


</template>


<script setup lang="ts">
import { Helper } from '@/commons';
import LogoApp from '@/components/LogoApp.vue';
import AuthService from '@/services/AuthService';
import { FwbHeading, FwbDropdown, FwbButton, FwbToggle, FwbModal } from 'flowbite-vue';
import { onMounted, ref } from 'vue';
import FwbModalCustome from '../FwbModalCustome.vue';
import VTInput from '../VTInput/VTInput.vue';
import { ToastService } from '@/services';

const emit = defineEmits(['onClickMenu', 'onClickLogout'])

const changePasswordModal = ref(false);
const user = ref({ name: '' })
const userTheme = ref('');
const isDark = ref(false);


const form = ref({
  userName: '',
  password: '',
  password_confirmation: '',
  old_password: '',
  token: ''
});

const clickMenu = () => {
  emit('onClickMenu');
}


const logout = () => {
  emit('onClickLogout');
}


onMounted(() => {

  const initUserTheme = getTheme() || getMediaPreference();
  isDark.value = initUserTheme == 'dark-theme' ? true : false;
  setTheme(initUserTheme);
  AuthService.getUser().then((res) => {
    user.value.name = !res.profile ? res.userName : res.profile.name;
  })


})


const setTheme = (theme: string) => {
  localStorage.setItem("user-theme", theme);
  userTheme.value = theme;

  if (theme === "dark-theme") {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }

}
const getMediaPreference = () => {
  const hasDarkPreference = window.matchMedia(
    "(prefers-color-scheme: dark)"
  ).matches;
  if (hasDarkPreference) {
    return "dark-theme";
  } else {
    return "light-theme";
  }
}

const getTheme = () => {
  return localStorage.getItem("user-theme");
}




const toggleTheme = () => {
  const activeTheme = localStorage.getItem("user-theme");
  if (activeTheme === "light-theme") {
    setTheme("dark-theme");
  } else {
    setTheme("light-theme");
  }
}


const showChangePasswordModal = async () => {

  form.value.password = '';
  form.value.password_confirmation = '';
  form.value.old_password = '';


  const user = await AuthService.getUser();
  if (!user) {
    return;
  }

  form.value.userName = user.email;
  AuthService.resetpassword().then((res) => {
    if (res.isSuccess) {
      form.value.token = res.data as string;
      changePasswordModal.value = true;
    }
  })

}




const changePassword = () => {
  try {

    if (form.value.password !== form.value.password_confirmation) {
      throw new Error("Password tidak sama")
    }

    AuthService.changePassword(form.value.userName, form.value.token, form.value.password).then((res) => {
      if (res.isSuccess) {
        ToastService.successToast("Password berhasil diubah");
        changePasswordModal.value = false;
      }
    })
  } catch (error) {
    const err = error as Error;
    ToastService.dangerToast(err.message);
  }

}


</script>



<style lang="css" scoped>
:root {
  --background-color-primary: #ebebeb;
  --background-color-secondary: #fafafa;
  --accent-color: #cacaca;
  --text-primary-color: #222;
  --element-size: 4rem;
  /* <- this is the base size of our element */
}


.switch-checkbox {
  display: none;
}


.switch-label {
  /* for width, use the standard element-size */
  width: var(--element-size);

  /* for other dimensions, calculate values based on it */
  border-radius: var(--element-size);
  border: calc(var(--element-size) * 0.025) solid var(--accent-color);
  padding: calc(var(--element-size) * 0.1);
  font-size: calc(var(--element-size) * 0.3);
  height: calc(var(--element-size) * 0.35);

  align-items: center;
  background: var(--text-primary-color);
  cursor: pointer;
  display: flex;
  position: relative;
  transition: background 0.5s ease;
  justify-content: space-between;
  z-index: 1;
}

.switch-toggle {
  position: absolute;
  background-color: var(--background-color-primary);
  border-radius: 50%;
  top: calc(var(--element-size) * 0.07);
  left: calc(var(--element-size) * 0.07);
  height: calc(var(--element-size) * 0.4);
  width: calc(var(--element-size) * 0.4);
  transform: translateX(0);
  transition: transform 0.3s ease, background-color 0.5s ease;
}

.switch-toggle-checked {
  transform: translateX(calc(var(--element-size) * 0.6)) !important;
}
</style>
