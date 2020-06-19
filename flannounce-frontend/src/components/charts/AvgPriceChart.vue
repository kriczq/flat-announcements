<script lang="ts">
import { Bar, mixins } from 'vue-chartjs'
import { Component, Mixins } from 'vue-property-decorator'

@Component
export default class AvgPriceChart extends Mixins(Bar, mixins.reactiveProp) {
  private options = {
    layout: {
      padding: {
        left: 30,
        right: 30
      }
    },
    legend: { display: false },
    maintainAspectRatio: false,
    responsive: true,
    scales: {
      yAxes: [
        {
          ticks: {
            beginAtZero: true
          },
          gridLines: { display: false }
        }
      ],
      xAxes: [
        {
          gridLines: { display: false }
        }
      ]
    }
  }

  private mounted() {
    this.renderChart(this.chartData, this.options)
  }
}
</script>
