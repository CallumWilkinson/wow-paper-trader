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
        borderRadius: 1,
        overflow: "hidden",
        bgcolor: "background.paper",
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
              dense
              sx={{
                px: 1.5,
                py: 0.75,
                minHeight: 44,
              }}
            >
              <Box
                component="img"
                src={item.imageUrl}
                alt={item.name}
                sx={{
                  width: 28,
                  height: 24,
                  mr: 1.25,
                  borderRadius: 0.5,
                  flexShrink: 0,
                }}
              />
              <ListItemText
                disableTypography
                primary={
                  <Typography variant="body1" sx={{ lineHeight: 1.2 }}>
                    {item.name}
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
