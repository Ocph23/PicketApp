import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/


const picketapiUrl = process.env.PIKETAPI_HTTPS || process.env.PIKETAPI_HTTP;
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
    proxy: {
      '/api': {
        target: picketapiUrl,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/api/, ''),
        secure: false
      }
    }
  },
  build: {
    chunkSizeWarningLimit: 1000,
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['vue', 'vue-router', 'pinia'],
          ui: ['@heroicons/vue', 'tailwind-merge'],
          charts: ['apexcharts', 'vue3-apexcharts'],
          utils: ['luxon', '@vuelidate/core', '@vuelidate/validators', '@vueuse/core', 'axios']
        }
      }
    }
  }
})
