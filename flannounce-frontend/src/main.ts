import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import { config } from 'vuex-module-decorators';

config.rawError = true;

createApp(App)
  .use(router)
  .use(store)
  .mount('#app');
