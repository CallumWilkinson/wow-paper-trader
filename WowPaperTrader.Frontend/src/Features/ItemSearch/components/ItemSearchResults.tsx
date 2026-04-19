import {
  Card,
  CardActionArea,
  CardContent,
  List,
  ListItem,
  Stack,
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
    <List disablePadding>
      <Stack spacing={1.5}>
        {items.map((item) => {
          const isSelected = item.itemId === selectedItemId;

          return (
            <ListItem key={item.itemId} disableGutters>
              <Card
                variant={isSelected ? "elevation" : "outlined"}
                sx={{ width: "100%" }}
              >
                <CardActionArea onClick={() => onSelectItem(item.itemId)}>
                  <CardContent>
                    <Typography variant="h6">{item.name}</Typography>
                  </CardContent>
                </CardActionArea>
              </Card>
            </ListItem>
          );
        })}
      </Stack>
    </List>
  );
}
