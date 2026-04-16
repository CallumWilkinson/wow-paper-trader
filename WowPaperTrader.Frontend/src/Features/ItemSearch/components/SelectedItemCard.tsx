import {
  Box,
  Card,
  CardContent,
  CircularProgress,
  Stack,
  Typography,
} from "@mui/material";
import type { ItemMetadataResponse } from "../Types/ItemSearchTypes";

interface SelectedItemCardProps {
  item: ItemMetadataResponse | undefined;
  isLoading: boolean;
  isError: boolean;
}

function formatUnitPrice(unitPrice: number): string {
  if (!Number.isInteger(unitPrice) || unitPrice < 0) {
    throw new Error("unitPrice must be a non-negative integer");
  }

  const gold = Math.floor(unitPrice / 10_000);
  const silver = Math.floor((unitPrice % 10_000) / 100);
  const copper = unitPrice % 100;

  const parts: string[] = [];

  if (gold > 0) {
    parts.push(`${gold}g`);
  }

  if (silver > 0) {
    parts.push(`${silver}s`);
  }

  if (copper > 0 || parts.length === 0) {
    parts.push(`${copper}c`);
  }
  return parts.join(" ");
}
