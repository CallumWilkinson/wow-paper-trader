import { httpClient } from "../../../api/HttpClient";
import type { MonthlyPriceQuantityResponse } from "../types/priceQuantityTypes";

export async function getPriceHistory(
  itemId: number,
): Promise<MonthlyPriceQuantityResponse> {
  const response = await httpClient.get<MonthlyPriceQuantityResponse>(
    `items/${itemId}/price-history`,
  );

  return response.data;
}
