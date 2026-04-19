import {
  Box,
  Card,
  CardContent,
  CircularProgress,
  Stack,
  Typography,
} from "@mui/material";
import type { ItemMetadataResponse } from "../types/itemSearchTypes";
import { useAutoRefresh } from "../../../hooks/useAutoRefresh";
import { formatPriceTimestamp } from "../../../utils/formatPriceTimestamp";
import { formatUnitPrice } from "../../../utils/formatUnitPrice";

interface SelectedItemCardProps {
  item: ItemMetadataResponse | undefined;
  isLoading: boolean;
  isError: boolean;
}

export default function SelectedItemCard(props: SelectedItemCardProps) {
  const { item, isLoading, isError } = props;

  useAutoRefresh();

  if (isLoading) {
    return (
      <Card variant="outlined">
        <CardContent>
          <Stack direction="row" spacing={2} sx={{ alignItems: "center" }}>
            <CircularProgress size={24} />
            <Typography>Loading selected item...</Typography>
          </Stack>
        </CardContent>
      </Card>
    );
  }

  if (isError) {
    return (
      <Card variant="outlined">
        <CardContent>
          <Typography color="error">
            Could not load the selected item.
          </Typography>
        </CardContent>
      </Card>
    );
  }

  if (!item) {
    return <Card></Card>;
  }

  return (
    <Card>
      <CardContent>
        <Stack direction={{ xs: "column", sm: "row" }} spacing={2.5}>
          <Box
            sx={{
              width: 64,
              height: 64,
              borderRadius: 1,
              overflow: "hidden",
              border: "1px solid",
              borderColor: "divider",
              flexShrink: 0,
              bgcolor: "background.default",
            }}
          >
            {item.imageUrl ? (
              <Box
                component="img"
                src={item.imageUrl}
                alt={item.name}
                sx={{
                  width: "100%",
                  height: "100%",
                  objectFit: "cover",
                  display: "block",
                }}
              />
            ) : null}
          </Box>

          <Box>
            <Typography variant="h5" gutterBottom>
              {item.name}
            </Typography>

            <Typography variant="body1" sx={{ mb: 1 }}>
              Current Lowest Auction Price (US-realms):{" "}
              {formatUnitPrice(item.unitPrice)}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {formatPriceTimestamp(item.priceTakenAtUtc)}
            </Typography>
          </Box>
        </Stack>
      </CardContent>
    </Card>
  );
}
