import { useQuery } from "@tanstack/react-query";
import { searchItems } from "../api/itemSearchApi";
import type { ItemSearchResponse } from "../types/itemSearchTypes";

interface UseItemSearchOptions {
  searchTerm: string;
}

export function useItemSearch(options: UseItemSearchOptions) {
  const { searchTerm } = options;

  return useQuery<ItemSearchResponse[]>({
    queryKey: ["items", "search", searchTerm],
    queryFn: () => {
      return searchItems(searchTerm);
    },
    enabled: searchTerm.trim().length > 0,
    staleTime: 30_000,
    retry: 1,
  });
}
