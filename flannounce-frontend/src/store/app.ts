import { getModule, VuexModule, Module, Mutation } from 'vuex-module-decorators'

import store from '@/store'

@Module({
  dynamic: true,
  namespaced: true,
  name: 'appModule',
  store
})
export default class AppModule extends VuexModule {
  public loading = false

  @Mutation
  startLoading() {
    this.loading = true
  }

  @Mutation
  stopLoading() {
    this.loading = false
  }
}

export const appModule = getModule(AppModule, store)
