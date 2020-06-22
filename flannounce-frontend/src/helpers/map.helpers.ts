import { Announce } from '@/types/announce'

export const mapLocation = (latitude: string, longitude: string) => ({
  lat: Number.parseFloat(latitude),
  lng: Number.parseFloat(longitude)
})
