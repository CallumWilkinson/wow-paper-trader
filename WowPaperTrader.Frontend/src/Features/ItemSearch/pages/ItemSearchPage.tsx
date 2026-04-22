import { useState } from "react";
import { Alert, Box, Container, Paper, Stack, Typography } from "@mui/material";
import ItemSearchBar from "../components/ItemSearchBar";
import ItemSearchResults from "../components/ItemSearchResults";
import SelectedItemCard from "../components/SelectedItemCard";
import { useItemSearch } from "../hooks/useItemSearch";
import { useSelectedItem } from "../hooks/useSelectedItem";

export default function ItemSearchPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedItemId, setSelectedItemId] = useState<number | null>(null);

  const itemSearchPayload = useItemSearch({
    searchTerm: searchTerm,
  });

  const selectedItemPayload = useSelectedItem(selectedItemId);

  function handleSearch(searchTerm: string) {
    const trimmedSearchTerm = searchTerm.trim();
    setSearchTerm(trimmedSearchTerm);
    setSelectedItemId(null);
  }

  function handleSelectItem(itemId: number) {
    setSelectedItemId(itemId);
    setSearchTerm("");
  }

  const items = itemSearchPayload.data ?? [];

  const hasSearchResults = items.length > 0;

  const searchErrorMessage = itemSearchPayload.isError
    ? "Search failed. Check the API is running and that CORS is enabled."
    : null;

  const isDropdownShown =
    selectedItemId === null && searchTerm.trim().length > 0 && items.length > 0;

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Stack spacing={3}>
        <Box>
          <Typography variant="h3" gutterBottom>
            World of Warcraft: Midnight Auction House
          </Typography>

          <Typography color="text.secondary">
            Search for a WoW commodity to find the lowest buyout price across
            all US-realms.
          </Typography>
        </Box>

        <Paper sx={{ p: 3 }}>
          <Stack>
            <ItemSearchBar onDebouncedSearch={handleSearch}></ItemSearchBar>

            {searchErrorMessage ? (
              <Alert severity="error"> {searchErrorMessage}</Alert>
            ) : null}

            {searchTerm.trim().length > 0 &&
            !itemSearchPayload.isFetching &&
            !hasSearchResults &&
            !itemSearchPayload.isError ? (
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
      </Stack>
    </Container>
  );
}
