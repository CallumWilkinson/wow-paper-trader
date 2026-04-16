import {
  Card,
  CardActionArea,
  CardContent,
  List,
  ListItem,
  Stack,
  Typography,
} from "@mui/material";
import type { ItemSearchResponse } from "../Types/ItemSearchTypes";

interface ItemSearchResultsProps {
  items: ItemSearchResponse[];
  selectedItemId: number | null;
  onSelectItem: (itemId: number) => void;
}

export default function ItemSearchResults(props: ItemSearchResultsProps) {
  const { items, selectedItemId, onSelectItem } = props;

  if (items.length === 0) {
    return <Typography color="text.secondary">No results yet.</Typography>;
  }

  return (
    <List disablePadding>
      <Stack spacing={1.5}>
        {items.map((item) => {
          const isSelected = items.itemId === selectedItemId;

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
