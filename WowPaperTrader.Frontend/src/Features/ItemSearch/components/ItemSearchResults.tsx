import Inventory2OutlinedIcon from "@mui/icons-material/Inventory2Outlined";
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
} from "@mui/material";
import type { ItemSearchResponse } from "../types/itemSearchTypes";

interface ItemSearchResultsProps {
  items: ItemSearchResponse[];
  selectedItemId: number | null;
  onSelectItem: (itemId: number) => void;
}

export default function ItemSearchResults(props: ItemSearchResultsProps) {
  const { items, selectedItemId, onSelectItem } = props;

  return (
    <List
      disablePadding
      sx={{
        mt: 0.5,
        border: 1,
        borderColor: "divider",
        borderRadius: 2,
        overflow: "hidden",
        bgcolor: "rgba(8, 16, 28, 0.96)",
        maxHeight: 320,
        overflowY: "auto",
      }}
    >
      {items.map((item, index) => {
        const isSelected = item.itemId === selectedItemId;
        const isLastItem = index === items.length - 1;

        return (
          <ListItem key={item.itemId} disablePadding divider={!isLastItem}>
            <ListItemButton
              selected={isSelected}
              onClick={() => onSelectItem(item.itemId)}
              sx={{
                px: 2,
                py: 1.25,
                minHeight: 60,
                alignItems: "center",
                transition: "background-color 140ms ease",
                "&:hover": {
                  backgroundColor: "rgba(78, 127, 177, 0.12)",
                },
                "&.Mui-selected": {
                  backgroundColor: "rgba(214, 174, 88, 0.16)",
                },
                "&.Mui-selected:hover": {
                  backgroundColor: "rgba(214, 174, 88, 0.24)",
                },
              }}
            >
              <Box
                sx={{
                  width: 44,
                  height: 44,
                  mr: 1.75,
                  borderRadius: 1.5,
                  flexShrink: 0,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  border: "1px solid",
                  borderColor: "divider",
                  bgcolor: "rgba(214, 174, 88, 0.08)",
                  overflow: "hidden",
                }}
              >
                {item.imageUrl ? (
                  <Box
                    component="img"
                    src={item.imageUrl}
                    alt={item.name}
                    sx={{
                      width: 36,
                      height: 36,
                    }}
                  />
                ) : (
                  <Inventory2OutlinedIcon color="action"></Inventory2OutlinedIcon>
                )}
              </Box>
              <ListItemText
                disableTypography
                primary={
                  <Typography variant="body1" sx={{ lineHeight: 1.3, fontWeight: 600 }}>
                    {item.name}
                  </Typography>
                }
                secondary={
                  <Typography variant="body2" color="text.secondary">
                    Item #{item.itemId}
                  </Typography>
                }
              />
            </ListItemButton>
          </ListItem>
        );
      })}
    </List>
  );
}
