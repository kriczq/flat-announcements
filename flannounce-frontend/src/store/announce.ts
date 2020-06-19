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
import { Filters } from '@/types/filters'

@Module({
  dynamic: true,
  namespaced: true,
  name: 'announcesModule',
  store
})
export default class AnnounceModule extends VuexModule {
  public announces: Announce[] = []

  private pageNumber = 1

  @Action
  fetchAnnounces() {
    const filters = this.context.rootState.filtersModule.filters as Partial<
      Filters
    >
    return announceApi
      .fetchAnnounces(filters, this.pageNumber)
      .then(announces => {
        this.addAnnounces(announces)
        this.nextPage()
      })
  }

  @Action
  refreshAnnounces() {
    this.resetPage()
    this.clearAnnounces()

    return this.fetchAnnounces()
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
  clearAnnounces() {
    this.announces = []
  }

  @Mutation
  nextPage() {
    this.pageNumber += 1
  }

  @Mutation
  resetPage() {
    this.pageNumber = 1
  }
}

export const announceModule = getModule(AnnounceModule, store)
