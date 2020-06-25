import { AnnounceResponse, Announce } from '@/types/announce'
import { Filters } from '@/types/filters'
import { fetchEndpoint, objectToParams } from './fetch'
import { mapLocation } from '@/helpers/map.helpers'

const PAGE_SIZE = 50

const baseFilters = {
  WithImages: true
}

function fetchAnnounces(
  filters: Partial<Filters> = {},
  pageNumber = 1
): Promise<Announce[]> {
  const filtersParams = objectToParams({ ...baseFilters, ...filters })

  return fetchEndpoint({
    path: `announce/?pageNumber=${pageNumber}&pageSize=${PAGE_SIZE}&${filtersParams}`
  })
    .then(response => response.data as AnnounceResponse[])
    .then(announces =>
      announces.map(announce => {
        const { latitude, longitude, ...details } = announce
        return { ...details, location: mapLocation(latitude, longitude) }
      })
    )
}

// function addFlat(flat: IFlat): Promise<IFlat> {
//   return fetchEndpoint({ path: 'flat', method: 'POST', data: flat })
// }

// function updateFlat(flat: IFlat): Promise<IFlat> {
//   return fetchEndpoint({ path: `flat/${flat.flatId}`, method: 'PUT', data: flat })
// }

// function deleteFlat(id: string): Promise<void> {
//   return fetchEndpoint({ path: `flat/${id}`, method: 'DELETE' })
// }

export default {
  fetchAnnounces
}
