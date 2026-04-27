import { getPriceHistory } from "../api/priceQuantityApi";
import { useQuery } from "@tanstack/react-query";
import type { MonthlyPriceQuantityResponse } from "../types/priceQuantityTypes";

export function usePriceQuantityHistory(itemId: number | null) {
  return useQuery<MonthlyPriceQuantityResponse>({
    queryKey: ["price-quantity-history", itemId],
    queryFn: () => {
      return getPriceHistory(itemId!);
    },
    enabled: itemId !== null,
  });
}
