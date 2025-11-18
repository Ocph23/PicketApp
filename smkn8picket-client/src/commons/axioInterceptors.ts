import { VTToastService } from '@ocph23/vtocph23'
import axios, { AxiosError, type AxiosResponse } from 'axios'

const authString = localStorage.getItem('authToken')
const auth = JSON.parse(authString!)
axios.defaults.baseURL = import.meta.env.VITE_API_URL + '/api'
axios.defaults.headers.common['Content-Type'] = 'application/json'
axios.defaults.headers.common['Authorization'] = auth ? 'Bearer ' + auth.token : ''
axios.defaults.headers.post['Access-Control-Allow-Origin'] = '*';
axios.defaults.headers.put['Access-Control-Allow-Origin'] = '*';

const onNotFound = () => {
  VTToastService.error('Alamat URL Tidak Ditemukan.')
}

const onError = () => {
  VTToastService.error(
    'Maaf terjadi kesalahan. Coba Uangi lagi/Silahkan Hubungi administrator.',
  )
}

axios.interceptors.response.use(
  function (response) {
    // Optional: Do something with response data
    return response
  },
  function (err) {
    try {
      const axiosError = err as AxiosError
      if (axiosError.code === 'ERR_NETWORK') {
        VTToastService.error('Koneksi ke server terputus/tidak ada. Periksa koneksi internet atau server Anda.')
        return err
      }


      const axiosResponse = axiosError.response as AxiosResponse
      if (axiosResponse) {
        if (axiosResponse.status == 401) {
          // VTToastService.danger(axiosError.message)
          return err
        }

        if (axiosResponse.status == 404) {
          onNotFound()
          return err
        }
        if (axiosResponse.status == 405) {
          onError()
          return err
        }

        if (axiosResponse.status == 503) {
          onError()
          // VTToastService.danger(response.messages.err);
          // const router = useRouter();
          // router.push(`/error-page${axiosResponse.status}`);
          return err
        }
        if (axiosResponse.status == 500) {
          onError()
          return err
        }
        return err
      } else {
        console.log(axiosError.message)
        throw new Error('Maaf terjadi kesalahan...')
      }
    } catch (error) {
      const err = error as Error
      VTToastService.error(err.message)
    }
  },
)
