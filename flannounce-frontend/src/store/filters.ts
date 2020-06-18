import { getModule, VuexModule, Module, Mutation } from 'vuex-module-decorators'

import store from '@/store'
import { Filters, FilterName } from '@/types/filters'
import Vue from 'vue'

@Module({
  dynamic: true,
  namespaced: true,
  name: 'filtersModule',
  store
})
export default class FiltersModule extends VuexModule {
  public filters: Partial<Filters> = {}

  @Mutation
  setFilter<K extends FilterName>({
    key,
    value
  }: {
    key: K
    value: Filters[K]
  }) {
    if (!value && value !== false) return Vue.delete(this.filters, key)
    Vue.set(this.filters, key, value)
    console.log(this.filters)
  }

  @Mutation
  clearFilter(key: keyof Filters) {
    Vue.delete(this.filters, key)
    console.log(this.filters)
  }

  @Mutation
  clearAllFilters() {
    this.filters = {}
    // Object.keys(this.filters).forEach(key => Vue.delete(this.filters, key))
  }
}

export const filtersModule = getModule(FiltersModule, store)
