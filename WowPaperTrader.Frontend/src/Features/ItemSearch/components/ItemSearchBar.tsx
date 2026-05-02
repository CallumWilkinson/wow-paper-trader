import SearchRoundedIcon from "@mui/icons-material/SearchRounded";
import { Box, InputAdornment, TextField } from "@mui/material";

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
      <TextField
        fullWidth
        label="Search for a commodity"
        value={value}
        onChange={handleChange}
        onKeyDown={handleKeyDown}
        placeholder="Try Copper Ore, Linen Cloth, or Silk Cloth"
        autoComplete="off"
        slotProps={{
          input: {
            startAdornment: (
              <InputAdornment position="start">
                <SearchRoundedIcon color="action"></SearchRoundedIcon>
              </InputAdornment>
            ),
          },
        }}
        helperText="Press Enter to select the first result, or choose an item from the list."
        sx={{
          "& .MuiInputBase-input::placeholder": {
            color: "text.secondary",
            opacity: 1,
          },
        }}
      />
    </Box>
  );
}
