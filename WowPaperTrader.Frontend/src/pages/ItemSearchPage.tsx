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
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Stack spacing={3}>
        <Box>
          <Typography variant="h3" gutterBottom>
            World of Warcraft: Midnight <br></br> Auction House
          </Typography>

          <Typography color="text.secondary">
            Search for a WoW commodity to find the lowest buyout price across
            all US-realms.
          </Typography>
        </Box>

        <Paper sx={{ p: 3 }}>
          <Stack>
            <ItemSearchBar
              value={searchTerm}
              onChange={handleSearchInputChange}
              onEnter={handleEnter}
            ></ItemSearchBar>

            {searchErrorMessage ? (
              <Alert severity="error"> {searchErrorMessage}</Alert>
            ) : null}

            {isItemsFound ? (
              <Alert severity="info">No matching items found.</Alert>
            ) : null}

            {isDropdownShown ? (
              <ItemSearchResults
                items={items}
                selectedItemId={selectedItemId}
                onSelectItem={handleSelectItem}
              ></ItemSearchResults>
            ) : null}
          </Stack>
        </Paper>
        <Box>
          <SelectedItemCard
            item={selectedItemPayload.data}
            isLoading={selectedItemPayload.isLoading}
            isError={selectedItemPayload.isError}
          ></SelectedItemCard>
        </Box>
        <Box>
          {monthlyPriceQuantityPayload.data && (
            <PriceQuantityGraph
              data={monthlyPriceQuantityPayload.data}
            ></PriceQuantityGraph>
          )}
        </Box>
      </Stack>
    </Container>
  );
}
