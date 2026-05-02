import { useState } from "react";
import { Alert, Box, Chip, Container, Paper, Stack, Typography } from "@mui/material";
import ItemSearchBar from "../features/itemSearch/components/ItemSearchBar";
import ItemSearchResults from "../features/itemSearch/components/ItemSearchResults";
import SelectedItemCard from "../features/itemSearch/components/SelectedItemCard";
import { useItemSearch } from "../features/itemSearch/hooks/useItemSearch";
import { useSelectedItem } from "../features/itemSearch/hooks/useSelectedItem";
import { useDebouncedValue } from "../features/itemSearch/hooks/useDebouncedValue";
import PriceQuantityGraph from "../features/priceQuantityGraph/components/PriceQuantityGraph";
import { usePriceQuantityHistory } from "../features/priceQuantityGraph/hooks/usePriceQuantityHistory";

export default function ItemSearchPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedItemId, setSelectedItemId] = useState<number | null>(null);

  const debouncedSearchTerm = useDebouncedValue(searchTerm, 200);

  const itemSearchPayload = useItemSearch(debouncedSearchTerm);

  const selectedItemPayload = useSelectedItem(selectedItemId);

  const monthlyPriceQuantityPayload = usePriceQuantityHistory(selectedItemId);

  const items = itemSearchPayload.data ?? [];

  const hasSearchResults = items.length > 0;

  const searchErrorMessage = itemSearchPayload.isError
    ? "Search failed. Check the API is running and that CORS is enabled."
    : null;

  const isDropdownShown =
    selectedItemId === null && searchTerm.trim().length > 0 && items.length > 0;

  const isItemsFound =
    searchTerm.trim().length > 0 &&
    !itemSearchPayload.isFetching &&
    !hasSearchResults &&
    !itemSearchPayload.isError;

  function handleSearchInputChange(searchTerm: string) {
    setSearchTerm(searchTerm);
    setSelectedItemId(null);
  }

  function handleSelectItem(itemId: number) {
    setSelectedItemId(itemId);
    setSearchTerm("");
  }

  function handleEnter() {
    if (items.length === 0) {
      return;
    }

    const firstItem = items[0];
    setSelectedItemId(firstItem.itemId);
  }

  return (
    <Box sx={{ py: { xs: 4, md: 6 } }}>
      <Container maxWidth="xl">
        <Stack spacing={3.5}>
          <Box component="header">
            <Paper
              sx={{
                px: { xs: 2.5, md: 3.5 },
                py: 1.75,
                background:
                  "linear-gradient(180deg, rgba(51, 36, 40, 0.98), rgba(43, 31, 35, 0.96))",
              }}
            >
              <Stack
                direction={{ xs: "column", sm: "row" }}
                spacing={2}
                sx={{
                  justifyContent: "space-between",
                  alignItems: { xs: "flex-start", sm: "center" },
                }}
              >
                <Stack
                  direction="row"
                  spacing={2}
                  sx={{ alignItems: "center" }}
                >
                  <Box
                    component="img"
                    src="/wow-midnight-logo.avif"
                    alt="World of Warcraft Midnight"
                    sx={{
                      width: { xs: 132, sm: 168 },
                      height: "auto",
                      filter: "drop-shadow(0 10px 18px rgba(0, 0, 0, 0.45))",
                    }}
                  ></Box>

                  <Box>
                    <Typography variant="overline" color="primary.light">
                      WowPaperTrader
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Auction House market intelligence for US realms
                    </Typography>
                  </Box>
                </Stack>

                <Chip label="US market data"></Chip>
              </Stack>
            </Paper>
          </Box>

          <Paper
            component="section"
            sx={{
              position: "relative",
              overflow: "hidden",
              p: { xs: 3, md: 5 },
              background:
                "linear-gradient(135deg, rgba(10, 15, 27, 0.98), rgba(8, 13, 24, 0.97))",
            }}
          >
            <Box
              sx={{
                position: "absolute",
                inset: 0,
                background:
                  "radial-gradient(circle at top right, rgba(255, 196, 70, 0.16), transparent 24%)",
                pointerEvents: "none",
              }}
            ></Box>
            <Box
              sx={{
                position: "absolute",
                inset: 0,
                background:
                  "radial-gradient(circle at 18% 18%, rgba(50, 97, 174, 0.22), transparent 26%)",
                pointerEvents: "none",
              }}
            ></Box>

            <Stack
              direction={{ xs: "column", lg: "row" }}
              spacing={{ xs: 4, lg: 6 }}
              sx={{ position: "relative", alignItems: "center" }}
            >
                <Stack spacing={4} sx={{ flex: 1, width: "100%", maxWidth: 760 }}>
                  <Stack spacing={2}>
                    <Typography variant="overline" color="primary.light">
                      Midnight Auction House Watch
                    </Typography>

                    <Typography variant="h2">
                      Track World of Warcraft commodity prices across US realms.
                    </Typography>

                    <Typography variant="body1" color="text.secondary">
                      Search for commodities, inspect the current lowest buyout,
                      and review 30 days of price and quantity movement through a
                      WoW-inspired market dashboard.
                    </Typography>
                  </Stack>

                <Stack
                  direction="row"
                  spacing={1}
                  useFlexGap
                  sx={{ flexWrap: "wrap" }}
                >
                  <Chip label="Official Midnight branding"></Chip>
                  <Chip label="US realms"></Chip>
                  <Chip label="Lowest buyout"></Chip>
                  <Chip label="30-day history"></Chip>
                </Stack>

                <Box
                  sx={{
                    p: { xs: 2, md: 2.5 },
                    borderRadius: 3,
                    border: "1px solid",
                    borderColor: "divider",
                    bgcolor: "rgba(9, 14, 24, 0.84)",
                    backdropFilter: "blur(8px)",
                  }}
                >
                  <Stack spacing={1.5}>
                    <ItemSearchBar
                      value={searchTerm}
                      onChange={handleSearchInputChange}
                      onEnter={handleEnter}
                    ></ItemSearchBar>

                    {searchErrorMessage ? (
                      <Alert severity="error">{searchErrorMessage}</Alert>
                    ) : null}

                    {isItemsFound ? (
                      <Alert severity="info">
                        No matching items found for that search.
                      </Alert>
                    ) : null}

                    {isDropdownShown ? (
                      <ItemSearchResults
                        items={items}
                        selectedItemId={selectedItemId}
                        onSelectItem={handleSelectItem}
                      ></ItemSearchResults>
                    ) : null}
                  </Stack>
                </Box>
              </Stack>

              <Box
                sx={{
                  flex: { lg: "0 0 360px" },
                  width: "100%",
                  display: "flex",
                  justifyContent: "center",
                }}
              >
                <Box
                  component="img"
                  src="/wow-midnight-logo.avif"
                  alt="World of Warcraft Midnight"
                  sx={{
                    width: "100%",
                    maxWidth: 360,
                    height: "auto",
                    filter:
                      "drop-shadow(0 18px 36px rgba(0, 0, 0, 0.5)) drop-shadow(0 0 22px rgba(214, 174, 88, 0.18))",
                  }}
                ></Box>
              </Box>
            </Stack>
          </Paper>

          <SelectedItemCard
            item={selectedItemPayload.data}
            isLoading={selectedItemPayload.isLoading}
            isError={selectedItemPayload.isError}
          ></SelectedItemCard>

          <PriceQuantityGraph
            data={monthlyPriceQuantityPayload.data}
            isLoading={monthlyPriceQuantityPayload.isLoading}
            isError={monthlyPriceQuantityPayload.isError}
            itemName={selectedItemPayload.data?.name}
          ></PriceQuantityGraph>
        </Stack>
      </Container>
    </Box>
  );
}
