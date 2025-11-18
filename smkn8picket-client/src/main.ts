import './assets/main.output.css'
import './assets/index.scss'
import '../node_modules/@ocph23/vtocph23/dist/index.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'

const app = createApp(App)
app.use(createPinia())
app.use(router)

app.mount('#app')
