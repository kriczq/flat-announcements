<template>
  <div class="container pa-10">
    <filters class="all-row" @refresh="refreshAnnounces" />
    <template v-if="announces.length > 0">
      <Announce :announce="a" v-for="a in announces" :key="a.id" />
    </template>
    <div v-else-if="loading" class="all-row" style="text-align: center;">
      <v-icon x-large color="deep-purple accent-4">mdi-home-city-outline</v-icon
      ><span class="ml-5 deep-purple--text text--accent-4"
        >Trwa ładowanie ogłoszeń.</span
      >
    </div>
    <div v-else class="all-row" style="text-align: center;">
      <v-icon x-large color="deep-purple accent-4">mdi-home-city-outline</v-icon
      ><span class="ml-5 deep-purple--text text--accent-4"
        >Brak ogłoszeń spełniających podane kryteria.</span
      >
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Mixins } from 'vue-property-decorator'
import { announceModule } from '@/store/announce'
import { appModule } from '@/store/app'
import Announce from '@/components/Announce.vue'
import Filters from '@/components/Filters.vue'
import { ScrollMixin } from '@/mixins/scroll'

@Component({
  components: {
    Announce,
    Filters
  }
})
export default class AnnouncesList extends Mixins(ScrollMixin) {
  private get announces() {
    return announceModule.announces
  }

  private get loading() {
    return appModule.loading
  }

  private created() {
    if (this.announces.length > 0) return

    this.fetchAnnounces()
  }

  fetchAnnounces() {
    appModule.startLoading()
    announceModule.fetchAnnounces().then(() => {
      appModule.stopLoading()
    })
  }

  refreshAnnounces() {
    appModule.startLoading()
    announceModule.refreshAnnounces().then(() => {
      appModule.startLoading()
    })
  }

  scrolled() {
    if (!appModule.loading) this.fetchAnnounces()
  }
}
</script>

<style lang="scss" scoped>
.container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  row-gap: 3rem;
  column-gap: 3rem;
}

.all-row {
  grid-column: 1 / -1;
}
</style>
