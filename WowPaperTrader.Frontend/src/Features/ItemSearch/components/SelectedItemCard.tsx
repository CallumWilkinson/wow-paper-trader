import {
  Box,
  Card,
  CardContent,
  Divider,
  Skeleton,
  Stack,
  Typography,
} from "@mui/material";
import type { ReactNode } from "react";
import AutoGraphRoundedIcon from "@mui/icons-material/AutoGraphRounded";
import CategoryRoundedIcon from "@mui/icons-material/CategoryRounded";
import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import CurrencyAmount from "../../../components/CurrencyAmount";
import type { ItemMetadataResponse } from "../types/itemSearchTypes";
import { useAutoRefresh } from "../../../hooks/useAutoRefresh";
import { formatPriceTimestamp } from "../../../utils/formatPriceTimestamp";

interface SelectedItemCardProps {
  item: ItemMetadataResponse | undefined;
  isLoading: boolean;
  isError: boolean;
}

interface DetailRow {
  label: string;
  value: ReactNode;
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
    item.purchasePrice !== null
      ? {
          label: "Vendor price",
          value: (
            <CurrencyAmount unitPrice={item.purchasePrice}></CurrencyAmount>
          ),
        }
      : null,
    item.sellPrice !== null
      ? {
          label: "Vendor sell",
          value: <CurrencyAmount unitPrice={item.sellPrice}></CurrencyAmount>,
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
                  bgcolor: "rgba(75, 141, 228, 0.12)",
                  color: "secondary.main",
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
                <Typography variant="h4">Search for an item</Typography>
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

  return (
    <Card
      component="section"
      sx={{
        overflow: "hidden",
        borderRadius: 3,
        border: "1px solid",
        borderColor: "divider",
        bgcolor: "rgba(10, 17, 31, 0.96)",
      }}
    >
      <CardContent sx={{ p: { xs: 3, md: 4 } }}>
        <Stack spacing={3}>
          <Stack
            direction={{ xs: "column", lg: "row" }}
            spacing={3}
            sx={{
              p: { xs: 2.5, md: 3 },
              borderRadius: 3,
              border: "1px solid",
              borderColor: "divider",
              bgcolor: "rgba(11, 18, 32, 0.78)",
            }}
          >
            <Stack
              direction={{ xs: "column", sm: "row" }}
              spacing={{ xs: 2.5, sm: 3 }}
              sx={{
                flexGrow: 1,
                minWidth: 0,
                alignItems: { xs: "flex-start", sm: "center" },
              }}
            >
              <Box
                sx={{
                  position: "relative",
                  p: 0.5,
                  borderRadius: 3,
                  border: "1px solid",
                  borderColor: "divider",
                  bgcolor: "rgba(146, 120, 255, 0.12)",
                  flexShrink: 0,
                }}
              >
                <Box
                  sx={{
                    width: { xs: 104, sm: 128 },
                    height: { xs: 104, sm: 128 },
                    borderRadius: 2.5,
                    overflow: "hidden",
                    bgcolor: "rgba(10, 17, 31, 0.96)",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
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
                      sx={{ fontSize: 42, color: "text.secondary" }}
                    ></Inventory2OutlinedIcon>
                  )}
                </Box>
              </Box>

              <Stack
                spacing={2}
                sx={{
                  minWidth: 0,
                  flexGrow: 1,
                  justifyContent: "center",
                }}
              >
                <Box sx={{ maxWidth: 560 }}>
                  <Typography
                    variant="h3"
                    sx={{
                      lineHeight: 1.05,
                      letterSpacing: "-0.02em",
                    }}
                  >
                    {item.name}
                  </Typography>
                </Box>
              </Stack>
            </Stack>

            <Box
              sx={{
                minWidth: { lg: 320 },
                p: { xs: 2.5, md: 3 },
                borderRadius: 3,
                bgcolor: "rgba(15, 23, 40, 0.92)",
                border: "1px solid",
                borderColor: "rgba(146, 120, 255, 0.24)",
                display: "flex",
                alignItems: "center",
              }}
            >
              <Stack spacing={1.5}>
                <Typography
                  variant="overline"
                  color="info.light"
                  sx={{ letterSpacing: "0.16em" }}
                >
                  Lowest buyout
                </Typography>
                <Box
                  sx={{
                    fontSize: { xs: "2rem", md: "2.35rem" },
                    fontWeight: 400,
                    lineHeight: 1.15,
                  }}
                >
                  <CurrencyAmount
                    unitPrice={item.unitPrice}
                    iconSize={20}
                    amountSx={{ fontSize: { xs: "2rem", md: "2.35rem" } }}
                  ></CurrencyAmount>
                </Box>
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
                  bgcolor: "rgba(11, 18, 32, 0.78)",
                }}
              >
                <Stack direction="row" spacing={1.25}>
                  <CategoryRoundedIcon
                    sx={{ color: "secondary.main", fontSize: 20, mt: 0.2 }}
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
