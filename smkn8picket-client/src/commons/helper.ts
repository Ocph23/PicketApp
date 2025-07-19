import type { AxiosError, AxiosResponse } from 'axios'
import { type ErrorDetail, type ErrorResponse, type RequestResponse } from '@/commons'
import { DateTime } from 'luxon';

const Helper = {
  getResult: (response: unknown): RequestResponse => {
    if (!response) {
      return { data: null, isSuccess: false, error: null }
    }

    const res = response as AxiosResponse;
    if (res && res.status === 200)
      return { data: res.data, isSuccess: true, error: null }
    else {
      const x = response as AxiosError
      return { data: null, isSuccess: false, error: x.response?.data as ErrorResponse }
    }
  },

  genders: [
    { value: '1', name: 'Perempuan' },
    { value: '0', name: 'Laki-laki' },
  ],

  getAttendanceStatus: (value: number, isInitial: boolean = false) => {
    switch (value) {
      case 0:
        return isInitial ? '' : 'None'
      case 1:
        return isInitial ? 'H' : 'Hadir'
      case 2:
        return isInitial ? 'T' : 'Terlambat'
      case 3:
        return isInitial ? '-' : 'Alpa'
      case 4:
        return isInitial ? 'S' : 'Sakit'
      case 5:
        return isInitial ? 'I' : 'Izin'
      case 6:
        return isInitial ? 'B' : 'Bolos'
      case 7:
        return isInitial ? 'TK' : 'Lainnya'
      default:
        return isInitial ? '' : 'None'
    }
  },
  studentStatus: (status: number) => {
    switch (status) {
      case 0:
        return 'Aktif'
      case 1:
        return 'Pindah'
      case 2:
        return 'Tamat'
      case 3:
        return 'Keluar'
      case 4:
        return 'Tanpa Status'
      default:
        return 'Aktif';
    }
  },

  getWeartherString: (value: number) => {
    switch (value) {
      case 0:
        return 'Cerah'
      case 1:
        return 'Mendung'
      case 2:
        return 'Gerimis'
      case 3:
        return 'Hujan'
      default:
        return 'Cerah'
    }
  },

  readDetailError: (error: ErrorResponse): string => {
    let message: string = ''
    if ('detail' in error) {
      message = error.detail
    }
    return message
  },

  readError: (errors: ErrorDetail[], propName: string): string => {
    if (errors && propName) {
      const result = errors.find((x) => x.code === propName)
      return result ? result.description : ''
    }
    return ''
  },

  getErrorStatus: (errors: ErrorDetail[], propName: string) => {
    if (errors && propName) {
      const result = errors.find((x) => x.code === propName)
      if (result) return 'error'
    }
  },
  getIndonesiaDay: (value: number) => {
    switch (value) {
      case 0:
        return 'Minggu'
      case 1:
        return 'Senin'
      case 2:
        return 'Selasa'
      case 3:
        return 'Rabu'
      case 4:
        return 'Kamis'
      case 5:
        return 'Jumat'
      case 6:
        return 'Sabtu'
      default:
        return value
    }
  },

  getDateTimeString: (date: Date, format: string) => {
    return DateTime.fromJSDate(date).toFormat(format)
  },

  getTeacherAvatar: (photo: string) => {
    if (photo) return `${import.meta.env.VITE_API_URL}/photos/teacher/${photo}`
    return '/man.png'
  },
  getStudentAvatar: (photo: string) => {
    if (photo) return `${import.meta.env.VITE_API_URL}/photos/student/${photo}`
    return '/man.png'
  },
  fileToBase64: (file: Blob) => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader()
      reader.readAsDataURL(file)
      reader.onload = () => {
        const base64String = (reader.result as string).split(',')[1]
        resolve(base64String)
      }
      reader.onerror = (error) => reject(error)
    })
  },
  infoSekolah: {
    nama_sekolah: import.meta.env.VITE_NAMA_SEKOLAH,
    alamat_sekolah: import.meta.env.VITE_ALAMAT_SEKOLAH,
    no_telp: import.meta.env.VITE_NO_TELP,
    email: import.meta.env.VITE_EMAIL,
    kota: import.meta.env.VITE_KOTA,
    provinsi: import.meta.env.VITE_PROVINSI,
    kode_pos: import.meta.env.VITE_KODE_POS,
    logo: import.meta.env.VITE_LOGO,
  }
}

export default Helper
