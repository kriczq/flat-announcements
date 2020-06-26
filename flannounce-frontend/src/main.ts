import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'

import './styles/styles.scss'

Vue.config.productionTip = false

// eslint-disable-next-line @typescript-eslint/ban-ts-ignore
// @ts-ignore
import * as VueGoogleMaps from 'vue2-google-maps'

Vue.use(VueGoogleMaps, {
  load: {
    key: process.env.VUE_APP_MAPS_API_KEY
  },
  installComponents: true
})

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
