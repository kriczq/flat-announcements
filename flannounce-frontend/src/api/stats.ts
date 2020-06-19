import { Filters } from '@/types/filters'
import { fetchEndpoint, objectToParams } from './fetch'
import { AvgPriceData } from '@/types/stats'

const MAX_COUNT = 100

function fetchAvgPricePerCity(
  orderAsc = false,
  filters: Partial<Filters> = {}
): Promise<AvgPriceData> {
  const filtersParams = objectToParams(filters)

  return fetchEndpoint({
    path: `avgPricePerCity?count=${MAX_COUNT}&sortAsc=${orderAsc}&${filtersParams}`
  })
}

export default {
  fetchAvgPricePerCity
}
