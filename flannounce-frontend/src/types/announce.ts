type AnnounceType = 'Sale'

enum BuildingType {
  Raz,
  Dwa,
  Trzy,
  Cztery
}

enum OfferedBy {
  Person,
  Agency
}

export interface Announce {
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
  buildingType: BuildingType
  isFromDeveloper: boolean
  includesFurniture: boolean
  offeredBy: OfferedBy
  price: number
  pricePerSquareMeter: number
  livingSpace: number
  createdAt: string
  scrapedAt: string
  image: string | null
}
