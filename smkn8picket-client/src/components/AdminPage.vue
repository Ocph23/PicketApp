<template>
  <div v-if="!isLoginPage">
    <aside style="background-image: url('/bgside.webp'); background-size: cover;" id="logo-sidebar" class="fixed shadow-md shadow-gray-800 top-0 left-0 z-40 w-64 h-screen  transition-transform -translate-x-full
       border-r border-gray-200 sm:translate-x-0 dark:bg-gray-800 dark:border-gray-700" aria-label="Sidebar">
      <div class="h-full  p-4 overflow-y-auto bg-gray-800/60">
        <div class="my-5 flex  justify-center ">
          <img :src="Helper.infoSekolah.logo" class="h-12 me-4" alt="Smk Logo" />
        </div>
        <ul class="space-y-2 font-medium">
          <router-link :to="{ name: 'home' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg class="w-6 h-6 " xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                  <path
                    d="M543.8 287.6c17 0 32-14 32-32.1c1-9-3-17-11-24L512 185l0-121c0-17.7-14.3-32-32-32l-32 0c-17.7 0-32 14.3-32 32l0 36.7L309.5 7c-6-5-14-7-21-7s-15 1-22 8L10 231.5c-7 7-10 15-10 24c0 18 14 32.1 32 32.1l32 0 0 69.7c-.1 .9-.1 1.8-.1 2.8l0 112c0 22.1 17.9 40 40 40l16 0c1.2 0 2.4-.1 3.6-.2c1.5 .1 3 .2 4.5 .2l31.9 0 24 0c22.1 0 40-17.9 40-40l0-24 0-64c0-17.7 14.3-32 32-32l64 0c17.7 0 32 14.3 32 32l0 64 0 24c0 22.1 17.9 40 40 40l24 0 32.5 0c1.4 0 2.8 0 4.2-.1c1.1 .1 2.2 .1 3.3 .1l16 0c22.1 0 40-17.9 40-40l0-16.2c.3-2.6 .5-5.3 .5-8.1l-.7-160.2 32 0z" />
                </svg>
                <span class="ms-3">Home</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'Tahun-ajaran' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg class="w-6 h-6 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24"
                  height="24" fill="currentColor" viewBox="0 0 24 24">
                  <path fill-rule="evenodd"
                    d="M5 5a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1h1a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1h1a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1 2 2 0 0 1 2 2v1a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V7a2 2 0 0 1 2-2ZM3 19v-7a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2Zm6.01-6a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm2 0a1 1 0 1 1 2 0 1 1 0 0 1-2 0Zm6 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm-10 4a1 1 0 1 1 2 0 1 1 0 0 1-2 0Zm6 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm2 0a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    clip-rule="evenodd" />
                </svg>

                <span class="ms-3">Tahun Ajaran</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'Jurusan' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg
                  class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                  xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512">
                  <path
                    d="M96 128a128 128 0 1 1 256 0A128 128 0 1 1 96 128zM0 482.3C0 383.8 79.8 304 178.3 304l91.4 0C368.2 304 448 383.8 448 482.3c0 16.4-13.3 29.7-29.7 29.7L29.7 512C13.3 512 0 498.7 0 482.3zM625 177L497 305c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L591 143c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z" />
                </svg>

                <span class="flex-1 ms-3 whitespace-nowrap">Jurusan</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'Guru' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg
                  class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                  xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512">
                  <path
                    d="M192 96a48 48 0 1 0 0-96 48 48 0 1 0 0 96zm-8 384l0-128 16 0 0 128c0 17.7 14.3 32 32 32s32-14.3 32-32l0-288 56 0 64 0 16 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-16 0 0-64 192 0 0 192-192 0 0-32-64 0 0 48c0 26.5 21.5 48 48 48l224 0c26.5 0 48-21.5 48-48l0-224c0-26.5-21.5-48-48-48L368 0c-26.5 0-48 21.5-48 48l0 80-76.9 0-65.9 0c-33.7 0-64.9 17.7-82.3 46.6l-58.3 97c-9.1 15.1-4.2 34.8 10.9 43.9s34.8 4.2 43.9-10.9L120 256.9 120 480c0 17.7 14.3 32 32 32s32-14.3 32-32z" />
                </svg>

                <span class="flex-1 ms-3 whitespace-nowrap">Guru</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'Siswa' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg
                  class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                  xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                  <path
                    d="M219.3 .5c3.1-.6 6.3-.6 9.4 0l200 40C439.9 42.7 448 52.6 448 64s-8.1 21.3-19.3 23.5L352 102.9l0 57.1c0 70.7-57.3 128-128 128s-128-57.3-128-128l0-57.1L48 93.3l0 65.1 15.7 78.4c.9 4.7-.3 9.6-3.3 13.3s-7.6 5.9-12.4 5.9l-32 0c-4.8 0-9.3-2.1-12.4-5.9s-4.3-8.6-3.3-13.3L16 158.4l0-71.8C6.5 83.3 0 74.3 0 64C0 52.6 8.1 42.7 19.3 40.5l200-40zM111.9 327.7c10.5-3.4 21.8 .4 29.4 8.5l71 75.5c6.3 6.7 17 6.7 23.3 0l71-75.5c7.6-8.1 18.9-11.9 29.4-8.5C401 348.6 448 409.4 448 481.3c0 17-13.8 30.7-30.7 30.7L30.7 512C13.8 512 0 498.2 0 481.3c0-71.9 47-132.7 111.9-153.6z" />
                </svg>
                <span class="flex-1 ms-3 whitespace-nowrap">Siswa</span>
              </span>
            </li>
          </router-link>

          <router-link :to="{ name: 'Picket' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg class="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
                  width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
                  <path fill-rule="evenodd"
                    d="M5 5a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1h1a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1h1a1 1 0 0 0 1-1 1 1 0 1 1 2 0 1 1 0 0 0 1 1 2 2 0 0 1 2 2v1a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V7a2 2 0 0 1 2-2ZM3 19v-7a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2Zm6.01-6a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm2 0a1 1 0 1 1 2 0 1 1 0 0 1-2 0Zm6 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm-10 4a1 1 0 1 1 2 0 1 1 0 0 1-2 0Zm6 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm2 0a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    clip-rule="evenodd" />
                </svg>
                <span class="flex-1 ms-3 whitespace-nowrap">Piket</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'classroom' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 64 64" id="classroom" width="24" height="24">
                  <path d="M5 60h14a1 1 0 0 0 1-1 8 8 0 0 0-16 0 1 1 0 0 0 1 1Z"></path>
                  <circle cx="12" cy="45" r="5"></circle>
                  <path d="M24 59a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1 8 8 0 0 0-16 0Z"></path>
                  <circle cx="32" cy="45" r="5"></circle>
                  <path d="M52 51a8 8 0 0 0-8 8 1 1 0 0 0 1 1h14a1 1 0 0 0 1-1 8 8 0 0 0-8-8Z"></path>
                  <circle cx="52" cy="45" r="5"></circle>
                  <circle cx="13" cy="9" r="5"></circle>
                  <path
                    d="M5 27h3v10a1 1 0 0 0 1 1h3V28a1 1 0 0 1 2 0v10h3a1 1 0 0 0 1-1V21h7a1 1 0 0 0 1-1v-4a1 1 0 0 0-1-1H8a4 4 0 0 0-4 4v7a1 1 0 0 0 1 1Z">
                  </path>
                  <path
                    d="M59 4H21a1 1 0 0 0-1 1v9h5a2 2 0 0 1 2 2v4a2 2 0 0 1-2 2h-5v9a1 1 0 0 0 1 1h38a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1Zm-6 22H43a1 1 0 0 1 0-2h10a1 1 0 0 1 0 2Zm0-7H29a1 1 0 0 1 0-2h24a1 1 0 0 1 0 2Zm0-7H27a1 1 0 0 1 0-2h26a1 1 0 0 1 0 2Z">
                  </path>
                </svg>
                <span class="flex-1 ms-3 whitespace-nowrap">Kelas</span>
              </span>
            </li>
          </router-link>
          <router-link :to="{ name: 'history' }">
            <li>
              <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
                <svg
                  class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                  xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                  <path
                    d="M75 75L41 41C25.9 25.9 0 36.6 0 57.9L0 168c0 13.3 10.7 24 24 24l110.1 0c21.4 0 32.1-25.9 17-41l-30.8-30.8C155 85.5 203 64 256 64c106 0 192 86 192 192s-86 192-192 192c-40.8 0-78.6-12.7-109.7-34.4c-14.5-10.1-34.4-6.6-44.6 7.9s-6.6 34.4 7.9 44.6C151.2 495 201.7 512 256 512c141.4 0 256-114.6 256-256S397.4 0 256 0C185.3 0 121.3 28.7 75 75zm181 53c-13.3 0-24 10.7-24 24l0 104c0 6.4 2.5 12.5 7 17l72 72c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-65-65 0-94.1c0-13.3-10.7-24-24-24z" />
                </svg>

                <span class="flex-1 ms-3 whitespace-nowrap">Riwayat Piket</span>

              </span>
            </li>
          </router-link>
          <li @click="logout" class="cursor-pointer">
            <span class="flex items-center p-2  rounded-lg hover:bg-gray-400/75 group">
              <svg
                class="flex-shrink-0 w-5 h-5 text-gray-800 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
                aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 18 16">
                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M1 8h11m0 0L8 4m4 4-4 4m4-11h3a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2h-3" />
              </svg>

              <span class="flex-1 ms-3 whitespace-nowrap">Keluar</span>
            </span>
          </li>
        </ul>
      </div>
    </aside>

    <div class="min-h-screen  pt-20 sm:ml-64 bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
      <nav
        class="fixed pl-3 top-0 z-50 w-full  bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
        <div class="px-3 py-3 lg:px-5 lg:pl-3">
          <div class="flex items-center justify-between">
            <div class="flex items-center justify-start rtl:justify-end">
              <button data-drawer-target="logo-sidebar" data-drawer-toggle="logo-sidebar" aria-controls="logo-sidebar"
                type="button"
                class="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600">
                <span class="sr-only">Open sidebar</span>
                <svg class="w-6 h-6" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
                  xmlns="http://www.w3.org/2000/svg">
                  <path clip-rule="evenodd" fill-rule="evenodd"
                    d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z">
                  </path>
                </svg>
              </button>
              <div>
                <router-link :to="{ name: 'home' }">
                  <span class="flex ms-2 md:me-24">
                    <span
                      class="self-center text-xl font-semibold sm:text-xl whitespace-nowrap text-black dark:text-white sm:inline hidden">
                      APLIKASI PIKET
                    </span>
                  </span>
                </router-link>

                <ThemeDark></ThemeDark>
              </div>
            </div>

            <div class="flex items-center">
              <div class="flex items-center ms-3">
                <div>
                  <button type="button"
                    class="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
                    aria-expanded="false" data-dropdown-toggle="dropdown-user">
                    <span class="sr-only">Open user menu</span>
                    <img class="w-8 h-8 rounded-full"
                      src="https://flowbite.com/docs/images/people/profile-picture-5.jpg" alt="user photo" />
                  </button>
                </div>
                <div
                  class="z-50 hidden my-4 text-base list-none bg-white divide-y divide-gray-100 rounded shadow dark:bg-gray-700 dark:divide-gray-600"
                  id="dropdown-user">
                  <div class="px-4 py-3" role="none">
                    <p class="text-sm text-gray-900 dark:text-white" role="none">
                      fidel
                    </p>
                    <p class="text-sm font-medium text-gray-900 truncate dark:text-gray-300" role="none">
                      fidel@gmail.com
                    </p>
                  </div>
                  <ul class="py-1" role="none">
                    <li>
                      <a href="#"
                        class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
                        role="menuitem">Home</a>
                    </li>
                    <li>
                      <a href="#"
                        class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-600 dark:hover:text-white"
                        role="menuitem">Sign out</a>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </nav>
      <div style="padding: 20px;">
        <slot></slot>
      </div>
    </div>
  </div>


</template>
<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
// Periksa apakah route saat ini adalah '/login'
const isLoginPage = computed(() => route.path === "/login");

import { useRouter } from "vue-router";
import ThemeDark from "./ThemeDark.vue";
import { Helper } from "@/commons";
const router = useRouter();
const logout = () => {
  const confirmed = window.confirm("Apakah Anda yakin ingin Keluar?");
  if (confirmed) {
    localStorage.removeItem("authToken");
    router.push("/login");
  }
};
</script>
<style lang="css" scoped>
span,
svg {
  color: white;
  fill: white;
}
</style>
