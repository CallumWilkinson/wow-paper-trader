import { useQuery } from "@tanstack/react-query";
import { searchItems } from "../api/itemsApi";
import { ItemSearchResult } from "../types/itemTypes";

interface UseItemSearchOptions {
  searchTerm: string;
  hasSubmittedSearch: boolean;
}

export function useItemSearch(options: UseItemSearchOptions) {
  const { searchTerm, hasSubmittedSearch } = options;

  return useQuery<ItemSearchResult[]>({
    queryKey: ["items", "search", searchTerm],
    queryFn: () => searchItems(searchTerm),
    enabled: hasSubmittedSearch && searchTerm.trim().length > 0,
    staleTime: 30_000,
    retry: 1,
  });
}
