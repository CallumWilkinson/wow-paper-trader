import {
  ResponsiveContainer,
  ComposedChart,
  Line,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  Legend,
} from "recharts";
import type { MonthlyPriceQuantityResponse } from "../types/priceQuantityTypes";
import { formatLocalDate } from "../../../utils/formatLocalDate";
import { formatUnitPrice } from "../../../utils/formatUnitPrice";

interface PriceQuantityGraphProps {
  data: MonthlyPriceQuantityResponse;
}

function formatTooltipLabel(label: React.ReactNode): React.ReactNode {
  if (typeof label !== "string") {
    return label;
  }

  return new Date(label).toLocaleString();
}

export default function PriceQuantityGraph(props: PriceQuantityGraphProps) {
  const priceQuantityData = props.data.priceQuantityResponses;

  return (
    <ResponsiveContainer width="100%" height={400}>
      <ComposedChart data={priceQuantityData}>
        <CartesianGrid strokeDasharray="3 3"></CartesianGrid>
        <XAxis dataKey="fetchedAtUtc" tickFormatter={formatLocalDate}></XAxis>

        <YAxis
          yAxisId="price"
          orientation="left"
          tickFormatter={formatUnitPrice}
        ></YAxis>

        <Tooltip
          labelFormatter={formatTooltipLabel}
          formatter={(value, name) => {
            if (name === "lowestUnitPrice") {
              return [formatUnitPrice(value as number), "Price"];
            }
            return [value, "Quantity"];
          }}
        ></Tooltip>
        <Legend></Legend>

        <Bar
          yAxisId="quantity"
          dataKey="totalQuantityPosted"
          name="Quantity"
        ></Bar>

        <Line
          yAxisId="price"
          type="monotone"
          dataKey="lowestUnitPrice"
          name="Price"
          dot={false}
        ></Line>
      </ComposedChart>
    </ResponsiveContainer>
  );
}
