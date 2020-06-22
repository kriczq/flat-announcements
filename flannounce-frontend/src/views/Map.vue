<template>
  <div style="width: 100%; height: 100%; position: relative;">
    <filters
      style="position: absolute; top: 1rem; left: 0; right: 0; margin: 0 auto; width: 75%;"
      auto-collapse
      @refresh="fetchData"
    />
    <GmapMap
      ref="mapRef"
      style="width: 100%; height: 100%"
      :center="{ lat: 51.9189, lng: 19.1344 }"
      :zoom="6"
      :options="{ disableDefaultUI: true }"
    >
      <GmapInfoWindow
        :options="infoOptions"
        :position="infoWindowPos"
        :opened="infoWinOpen"
        @closeclick="infoWinOpen = false"
        ><Announce
          v-if="infoWinOpen && currentAnnounce"
          :announce="currentAnnounce"
        />
      </GmapInfoWindow>
      <GmapMarker
        :key="index"
        v-for="(a, index) in announces"
        :position="a.location"
        :clickable="true"
        @click="toggleInfoWindow(a, index, $event)"
      />
    </GmapMap>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Ref } from 'vue-property-decorator'
import Filters from '@/components/Filters.vue'
import announceApi from '@/api/announce'
import { filtersModule } from '../store/filters'
import { appModule } from '@/store/app'
import { Announce as TAnnounce, AnnounceLocation } from '../types/announce'
import Announce from '@/components/Announce.vue'
import { gmapApi } from 'vue2-google-maps'
import {} from 'googlemaps'

@Component({
  components: {
    Filters,
    Announce
  }
})
export default class Map extends Vue {
  @Ref()
  private readonly mapRef!: any

  private loading = true

  private announces: TAnnounce[] = []

  private created() {
    this.fetchData()
  }

  async fetchData() {
    appModule.startLoading()
    const filters = {
      ...filtersModule.filters,
      hasCoordinates: true
    }
    this.announces = await announceApi.fetchAnnounces(filters)

    const google = await this.mapRef.$gmapApiPromiseLazy()
    const bounds = new google.maps.LatLngBounds()
    for (const a of this.announces) {
      bounds.extend(a.location)
    }
    this.mapRef.fitBounds(bounds)
    appModule.stopLoading()
  }

  private infoWindowPos: AnnounceLocation | null = null
  private infoWinOpen = false
  private infoOptions = {
    pixelOffset: {
      width: 0,
      height: -35
    }
  }

  private currentMidx = -1
  private currentAnnounce: TAnnounce | null = null

  private toggleInfoWindow(announce: TAnnounce, idx: number) {
    this.infoWindowPos = announce.location
    this.currentAnnounce = announce

    //check if its the same marker that was selected if yes toggle
    if (this.currentMidx == idx) {
      this.infoWinOpen = !this.infoWinOpen
    }
    //if different marker set infowindow to open and reset current marker index
    else {
      this.infoWinOpen = true
      this.currentMidx = idx
    }
  }
}
</script>

<style lang="scss" scoped>
// .chart-wraper {
//   height: 10rem;
// }
</style>

<style lang="scss">
button.gm-ui-hover-effect {
  top: 2px !important;
  right: 2px !important;
  width: 30px !important;
  height: 30px !important;
}
button.gm-ui-hover-effect > img {
  width: 24px !important;
  height: 24px !important;
  margin: 3px !important;
}
</style>
