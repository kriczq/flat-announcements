import { BuildingType, OfferedBy } from './announce'

export interface Filters {
  city: string
  district: string
  rooms: string
  floor: string
  buildingType: BuildingType
  includesFurniture: boolean
  hasCoordinates: boolean
  withImages: boolean
  offeredBy: OfferedBy
  priceMin: number
  priceMax: number
  pricePerSquareMeterMin: number
  pricePerSquareMeterMax: number
  livingSpaceMin: number
  livingSpaceMax: number
  createdAtMin: string
  createdAtMax: string
}

export type FilterName = keyof Filters
