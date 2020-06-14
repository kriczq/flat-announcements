<template>
  <div>
    <v-progress-linear
      v-if="loading"
      class="loading-bar"
      indeterminate
      color="deep-purple accent-4"
    ></v-progress-linear>
    <div class="container pa-10">
      <Announce :announce="a" v-for="a in announces" :key="a.id" />
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Prop, Component, Mixins } from 'vue-property-decorator'
import { announceModule } from '@/store/announce'
import Announce from '@/components/Announce.vue'
import { ScrollMixin } from '@/mixins/scroll'

@Component({
  components: {
    Announce
  }
})
export default class AnnouncesList extends Mixins(ScrollMixin) {
  private loading = true

  private get announces() {
    return announceModule.announces
  }

  private created() {
    this.fetchAnnounces()
  }

  fetchAnnounces() {
    this.loading = true
    announceModule.fetchAnnounces().then(() => {
      console.log('end loaidng')
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

.loading-bar {
  position: sticky;
  top: 64px;
  z-index: 10;
}
</style>
