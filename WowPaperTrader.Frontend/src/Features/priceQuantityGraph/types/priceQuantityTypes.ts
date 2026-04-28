export interface MonthlyPriceQuantityResponse {
  itemId: number;
  priceQuantityResponses: PriceQuantityResponse[];
}

export interface PriceQuantityResponse {
  commodityAuctionSnapshotId: number | null;
  fetchedAtUtc: string | null;
  lowestUnitPrice: number | null;
  totalQuantityPosted: number | null;
}
