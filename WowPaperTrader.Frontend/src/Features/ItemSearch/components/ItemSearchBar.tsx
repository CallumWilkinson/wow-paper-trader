import { Box, Stack, TextField } from "@mui/material";

interface ItemSearchBarProps {
  value: string;
  onChange: (value: string) => void;
  onEnter: () => void;
}

export default function ItemSearchBar(props: ItemSearchBarProps) {
  const { value, onChange, onEnter } = props;

  function handleChange(event: React.ChangeEvent<HTMLInputElement>) {
    onChange(event.target.value);
  }

  function handleKeyDown(event: React.KeyboardEvent<HTMLInputElement>) {
    if (event.key === "Enter") {
      event.preventDefault();
      onEnter();
    }
  }
  return (
    <Box>
      <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
        <TextField
          fullWidth
          label="Search for an item"
          value={value}
          onChange={handleChange}
          onKeyDown={handleKeyDown}
          placeholder="e.g. Copper Ore"
        />
      </Stack>
    </Box>
  );
}
