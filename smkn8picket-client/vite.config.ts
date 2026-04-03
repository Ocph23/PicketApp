import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/


const picketapiUrl = process.env.PIKETAPI_HTTPS || process.env.PIKETAPI_HTTP
console.log("picketapiurl: " + picketapiUrl)
process.env.VITE_API_URL = picketapiUrl;

export default defineConfig({
  plugins: [
    vue(),
    vueJsx(),
    vueDevTools(),
  ],

  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  server: {
    host: 'localhost',
    port: 5173,
    strictPort: true,
    proxy: {
      '/api': {
        target: picketapiUrl,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/api/, ''),
        secure: false
      }
    }
  }
})
