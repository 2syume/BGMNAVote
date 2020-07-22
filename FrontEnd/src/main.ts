import Vue from 'vue'
import App from './App.vue'
import router from './router'
import 'muse-ui/dist/muse-ui.css'
import MuseUI from 'muse-ui'
import AsyncComputed from 'vue-async-computed'

Vue.use(MuseUI)
Vue.use(AsyncComputed)

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
