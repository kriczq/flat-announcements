<template>
  <v-card class="d-flex flex-column">
    <v-img
      style="flex-grow: 0;"
      class="white--text align-end"
      :height="small ? '150px' : '200px'"
      :src="announce.images[0]"
    >
    </v-img>
    <v-card-title class="overflow-hidden">{{ announce.title }}</v-card-title>
    <v-card-text
      class="d-flex flex-column"
      style="flex-grow: 1; padding-bottom: 10px"
    >
      <div class="subtitle-1" :class="{ 'mb-2': !small }">
        <span class="font-weight-bold">{{ announce.price }} zł</span> •
        {{ announce.pricePerSquareMeter }} zł/m² • {{ announce.livingSpace }} m²
      </div>
      <div v-if="!small" class="mb-2 subtitle-1">
        {{ formatLocation(announce) }}
      </div>
      <div class="subtitle-1" :class="{ 'mb-4': !small }">
        {{ formatRooms(announce) }} • {{ formatFloor(announce) }} •
        {{ formatBuildingType(announce) }}
      </div>
      <div style="flex-grow: 1;" />
      <div>
        {{ formatOfferedBy(announce) }}
      </div>
    </v-card-text>
    <v-divider class="mx-4"></v-divider>
    <v-card-actions>
      <v-btn
        text
        color="deep-purple accent-4"
        :href="announce.url"
        target="_blank"
      >
        <v-icon class="mr-2" style="font-size: 16px;">mdi-open-in-new</v-icon
        >Zobacz w olx
      </v-btn>
      <v-spacer />
      <div class="grey--text text-overline">
        dodano
        {{
          daysDiff(announce.createdAt) > 0
            ? daysDiff(announce.createdAt) + ' dni temu'
            : ' dziś'
        }}
      </div>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Prop, Component } from 'vue-property-decorator'
import {
  Announce as TAnnounce,
  BuildingType,
  OfferedBy
} from '@/types/announce'

@Component
export default class Announce extends Vue {
  @Prop()
  private readonly announce!: TAnnounce

  @Prop({ type: Boolean, default: false })
  private readonly small!: boolean

  private daysDiff(date: string) {
    return moment().diff(moment(date), 'days')
  }

  private formatLocation(announce: TAnnounce) {
    const { city, street, district: origDistrict } = announce
    const trimDistrict = (origDistrict ?? '').replace(/\.css-.+{.+}/, '')
    const district = trimDistrict === announce.title ? '' : trimDistrict

    return (
      city + (street ? `, ${street}` : '') + (district ? `, ${district}` : '')
    )
  }

  private formatFloor(announce: TAnnounce) {
    if (['parter', 'suterena', 'poddasze'].includes(announce.floor))
      return announce.floor

    return `piętro: ${announce.floor}`
  }

  private formatBuildingType(announce: TAnnounce) {
    switch (announce.buildingType) {
      case BuildingType.Blok:
        return 'blok mieszkalny'
      case BuildingType.Kamienica:
        return 'kamienica'
      case BuildingType.Apartamentowiec:
        return 'apartamentowiec'
      case BuildingType.Loft:
        return 'loft'
      case BuildingType.Pozostałe:
        return 'nieznany rodzaj budynku'
    }
  }

  private formatOfferedBy(announce: TAnnounce) {
    switch (announce.offeredBy) {
      case OfferedBy.Person:
        return 'Oferta od osoby prywatnej'
      case OfferedBy.Agency:
        return 'Oferta od agencji nieruchomości'
    }
  }

  private formatRooms(announce: TAnnounce) {
    const { rooms } = announce

    if (rooms.length === 1)
      return rooms + (rooms === '1' ? ' pokój' : ' pokoje')

    if (rooms === '4 i więcej') return '4 pokoje'

    return rooms
  }
}
</script>

<style lang="scss" scoped>
::v-deep .v-card__title {
  // text-shadow: 2px 2px 5px rgba(5, 5, 5, 1);
  word-break: initial;
  line-height: initial;
  padding: 10px 16px;
}
</style>
