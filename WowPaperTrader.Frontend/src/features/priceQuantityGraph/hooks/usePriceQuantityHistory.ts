import { getPriceHistory } from "../api/priceQuantityApi";
import { useQuery } from "@tanstack/react-query";
import type { MonthlyPriceQuantityResponse } from "../types/priceQuantityTypes";

export function usePriceQuantityHistory(itemId: number | null) {
  return useQuery<MonthlyPriceQuantityResponse>({
    queryKey: ["items", "priceQuantity", itemId],
    queryFn: () => {
      return getPriceHistory(itemId as number);
    },
    enabled: itemId !== null,
    staleTime: 30_000,
    retry: 1,
  });
}
