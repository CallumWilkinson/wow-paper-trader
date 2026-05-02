const goldIconUrl =
  "https://render.worldofwarcraft.com/us/icons/56/inv_misc_coin_02.jpg";
const silverIconUrl =
  "https://render.worldofwarcraft.com/us/icons/56/inv_misc_coin_04.jpg";
const copperIconUrl =
  "https://render.worldofwarcraft.com/us/icons/56/inv_misc_coin_06.jpg";

export interface UnitPricePart {
  amount: number;
  iconUrl: string;
}

function validateUnitPrice(unitPrice: number) {
  if (!Number.isInteger(unitPrice) || unitPrice < 0) {
    throw new Error("unitPrice must be a non-negative integer");
  }
}

export function getUnitPriceParts(unitPrice: number): UnitPricePart[] {
  validateUnitPrice(unitPrice);

  const gold = Math.floor(unitPrice / 10_000);
  const silver = Math.floor((unitPrice % 10_000) / 100);
  const copper = unitPrice % 100;

  const parts: UnitPricePart[] = [];

  if (gold > 0) {
    parts.push({
      amount: gold,
      iconUrl: goldIconUrl,
    });
  }

  if (silver > 0) {
    parts.push({
      amount: silver,
      iconUrl: silverIconUrl,
    });
  }

  if (copper > 0 || parts.length === 0) {
    parts.push({
      amount: copper,
      iconUrl: copperIconUrl,
    });
  }

  return parts;
}

export function formatUnitPriceAriaLabel(unitPrice: number): string {
  validateUnitPrice(unitPrice);

  const gold = Math.floor(unitPrice / 10_000);
  const silver = Math.floor((unitPrice % 10_000) / 100);
  const copper = unitPrice % 100;

  return `${gold} gold ${silver} silver ${copper} copper`;
}
