import { Box } from "@mui/material";
import type { SxProps, Theme } from "@mui/material/styles";
import {
  formatUnitPriceAriaLabel,
  getUnitPriceParts,
} from "../utils/formatUnitPrice";

interface CurrencyAmountProps {
  unitPrice: number;
  iconSize?: number;
  amountSx?: SxProps<Theme>;
}

export default function CurrencyAmount(props: CurrencyAmountProps) {
  const { unitPrice, iconSize = 16, amountSx } = props;

  const priceParts = getUnitPriceParts(unitPrice);

  return (
    <Box
      component="span"
      sx={{
        display: "inline-flex",
        alignItems: "center",
        gap: 1,
        flexWrap: "wrap",
      }}
      aria-label={formatUnitPriceAriaLabel(unitPrice)}
    >
      {priceParts.map((part) => (
        <Box
          key={`${part.iconUrl}-${part.amount}`}
          component="span"
          sx={{
            display: "inline-flex",
            alignItems: "center",
            gap: 0.5,
          }}
        >
          <Box
            component="span"
            sx={{
              fontVariantNumeric: "tabular-nums",
              lineHeight: 1,
              ...amountSx,
            }}
          >
            {part.amount.toLocaleString()}
          </Box>
          <Box
            component="img"
            src={part.iconUrl}
            alt=""
            aria-hidden="true"
            sx={{
              width: iconSize,
              height: iconSize,
              borderRadius: "50%",
              flexShrink: 0,
            }}
          />
        </Box>
      ))}
    </Box>
  );
}
