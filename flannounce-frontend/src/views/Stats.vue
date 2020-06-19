<template>
  <div class="ma-10">
    <filters class="mb-10" @refresh="fetchData" />
    <v-card
      class="d-flex flex-column"
      height="23rem"
      elevation="10"
      :loading="loading"
    >
      <div class="d-flex justify-space-between my-2 mx-4">
        <v-card-title>
          Średnia cena mieszkań za m²
        </v-card-title>
        <v-select
          v-model="orderAsc"
          :items="orderOptions"
          dense
          style="max-width: 9rem;"
        ></v-select>
      </div>
      <AvgPriceChart
        v-if="chartData.datasets"
        class="overflow-hidden"
        :chartData="chartData"
      />
    </v-card>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import AvgPriceChart from '@/components/charts/AvgPriceChart.vue'
import statsApi from '@/api/stats'
import Filters from '@/components/Filters.vue'
import { filtersModule } from '../store/filters'

@Component({
  components: {
    AvgPriceChart,
    Filters
  }
})
export default class Stats extends Vue {
  private chartData = {}

  private loading = true

  private orderOptions = [
    {
      text: 'od najwyższej',
      value: false
    },
    {
      text: 'od najniższej',
      value: true
    }
  ]

  private orderAsc = false

  private created() {
    this.fetchData()
  }

  @Watch('orderAsc')
  private reload() {
    this.fetchData()
  }

  async fetchData() {
    this.loading = true
    const rawData = await statsApi.fetchAvgPricePerCity(
      this.orderAsc,
      filtersModule.filters
    )
    const labels = rawData.map(data => data.city)
    const data = rawData.map(data => data.averagePrice)

    this.chartData = {
      labels,
      datasets: [
        {
          label: 'Średnia cena',
          backgroundColor: '#00BFA5',
          data
        }
      ]
    }
    this.loading = false
  }
}
</script>

<style lang="scss" scoped>
// .chart-wraper {
//   height: 10rem;
// }
</style>
