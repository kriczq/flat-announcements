<template>
  <v-card>
    <v-card-title>{{ announce.title }}</v-card-title>
    <v-card-text>
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
      <div v-if="announce.description" class="text--primary">
        {{ announce.description }}
      </div>
    </v-card-text>
    <v-card-actions>
      <v-btn
        text
        color="deep-purple accent-4"
        :href="announce.url"
        target="_blank"
      >
        Zobacz w olx
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
