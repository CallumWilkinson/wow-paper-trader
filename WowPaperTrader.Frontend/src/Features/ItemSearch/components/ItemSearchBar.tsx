import { useState, useEffect } from "react";
import { Box, Stack, TextField } from "@mui/material";
import { useDebouncedValue } from "../hooks/useDebouncedValue";

interface ItemSearchBarProps {
  onDebouncedSearch: (searchTerm: string) => void;
}

export default function ItemSearchBar(props: ItemSearchBarProps) {
  const { onDebouncedSearch } = props;
  const [searchTerm, setSearchTerm] = useState("");

  const debouncedValue = useDebouncedValue(searchTerm.trim(), 300);

  function handleChange(event: React.ChangeEvent<HTMLInputElement>) {
    setSearchTerm(event.target.value);
  }

  useEffect(() => {
    onDebouncedSearch(debouncedValue);
  }, [debouncedValue, onDebouncedSearch]);

  return (
    <Box>
      <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
        <TextField
          fullWidth
          label="Search for an item"
          value={searchTerm}
          onChange={handleChange}
          placeholder="e.g. Copper Ore"
        />
      </Stack>
    </Box>
  );
}
