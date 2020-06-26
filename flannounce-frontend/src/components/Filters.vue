<template>
  <v-expansion-panels hover v-model="openConverted">
    <v-expansion-panel>
      <v-expansion-panel-header class="font-weight-bold"
        ><span
          ><v-icon class="mr-2" style="font-size: 20px; color: black;"
            >mdi-filter-outline</v-icon
          >Filtruj wyniki</span
        ></v-expansion-panel-header
      >
      <v-expansion-panel-content>
        <div class="filter-items mt-2">
          <div class="filter-item">
            <div class="text-subtitle-2">Miasto</div>
            <v-text-field
              clearable
              placeholder="Wszystkie"
              :value="filters.city"
              @change="setFilter('city', $event)"
            />
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Rodzaj zabudowy</div>
            <v-select
              clearable
              :items="buildingOptions"
              :value="filters.buildingType"
              placeholder="Wszystkie"
              @change="setFilter('buildingType', $event)"
            ></v-select>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Rodzaj ogłoszenia</div>
            <v-select
              clearable
              :items="offeredByOptions"
              :value="filters.offeredBy"
              placeholder="Wszystkie"
              @change="setFilter('offeredBy', $event)"
            ></v-select>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Umeblowane</div>
            <v-select
              clearable
              :items="furnitureOptions"
              :value="filters.includesFurniture"
              placeholder="Wszystkie"
              @change="setFilter('includesFurniture', $event)"
            ></v-select>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Piętro</div>
            <v-select
              clearable
              :items="floorOptions"
              :value="filters.floor"
              placeholder="Wszystkie"
              @change="setFilter('floor', $event)"
            ></v-select>
          </div>
                    <div class="filter-item">
            <div class="text-subtitle-2">Liczba pokoi</div>
            <v-select
              clearable
              :items="roomsOptions"
              :value="filters.rooms"
              placeholder="Wszystkie"
              @change="setFilter('rooms', $event)"
            ></v-select>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Cena</div>
            <div class="d-flex align-center">
              <v-text-field
                type="number"
                placeholder="od"
                :value="filters.priceMin"
                @change="setFilter('priceMin', $event)"
              /><span class="ml-1">zł</span>
              <span class="mx-3">&ndash;</span>
              <v-text-field
                type="number"
                placeholder="do"
                :value="filters.priceMax"
                @change="setFilter('priceMax', $event)"
              /><span class="ml-1">zł</span>
            </div>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Cena za m²</div>
            <div class="d-flex align-center">
              <v-text-field
                type="number"
                placeholder="od"
                :value="filters.pricePerSquareMeterMin"
                @change="setFilter('pricePerSquareMeterMin', $event)"
              /><span class="ml-1">zł</span>
              <span class="mx-3">&ndash;</span>
              <v-text-field
                type="number"
                placeholder="do"
                :value="filters.pricePerSquareMeterMax"
                @change="setFilter('pricePerSquareMeterMax', $event)"
              /><span class="ml-1">zł</span>
            </div>
          </div>
          <div class="filter-item">
            <div class="text-subtitle-2">Powierzchnia</div>
            <div class="d-flex align-center">
              <v-text-field
                type="number"
                placeholder="od"
                :value="filters.livingSpaceMin"
                @change="setFilter('livingSpaceMin', $event)"
              /><span class="ml-1">m²</span>
              <span class="mx-3">&ndash;</span>
              <v-text-field
                type="number"
                placeholder="do"
                :value="filters.livingSpaceMax"
                @change="setFilter('livingSpaceMax', $event)"
              /><span class="ml-1">m²</span>
            </div>
          </div>
        </div>
        <div class="float-right">
          <v-btn
            class="mr-3"
            text
            color="deep-purple accent-4"
            @click="clearAllFilters"
          >
            Wyczyść
          </v-btn>
          <v-btn outlined color="deep-purple accent-4" @click="refresh">
            Filtruj
          </v-btn>
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
  </v-expansion-panels>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { filtersModule } from '@/store/filters'
import { FilterName, Filters as TFilters } from '@/types/filters'

@Component
export default class Filters extends Vue {
  @Prop({ type: Boolean, default: false })
  private readonly autoCollapse!: boolean

  @Prop({ type: Boolean, default: false })
  private readonly startOpen!: boolean

  private open = this.startOpen

  private get openConverted() {
    return this.open ? 0 : undefined
  }

  private set openConverted(val: 0 | undefined) {
    this.open = val === 0
  }

  private get filters() {
    return filtersModule.filters
  }

  private setFilter<K extends FilterName>(key: K, value: TFilters[K]) {
    filtersModule.setFilter({ key, value })
  }

  private clearFilter = filtersModule.clearFilter

  private clearAllFilters() {
    filtersModule.clearAllFilters()
    this.refresh()
  }

  private refresh() {
    if (this.autoCollapse) this.open = false
    this.$emit('refresh')
  }

  private offeredByOptions = [
    {
      text: 'Prywatne',
      value: 'private'
    },
    {
      text: 'Agencja / deweloper',
      value: 'agency'
    }
  ]

  private furnitureOptions = [
    {
      text: 'Tak',
      value: true
    },
    {
      text: 'Nie',
      value: false
    }
  ]

  private buildingOptions = [
    {
      text: 'Blok',
      value: 0
    },
    {
      text: 'Kamienica',
      value: 1
    },
    {
      text: 'Apartamentowiec',
      value: 2
    },
    {
      text: 'Loft',
      value: 3
    },
    {
      text: 'Pozostałe',
      value: 4
    }
  ]

  private roomsOptions = [
    {
      text: '1 pokój',
      value: '1 pokój'
    },
    {
      text: '2 pokoje',
      value: '2 pokoje'
    },
    {
      text: '3 pokoje',
      value: '3 pokoje'
    },
    {
      text: '4 lub więcej',
      value: '4 i więcej'
    }
  ]

  private floorOptions = [
    {
      text: 'Suterena',
      value: 'suterena'
    },
    {
      text: 'Parter',
      value: 'parter'
    },
    ...new Array(10)
      .fill(undefined)
      .map((_, idx) => ({ text: idx + 1, value: idx + 1 })),
    {
      text: 'Powyżej 10',
      value: 'powyżej 10'
    },
    {
      text: 'Poddasze',
      value: 'poddasze'
    }
  ]
}
</script>

<style lang="scss" scoped>
.filter-items {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(11rem, auto));
  column-gap: 2rem;
  row-gap: 2rem;
}
</style>
