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
import {
  formatLocalDateLong,
  formatLocalDateShort,
} from "../../../utils/formatLocalDate";
import { formatUnitPrice } from "../../../utils/formatUnitPrice";

interface PriceQuantityGraphProps {
  data: MonthlyPriceQuantityResponse;
}

export default function PriceQuantityGraph(props: PriceQuantityGraphProps) {
  const priceQuantityData = props.data.priceQuantityResponses;

  return (
    <ResponsiveContainer width="100%" height={400}>
      <ComposedChart data={priceQuantityData}>
        <CartesianGrid strokeDasharray="3 3"></CartesianGrid>
        <XAxis
          dataKey="fetchedAtUtc"
          tickFormatter={formatLocalDateShort}
        ></XAxis>

        <YAxis
          yAxisId="price"
          orientation="left"
          tickFormatter={formatUnitPrice}
        ></YAxis>

        <YAxis yAxisId="quantity" orientation="right"></YAxis>

        <Tooltip
          labelFormatter={formatLocalDateLong}
          formatter={(value, name) => {
            if (name === "Price") {
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
          dot={true}
        ></Line>
      </ComposedChart>
    </ResponsiveContainer>
  );
}
