<template>
  <v-card class="d-flex flex-column">
    <v-img
      style="flex-grow: 0;"
      class="white--text align-end"
      height="200px"
      :src="announce.images[0]"
    >
    </v-img>
    <v-card-title>{{ announce.title }}</v-card-title>
    <v-card-text style="flex-grow: 1;">
      <div class="mb-2 subtitle-1">
        <span class="font-weight-bold">{{ announce.price }} zł</span> •
        {{ announce.pricePerSquareMeter }} zł/m² • {{ announce.livingSpace }} m²
      </div>
      <div class="mb-2 subtitle-1">
        {{ announce.city }}{{ announce.street ? `, ${announce.street}` : '' }}
      </div>
      <div class="mb-4 subtitle-1">
        {{ announce.rooms }} • piętro {{ announce.floor }}
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

@Component
export default class Announce extends Vue {
  @Prop()
  private announce!: Announce

  daysDiff(date: string) {
    return moment().diff(moment(date), 'days')
  }
}
</script>

<style lang="scss" scoped>
::v-deep .v-card__title {
  // text-shadow: 2px 2px 5px rgba(5, 5, 5, 1);
  word-break: initial;
}
</style>
