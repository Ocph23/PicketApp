import type { AuthResponse } from '@/models'
import { ToastService } from '@/services'
import AdminView from '@/views/admin/AdminView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'rooxt',
    component: AdminView,
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/auth/LoginView.vue'),
  },
  {
    path: '/admin',
    name: 'admin',
    redirect: '/admin/home',
    children: [
      {
        path: 'home',
        name: 'home',
        component: () => import('@/views/admin/home/HomeView.vue'),
        meta: { requiresAuth: true },
      },


      // routes tahun ajaran
      {
        path: 'Tahun-ajaran',
        name: 'Tahun-ajaran',
        component: () => import('@/views/admin/shcoolyear/tahunAjaran.vue'),
      },
      {
        path: 'Tahun-ajaran/add',
        name: 'addShoolyear',
        component: () => import('@/views/admin/shcoolyear/addTahunAjaran.vue'),
      },
      {
        path: 'Tahun-ajaran/:id/edit',
        name: 'editSchoolyear',
        component: () => import('@/views/admin/shcoolyear/editTahunAjaran.vue'),
      },

      //routes Departmen
      {
        path: 'Jurusan',
        name: 'Jurusan',
        component: () => import('@/views/admin/jurusan/jurusan.vue'),
      },
      {
        path: 'Jurusan/add',
        name: 'addJurusan',
        component: () => import('@/views/admin/jurusan/addJurusan.vue'),
      },
      {
        path: 'Jurusan/:id/edit',
        name: 'editJurusan',
        component: () => import('@/views/admin/jurusan/editJurusan.vue'),
      },

      // routes student
      {
        path: 'siswa',
        name: 'siswa',
        component: () => import('@/views/admin/student/siswa.vue'),
      },
      {
        path: 'siswa/add',
        name: 'addSiswa',
        component: () => import('@/views/admin/student/addSiswa.vue'),
      },
      {
        path: 'siswa/:id/edit',
        name: 'editSiswa',
        component: () => import('@/views/admin/student/editSiswa.vue'),
      },
      {
        path: 'siswa/:id/detail',
        name: 'detailSiswax',
        component: () => import('@/views/admin/student/detailSiswa.vue'),
      },

      // routes Teacher
      {
        path: 'guru',
        name: 'guru',
        component: () => import('@/views/admin/teacher/guru.vue'),
      },
      {
        path: 'guru/add',
        name: 'addGuru',
        component: () => import('@/views/admin/teacher/addGuru.vue'),
      },
      {
        path: 'guru/:id/edit',
        name: 'editGuru',
        component: () => import('@/views/admin/teacher/editGuru.vue'),
      },
      // routes schedule
      {
        path: 'Jadwal/:id',
        name: 'Jadwal',
        component: () => import('@/views/admin/schedule/JadwalView.vue'),
      },

      // routes Picket
      {
        path: 'hariini',
        name: 'hariini',
        component: () => import('@/views/admin/picket/PicketAdminViewx.vue'),
      },

      {
        path: 'history',
        name: 'history',
        component: () => import('@/views/admin/history/AdminHistory.vue'),
      },
      {
        path: 'history/:id',
        name: 'historydetail',
        component: () => import('@/views/admin/picket/PicketAdminViewx.vue'),
      },
      {
        path: 'jadwalpiket',
        name: 'Jadwalpiket',
        component: () => import('@/views/admin/schedule/JadwalView.vue'),
      },

      // routes Classrom
      {
        path: 'Classroom',
        name: 'classroom',
        component: () => import('@/views/admin/classroom/classroom.vue'),
      },
      {
        path: 'classroom/:id',
        name: 'classroomdetail',
        component: () => import('@/views/admin/classroom/classroomdetail.vue'),
      },
      {
        path: 'classroom/absen/:id',
        name: 'classroomabsen',
        component: () => import('@/views/admin/classroom/classroomabsen.vue'),
      },
      {
        path: 'laporan/absensiswa',
        name: 'laporan_absensiswa',
        component: () => import('@/views/admin/laporan/AbsenSiswa.vue'),
      },
      {
        path: 'laporan/absenguru',
        name: 'laporan_absenguru',
        component: () => import('@/views/admin/laporan/AbsenGuru.vue'),
      },
    ],
  },

  {
    path: '/piket',
    children: [
      // routes Picket -----------------------------------------------------------------------------------------------------
      {
        path: '/piket',
        name: 'piket',
        component: () => import('@/views/pikets/HomeView.vue'),
      },
      {
        path: '/piket/siswa',
        name: 'piket-siswa',
        component: () => import('@/views/pikets/StudentsView.vue'),
      },
      {
        path: 'siswa/:id/detail',
        name: 'detailSiswa',
        component: () => import('@/views/pikets/DetailSiswa.vue'),
      },

      {
        path: '/piket/pikethariini',
        name: 'piketpikethariini',
        component: () => import('@/views/pikets/PiketView.vue'),
      },

      {
        path: '/piket/history',
        name: 'piket-history',
        component: () => import('@/views/pikets/PicketHistory.vue'),
      },
      {
        path: '/piket/history/:id',
        name: 'pikethistorydetail',
        component: () => import('@/views/pikets/PiketView.vue'),
      },

      ////walikelas
    ],
  },

  {
    path: '/walikelas',
    name: 'walikelas',
    redirect: '/walikelas/home',
    children: [
      {

        path: 'home',
        component: () => import('@/views/walikelas/HomeView.vue'),
      },
      {

        path: 'kelas',
        component: () => import('@/views/walikelas/KelasView.vue'),
      },
      {

        path: 'kelas/:id',
        component: () => import('@/views/walikelas/KelasDetailView.vue'),
      },
      {
        path: 'kelas/absen/:id',
        component: () => import('@/views/walikelas/KelasDetailAbsenView.vue'),
      },
      {
        path: 'siswa',
        component: () => import('@/views/walikelas/SiswaView.vue'),
      },
      {
        path: 'siswa/:id',
        component: () => import('@/views/walikelas/SiswaDetail.vue'),
      },
    ],
  },

]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach(async (to) => {
  const token = localStorage.getItem('authToken')
  const auth = JSON.parse(token as string) as AuthResponse
  // const publicPages = ['/login', '/walikelas/login']
  // const walikelasPages = ['/walikelas', '/walikelas/kelas', '/walikelas/siswa']
  // const teacherPiketPages = ['/gurupiket', '/gurupiket/piket', '/gurupiket/siswa']



  if (to.path === '/') {
    if (auth && auth.isAdmin) {
      return '/admin'
    } else if (auth && auth.roles.includes('HomeRoomTeacher')) {
      return '/walikelas/home'
    } else if (auth && auth.roles.includes('PicketTeacher')) {
      return '/piket/pikethariini'
    } else {
      return '/login'
    }
  }

  if (auth && to.matched.find(x => x.path == '/admin')) {
    if (auth.loginAs != 'Administrator') {
      ToastService.dangerToast('Maaf anda tidak memiliki akses ke halaman ini')
      return '/login'
    }
  }


  if (auth && to.matched.find(x => x.path == '/walikelas')) {
    if (auth.loginAs != 'WaliKelas') {
      ToastService.dangerToast('Maaf anda tidak memiliki akses ke halaman ini')
      return '/login'
    }
  }


  if (auth && to.matched.find(x => x.path == '/piket')) {
    if (!auth.isTeacherPicket) {
      ToastService.dangerToast('Maaf anda tidak memiliki akses ke halaman ini')
      return '/login'
    }
  }

})

export default router
