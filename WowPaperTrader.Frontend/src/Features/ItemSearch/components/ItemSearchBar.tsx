import { useState } from "react";
import { Box, Stack, TextField, Button } from "@mui/material";

interface ItemSearchBarProps {
  onSearch: (searchTerm: string) => void;
  isSearching: boolean;
}

export default function ItemSearchBar(props: ItemSearchBarProps) {
  const { onSearch, isSearching } = props;
  const [searchTerm, setSearchTerm] = useState("");

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    onSearch(searchTerm);
  }

  function handleChange(event: React.ChangeEvent<HTMLInputElement>) {
    setSearchTerm(event.target.value);
  }
  return (
    <Box component="form" onSubmit={handleSubmit}>
      <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
        <TextField
          fullWidth
          label="Search for an item"
          value={searchTerm}
          onChange={handleChange}
          placeholder="e.g. Copper Ore"
        />

        <Button
          type="submit"
          variant="contained"
          disabled={isSearching || searchTerm.trim().length === 0}
          sx={{ minWidth: 140 }}
        >
          {isSearching ? "Searching..." : "Search"}
        </Button>
      </Stack>
    </Box>
  );
}
