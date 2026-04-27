export interface MonthlyPriceQuantityResponse {
  itemId: number;
  priceQuantityResponses: Array<PriceQuantityResponse>;
}

interface PriceQuantityResponse {
  commodityAuctionSnapshotId: number;
  fetchedAtUtc: string;
  lowestUnitPrice: number;
  totalQuantityPosted: number;
}
