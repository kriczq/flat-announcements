<template>
  <div>
    <v-progress-linear
      v-if="loading"
      class="loading-bar"
      indeterminate
      color="deep-purple accent-4"
    ></v-progress-linear>
    <div class="container pa-10">
      <filters class="all-row" @refresh="refreshAnnounces" />
      <template v-if="announces.length > 0">
        <Announce :announce="a" v-for="a in announces" :key="a.id" />
      </template>
      <div v-else-if="loading" class="all-row" style="text-align: center;">
        <v-icon x-large color="deep-purple accent-4"
          >mdi-home-city-outline</v-icon
        ><span class="ml-5 deep-purple--text text--accent-4"
          >Trwa ładowanie.</span
        >
      </div>
      <div v-else class="all-row" style="text-align: center;">
        <v-icon x-large color="deep-purple accent-4"
          >mdi-home-city-outline</v-icon
        ><span class="ml-5 deep-purple--text text--accent-4"
          >Brak ogłoszeń spełniających podane kryteria.</span
        >
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Mixins } from 'vue-property-decorator'
import { announceModule } from '@/store/announce'
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
  private loading = false

  private get announces() {
    return announceModule.announces
  }

  private created() {
    if (this.announces.length > 0) return

    this.fetchAnnounces()
  }

  fetchAnnounces() {
    this.loading = true
    announceModule.fetchAnnounces().then(() => {
      this.loading = false
    })
  }

  refreshAnnounces() {
        this.loading = true
    announceModule.refreshAnnounces().then(() => {
      this.loading = false
    })
  }

  scrolled() {
    if (!this.loading) this.fetchAnnounces()
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

.loading-bar {
  background-color: white;
  position: fixed;
  top: 60px;
  z-index: 10;
}
</style>
