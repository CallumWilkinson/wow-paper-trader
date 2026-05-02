import {
  Box,
  Card,
  CardContent,
  Chip,
  Divider,
  Skeleton,
  Stack,
  Typography,
} from "@mui/material";
import AutoGraphRoundedIcon from "@mui/icons-material/AutoGraphRounded";
import CategoryRoundedIcon from "@mui/icons-material/CategoryRounded";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import type { ItemMetadataResponse } from "../types/itemSearchTypes";
import { useAutoRefresh } from "../../../hooks/useAutoRefresh";
import { formatPriceTimestamp } from "../../../utils/formatPriceTimestamp";
import { formatUnitPrice } from "../../../utils/formatUnitPrice";

interface SelectedItemCardProps {
  item: ItemMetadataResponse | undefined;
  isLoading: boolean;
  isError: boolean;
}

interface DetailRow {
  label: string;
  value: string;
}

function buildDetailRows(item: ItemMetadataResponse): DetailRow[] {
  const rows: Array<DetailRow | null> = [
    item.qualityName
      ? {
          label: "Quality",
          value: item.qualityName,
        }
      : null,
    item.level > 0
      ? {
          label: "Item level",
          value: item.level.toString(),
        }
      : null,
    item.requiredLevel > 0
      ? {
          label: "Required level",
          value: item.requiredLevel.toString(),
        }
      : null,
    item.itemClassName
      ? {
          label: "Class",
          value: item.itemClassName,
        }
      : null,
    item.itemSubclassName
      ? {
          label: "Subclass",
          value: item.itemSubclassName,
        }
      : null,
    item.professionName
      ? {
          label: "Profession",
          value: item.professionSkillLevel
            ? `${item.professionName} (${item.professionSkillLevel})`
            : item.professionName,
        }
      : null,
    item.inventoryTypeName
      ? {
          label: "Inventory type",
          value: item.inventoryTypeName,
        }
      : null,
    item.skillDisplayString
      ? {
          label: "Skill",
          value: item.skillDisplayString,
        }
      : null,
    item.craftingReagent
      ? {
          label: "Crafting reagent",
          value: item.craftingReagent,
        }
      : null,
    item.maxCount !== null
      ? {
          label: "Max count",
          value: item.maxCount.toString(),
        }
      : null,
    item.purchaseQuantity !== null
      ? {
          label: "Vendor quantity",
          value: item.purchaseQuantity.toString(),
        }
      : null,
    item.purchasePrice !== null
      ? {
          label: "Vendor price",
          value: formatUnitPrice(item.purchasePrice),
        }
      : null,
    item.sellPrice !== null
      ? {
          label: "Vendor sell",
          value: formatUnitPrice(item.sellPrice),
        }
      : null,
  ];

  return rows.filter((row): row is DetailRow => row !== null);
}

export default function SelectedItemCard(props: SelectedItemCardProps) {
  const { item, isLoading, isError } = props;

  useAutoRefresh();

  if (isLoading) {
    return (
      <Card component="section">
        <CardContent sx={{ p: { xs: 3, md: 4 } }}>
          <Stack spacing={3}>
            <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
              <Skeleton
                variant="rounded"
                width={120}
                height={120}
                sx={{ flexShrink: 0 }}
              ></Skeleton>
              <Stack spacing={1.5} sx={{ flexGrow: 1 }}>
                <Skeleton variant="text" width="45%" height={48}></Skeleton>
                <Skeleton variant="text" width="68%"></Skeleton>
                <Skeleton variant="text" width="38%"></Skeleton>
                <Stack direction="row" spacing={1}>
                  <Skeleton variant="rounded" width={88} height={32}></Skeleton>
                  <Skeleton
                    variant="rounded"
                    width={104}
                    height={32}
                  ></Skeleton>
                </Stack>
              </Stack>
              <Skeleton variant="rounded" width={220} height={160}></Skeleton>
            </Stack>

            <Divider></Divider>

            <Box
              sx={{
                display: "grid",
                gap: 1.5,
                gridTemplateColumns: {
                  xs: "1fr",
                  sm: "repeat(2, minmax(0, 1fr))",
                },
              }}
            >
              {Array.from({ length: 6 }).map((_, index) => (
                <Skeleton key={index} variant="rounded" height={60}></Skeleton>
              ))}
            </Box>
          </Stack>
        </CardContent>
      </Card>
    );
  }

  if (isError) {
    return (
      <Card component="section">
        <CardContent sx={{ p: { xs: 3, md: 4 } }}>
          <Stack spacing={1.5}>
            <Typography variant="h4">Item details unavailable</Typography>
            <Typography color="error">
              The selected item could not be loaded from the API.
            </Typography>
            <Typography color="text.secondary">
              Check that the backend is running and that the selected item still
              has metadata available.
            </Typography>
          </Stack>
        </CardContent>
      </Card>
    );
  }

  if (!item) {
    return (
      <Card component="section">
        <CardContent sx={{ p: { xs: 3, md: 4 } }}>
          <Stack spacing={3}>
            <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
              <Box
                sx={{
                  width: 88,
                  height: 88,
                  borderRadius: 3,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  bgcolor: "rgba(255, 194, 54, 0.08)",
                  color: "primary.main",
                  border: "1px solid",
                  borderColor: "divider",
                  flexShrink: 0,
                }}
              >
                <AutoGraphRoundedIcon
                  sx={{ fontSize: 40 }}
                ></AutoGraphRoundedIcon>
              </Box>

              <Stack spacing={1.25}>
                <Typography variant="h4">Select an item to inspect</Typography>
                <Typography color="text.secondary">
                  Choose a commodity from search results to load the latest
                  lowest buyout, metadata summary, and market history.
                </Typography>
              </Stack>
            </Stack>

            <Stack
              direction="row"
              spacing={1}
              useFlexGap
              sx={{ flexWrap: "wrap" }}
            ></Stack>
          </Stack>
        </CardContent>
      </Card>
    );
  }

  const detailRows = buildDetailRows(item);

  const summaryChips: string[] = [];

  if (item.qualityName) {
    summaryChips.push(item.qualityName);
  }

  if (item.itemClassName) {
    summaryChips.push(item.itemClassName);
  }

  if (item.professionName) {
    summaryChips.push(item.professionName);
  }

  if (item.isStackable) {
    summaryChips.push("Stackable");
  }

  if (item.isEquippable) {
    summaryChips.push("Equippable");
  }

  return (
    <Card component="section">
      <CardContent sx={{ p: { xs: 3, md: 4 } }}>
        <Stack spacing={3}>
          <Stack direction={{ xs: "column", lg: "row" }} spacing={3}>
            <Stack
              direction={{ xs: "column", sm: "row" }}
              spacing={3}
              sx={{ flexGrow: 1 }}
            >
              <Box
                sx={{
                  width: { xs: 96, sm: 120 },
                  height: { xs: 96, sm: 120 },
                  borderRadius: 3,
                  overflow: "hidden",
                  border: "1px solid",
                  borderColor: "divider",
                  flexShrink: 0,
                  bgcolor: "rgba(255, 194, 54, 0.08)",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  boxShadow: "0 0 0 1px rgba(255, 194, 54, 0.12)",
                }}
              >
                {item.imageUrl ? (
                  <Box
                    component="img"
                    src={item.imageUrl}
                    alt={item.name}
                    sx={{
                      width: "100%",
                      height: "100%",
                      objectFit: "cover",
                    }}
                  />
                ) : (
                  <Inventory2OutlinedIcon
                    sx={{ fontSize: 36, color: "text.secondary" }}
                  ></Inventory2OutlinedIcon>
                )}
              </Box>

              <Stack spacing={2} sx={{ minWidth: 0 }}>
                <Box>
                  <Typography variant="h3" gutterBottom>
                    {item.name}
                  </Typography>
                  <Typography color="text.secondary">
                    Review the latest market snapshot, pricing details, and item
                    metadata.
                  </Typography>
                </Box>

                <Stack
                  direction="row"
                  spacing={1}
                  useFlexGap
                  sx={{ flexWrap: "wrap" }}
                >
                  {summaryChips.map((chipLabel) => (
                    <Chip key={chipLabel} label={chipLabel}></Chip>
                  ))}
                </Stack>
              </Stack>
            </Stack>

            <Box
              sx={{
                minWidth: { lg: 260 },
                p: 3,
                borderRadius: 3,
                background:
                  "linear-gradient(180deg, rgba(255, 194, 54, 0.16), rgba(47, 103, 177, 0.1))",
                border: "1px solid",
                borderColor: "rgba(255, 194, 54, 0.24)",
                boxShadow: "inset 0 1px 0 rgba(255, 255, 255, 0.04)",
              }}
            >
              <Stack spacing={1.25}>
                <Typography variant="overline" color="primary.main">
                  Lowest buyout
                </Typography>
                <Typography variant="h4">
                  {formatUnitPrice(item.unitPrice)}
                </Typography>
                <Typography color="text.secondary">
                  {formatPriceTimestamp(item.priceTakenAtUtc)}
                </Typography>
              </Stack>
            </Box>
          </Stack>

          <Divider></Divider>

          <Box
            sx={{
              display: "grid",
              gap: 1.5,
              gridTemplateColumns: {
                xs: "1fr",
                sm: "repeat(2, minmax(0, 1fr))",
              },
            }}
          >
            {detailRows.map((row) => (
              <Box
                key={row.label}
                sx={{
                  p: 2,
                  borderRadius: 2.5,
                  border: "1px solid",
                  borderColor: "divider",
                  bgcolor: "rgba(8, 15, 25, 0.72)",
                }}
              >
                <Stack direction="row" spacing={1.25}>
                  <CategoryRoundedIcon
                    sx={{ color: "primary.main", fontSize: 20, mt: 0.2 }}
                  ></CategoryRoundedIcon>
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      {row.label}
                    </Typography>
                    <Typography variant="body1" sx={{ fontWeight: 600 }}>
                      {row.value}
                    </Typography>
                  </Box>
                </Stack>
              </Box>
            ))}
          </Box>
        </Stack>
      </CardContent>
    </Card>
  );
}
