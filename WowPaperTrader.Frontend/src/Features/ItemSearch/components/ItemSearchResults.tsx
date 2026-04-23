import {
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
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
                  <Stack
                    direction="row"
                    spacing={2}
                    sx={{ p: 1, alignItems: "center" }}
                  >
                    <CardMedia
                      component="img"
                      image={item.imageUrl}
                      alt={item.name}
                      sx={{
                        width: 56,
                        height: 56,
                        borderRadius: 1,
                        flexShrink: 0,
                      }}
                    ></CardMedia>
                    <CardContent sx={{ p: 0 }}>
                      <Typography variant="h6">{item.name}</Typography>
                    </CardContent>
                  </Stack>
                </CardActionArea>
              </Card>
            </ListItem>
          );
        })}
      </Stack>
    </List>
  );
}
