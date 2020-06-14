import {
  getModule,
  VuexModule,
  Module,
  Mutation,
  Action
} from 'vuex-module-decorators'

import store from '@/store'
import { Announce } from '@/types/announce'
import announceApi from '@/api/announce'

@Module({
  dynamic: true,
  namespaced: true,
  name: 'Announces',
  store
})
export default class AnnounceModule extends VuexModule {
  public announces: Announce[] = []

  private pageNumber = 1

  @Action
  fetchAnnounces() {
    return announceApi.fetchAnnounces(this.pageNumber).then(announces => {
      this.addAnnounces(announces)
      this.nextPage()
    })
  }

  @Mutation
  addAnnounce(announce: Announce) {
    this.announces.push(announce)
  }

  @Mutation
  addAnnounces(announces: Announce[]) {
    this.announces.push(...announces)
  }

  @Mutation
  nextPage() {
    this.pageNumber += 1
  }
}

export const announceModule = getModule(AnnounceModule, store)
