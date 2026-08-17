export interface ItemSearchResponse {
  itemId: number;
  name: string;
  imageUrl: string;
}

export interface ItemMetadataResponse {
  itemId: number;
  unitPrice: number;
  priceTakenAtUtc: string;

  name: string;

  qualityType: string | null;
  qualityName: string | null;

  level: number;
  requiredLevel: number;

  itemClassId: number | null;
  itemClassName: string | null;

  itemSubclassId: number | null;
  itemSubclassName: string | null;

  professionId: number | null;
  professionName: string | null;
  professionSkillLevel: number | null;

  skillDisplayString: string | null;
  craftingReagent: string | null;

  inventoryType: string | null;
  inventoryTypeName: string | null;

  purchasePrice: number | null;
  sellPrice: number | null;

  maxCount: number | null;

  isEquippable: boolean;
  isStackable: boolean;

  purchaseQuantity: number | null;

  imageUrl: string | null;

  metadataLastFetchedUtc: string;
}
