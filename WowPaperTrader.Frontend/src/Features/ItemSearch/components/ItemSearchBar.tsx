import { Box, Stack, TextField } from "@mui/material";

interface ItemSearchBarProps {
  value: string;
  onChange: (value: string) => void;
}

export default function ItemSearchBar(props: ItemSearchBarProps) {
  const { value, onChange } = props;

  function handleChange(event: React.ChangeEvent<HTMLInputElement>) {
    onChange(event.target.value);
  }
  return (
    <Box>
      <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
        <TextField
          fullWidth
          label="Search for an item"
          value={value}
          onChange={handleChange}
          placeholder="e.g. Copper Ore"
        />
      </Stack>
    </Box>
  );
}
