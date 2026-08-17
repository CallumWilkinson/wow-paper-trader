import { httpClient } from "../../../api/HttpClient";
import type {
  ItemMetadataResponse,
  ItemSearchResponse,
} from "../types/itemSearchTypes";

export async function searchItems(
  itemName: string,
): Promise<ItemSearchResponse[]> {
  const trimmedName = itemName.trim();

  if (!trimmedName) {
    return [];
  }

  const response = await httpClient.get<ItemSearchResponse[]>("/items", {
    params: {
      itemName: trimmedName,
    },
  });

  return response.data;
}

export async function getItemMetadata(
  itemId: number,
): Promise<ItemMetadataResponse> {
  const response = await httpClient.get<ItemMetadataResponse>(
    `/items/${itemId}`,
  );

  return response.data;
}
