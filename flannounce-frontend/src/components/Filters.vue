<template>
  <v-expansion-panels>
    <v-expansion-panel>
      <v-expansion-panel-header class="font-weight-bold"
        >Filtruj wyniki</v-expansion-panel-header
      >
      <v-expansion-panel-content>
        <div class="filter-items">
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
import { Vue, Component } from 'vue-property-decorator'
import { filtersModule } from '@/store/filters'
import { FilterName, Filters as TFilters } from '@/types/filters'

@Component
export default class Filters extends Vue {
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
.filter-item {
  //   width: 10rem;
}

.filter-items {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(11rem, auto));
  column-gap: 2rem;
  row-gap: 2rem;
}
</style>
