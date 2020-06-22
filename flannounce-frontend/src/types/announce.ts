export type AnnounceType = 'Sale'

export enum BuildingType {
  Blok,
  Kamienica,
  Apartamentowiec,
  Loft,
  Pozostałe
}

export enum OfferedBy {
  Person,
  Agency
}

interface AnnounceDetails {
  id: string
  announceType: AnnounceType
  announceId: number
  title: string
  url: string
  city: string
  street: string | null
  district: string
  description: string | null
  rooms: string
  floor: string
  offeredBy: OfferedBy
  buildingType: BuildingType
  isFromDeveloper: boolean
  includesFurniture: boolean
  price: number
  pricePerSquareMeter: number
  livingSpace: number
  createdAt: string
  scrapedAt: string
  image: string | null
  images: string[]
}

export interface AnnounceLocation {
  lat: number
  lng: number
}

export interface AnnounceResponse extends AnnounceDetails {
  latitude: string
  longitude: string
}

export interface Announce extends AnnounceDetails {
  location: AnnounceLocation
}
