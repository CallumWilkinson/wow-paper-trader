export interface MonthlyPriceQuantityResponse {
  itemId: number;
  priceQuantityResponses: PriceQuantityResponse[];
}

interface PriceQuantityResponse {
  commodityAuctionSnapshotId: number;
  fetchedAtUtc: string;
  lowestUnitPrice: number;
  totalQuantityPosted: number;
}
