type AnnounceType = 'Sale';

export interface Announcement {
  id: string;
  announceType: AnnounceType;
  announceId: number;
  title: string;
  url: string;
  city: string;
  street: string | null;
  district: string;
  description: string | null;
  rooms: string;
  floor: string;
  buildingType: number;
  isFromDeveloper: boolean;
  includesFurniture: boolean;
  offeredBy: number;
  price: number;
  pricePerSquareMeter: number;
  livingSpace: number;
  createdAt: string;
  scrapedAt: string;
  image: string | null;
}
