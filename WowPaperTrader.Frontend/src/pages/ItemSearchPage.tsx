import { useState } from "react";
import { Alert, Box, Container, Paper, Stack, Typography } from "@mui/material";
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
          <Paper
            component="section"
            sx={{
              position: "relative",
              p: { xs: 3, md: 5 },
              bgcolor: "#0b1020",
            }}
          >
            <Stack
              direction={{ xs: "column", lg: "row" }}
              spacing={{ xs: 4, lg: 6 }}
              sx={{ alignItems: "center" }}
            >
              <Stack spacing={4} sx={{ flex: 1, width: "100%", maxWidth: 760 }}>
                <Stack spacing={2}>
                  <Typography variant="overline" color="secondary.light">
                    Midnight Auction House Watch
                  </Typography>

                  <Typography variant="h2">
                    Track World of Warcraft commodity prices across US realms.
                  </Typography>

                  <Typography variant="body1" color="text.secondary">
                    Search for commodities, inspect the current lowest buyout,
                    and review 30 days of price and quantity data.
                  </Typography>
                </Stack>

                <Stack
                  direction="row"
                  spacing={1}
                  useFlexGap
                  sx={{ flexWrap: "wrap" }}
                ></Stack>

                <Box
                  sx={{
                    p: { xs: 2, md: 2.5 },
                    borderRadius: 3,
                    border: "1px solid",
                    borderColor: (theme) => theme.palette.info.main,
                    bgcolor: "rgba(13, 19, 36, 0.88)",
                    backdropFilter: "blur(8px)",
                    boxShadow: (theme) =>
                      `inset 0 1px 0 rgba(255, 255, 255, 0.03), 0 0 0 1px ${theme.palette.divider}`,
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
                      "drop-shadow(0 18px 36px rgba(0, 0, 0, 0.5)) drop-shadow(0 0 20px rgba(75, 141, 228, 0.18)) drop-shadow(0 0 26px rgba(146, 120, 255, 0.18))",
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
