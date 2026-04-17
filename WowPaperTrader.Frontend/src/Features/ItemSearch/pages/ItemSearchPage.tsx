import { useState } from "react";
import { Alert, Box, Container, Paper, Stack, Typography } from "@mui/material";
import ItemSearchBar from "../components/ItemSearchBar";
import ItemSearchResults from "../components/ItemSearchResults";
import SelectedItemCard from "../components/SelectedItemCard";
import { useItemSearch } from "../hooks/useItemSearch";
import { useSelectedItem } from "../hooks/useSelectedItem";

export default function ItemSearchPage() {
  const [submittedSearchTerm, setSubmittedSearchTerm] = useState("");
  const [hasSubmittedSearch, setHasSubmittedSearch] = useState(false);
  const [selectedItemId, setSelectedItemId] = useState<number | null>(null);

  const itemSearchPayload = useItemSearch({
    searchTerm: submittedSearchTerm,
    hasSubmittedSearch: hasSubmittedSearch,
  });

  const selectedItemPayload = useSelectedItem(selectedItemId);

  function handleSearch(searchTerm: string) {
    const trimmedSearchTerm = searchTerm.trim();

    setSubmittedSearchTerm(trimmedSearchTerm);
    setHasSubmittedSearch(true);
    setSelectedItemId(null);
  }

  const items = itemSearchPayload.data ?? [];

  const hasSearchResults = items.length > 0;

  const searchErrorMessage = itemSearchPayload.isError
    ? "Search failed. Check the API is running and that CORS is enabled."
    : null;

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Stack spacing={3}>
        <Box>
          <Typography variant="h3" gutterBottom>
            World of Warcraft: Midnight Auction House
          </Typography>

          <Typography color="text.secondary">
            Search a WoW commodity for current prices across all US-realms.
          </Typography>
        </Box>

        <Paper sx={{ p: 3 }}>
          <Stack>
            <ItemSearchBar
              onSearch={handleSearch}
              isSearching={itemSearchPayload.isFetching}
            ></ItemSearchBar>

            {searchErrorMessage ? (
              <Alert severity="error"> {searchErrorMessage}</Alert>
            ) : null}

            {hasSubmittedSearch &&
            !itemSearchPayload.isFetching &&
            !hasSearchResults &&
            !itemSearchPayload.isError ? (
              <Alert severity="info">No matching items found.</Alert>
            ) : null}

            <ItemSearchResults
              items={items}
              selectedItemId={selectedItemId}
              onSelectItem={setSelectedItemId}
            ></ItemSearchResults>
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
