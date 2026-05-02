import { useQuery } from "@tanstack/react-query";
import { getItemMetadata } from "../api/itemSearchApi";
import type { ItemMetadataResponse } from "../types/itemSearchTypes";

export function useSelectedItem(selectedItemId: number | null) {
  return useQuery<ItemMetadataResponse>({
    queryKey: ["items", "metadata", selectedItemId],
    queryFn: () => {
      return getItemMetadata(selectedItemId as number);
    },
    enabled: selectedItemId !== null,
    staleTime: 30_000,
    retry: 1,
  });
}
