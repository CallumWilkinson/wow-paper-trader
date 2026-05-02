import QueryStatsRoundedIcon from "@mui/icons-material/QueryStatsRounded";
import CurrencyAmount from "../../../components/CurrencyAmount";
import {
  Alert,
  Box,
  Card,
  CardContent,
  Skeleton,
  Stack,
  Typography,
  useTheme,
} from "@mui/material";
import {
  Bar,
  CartesianGrid,
  ComposedChart,
  Legend,
  Line,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import type { MonthlyPriceQuantityResponse } from "../types/priceQuantityTypes";
import {
  formatLocalDateLong,
  formatLocalDateShort,
} from "../../../utils/formatLocalDate";

interface PriceQuantityGraphProps {
  data: MonthlyPriceQuantityResponse | undefined;
  isLoading: boolean;
  isError: boolean;
  itemName: string | undefined;
}

interface PriceAxisTickProps {
  x?: number | string;
  y?: number | string;
  payload?: {
    value?: number | string;
  };
}

export default function PriceQuantityGraph(props: PriceQuantityGraphProps) {
  const { data, isLoading, isError, itemName } = props;
  const theme = useTheme();

  const priceQuantityData =
    data?.priceQuantityResponses.filter(
      (entry) => entry.fetchedAtUtc !== null,
    ) ?? [];

  const hasHistory = priceQuantityData.length > 0;

  function renderPriceAxisTick(props: PriceAxisTickProps) {
    const value = props.payload?.value;

    if (typeof value !== "number") {
      return null;
    }

    const x = typeof props.x === "number" ? props.x : Number(props.x ?? 0);
    const y = typeof props.y === "number" ? props.y : Number(props.y ?? 0);

    return (
      <g transform={`translate(${x - 108}, ${y - 12})`}>
        <foreignObject width={108} height={24}>
          <div
            style={{
              width: "108px",
              height: "24px",
              display: "flex",
              alignItems: "center",
              justifyContent: "flex-end",
            }}
          >
            <CurrencyAmount
              unitPrice={value}
              iconSize={12}
              amountSx={{
                color: theme.palette.text.secondary,
                fontSize: "12px",
              }}
            ></CurrencyAmount>
          </div>
        </foreignObject>
      </g>
    );
  }

  if (isLoading) {
    return (
      <Card component="section">
        <CardContent sx={{ p: { xs: 3, md: 4 } }}>
          <Stack spacing={2.5}>
            <Box>
              <Skeleton variant="text" width="34%" height={42}></Skeleton>
              <Skeleton variant="text" width="54%"></Skeleton>
            </Box>
            <Skeleton variant="rounded" height={360}></Skeleton>
          </Stack>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card component="section">
      <CardContent sx={{ p: { xs: 3, md: 4 } }}>
        <Stack spacing={3}>
          <Box>
            <Typography variant="h4">Market activity</Typography>
            <Typography color="text.secondary">
              {itemName
                ? `Price and quantity history for ${itemName} across the last 30 days of auction snapshots.`
                : "Select an item to load recent price and quantity data."}
            </Typography>
          </Box>

          {isError ? (
            <Alert severity="error">
              Price history could not be loaded for the selected item.
            </Alert>
          ) : null}

          {!isError && !itemName ? (
            <Box
              sx={{
                minHeight: 320,
                borderRadius: 3,
                border: "1px dashed",
                borderColor: "divider",
                bgcolor: "rgba(12, 18, 33, 0.8)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                p: 3,
              }}
            >
              <Stack spacing={1.5} sx={{ alignItems: "center", maxWidth: 420 }}>
                <QueryStatsRoundedIcon
                  sx={{ fontSize: 40, color: "secondary.main" }}
                ></QueryStatsRoundedIcon>
                <Typography variant="h5" align="center">
                  Market history appears after item selection
                </Typography>
                <Typography color="text.secondary" align="center">
                  The chart will show lowest buyout and total quantity posted
                  for the selected commodity.
                </Typography>
              </Stack>
            </Box>
          ) : null}

          {!isError && itemName && !hasHistory ? (
            <Alert severity="info">
              No price history is available yet for this item.
            </Alert>
          ) : null}

          {!isError && itemName && hasHistory ? (
            <Box sx={{ width: "100%", height: { xs: 320, md: 380 } }}>
              <ResponsiveContainer width="100%" height="100%">
                <ComposedChart
                  data={priceQuantityData}
                  margin={{ top: 10, right: 4, left: 0, bottom: 4 }}
                >
                  <CartesianGrid
                    stroke={theme.palette.divider}
                    strokeDasharray="4 4"
                    vertical={false}
                  ></CartesianGrid>

                  <XAxis
                    dataKey="fetchedAtUtc"
                    tickFormatter={formatLocalDateShort}
                    axisLine={false}
                    tickLine={false}
                    minTickGap={24}
                    tick={{
                      fill: theme.palette.text.secondary,
                      fontSize: 12,
                    }}
                  ></XAxis>

                  <YAxis
                    yAxisId="price"
                    orientation="left"
                    axisLine={false}
                    tickLine={false}
                    width={112}
                    tick={renderPriceAxisTick}
                  ></YAxis>

                  <YAxis
                    yAxisId="quantity"
                    orientation="right"
                    axisLine={false}
                    tickLine={false}
                    width={64}
                    tick={{
                      fill: theme.palette.text.secondary,
                      fontSize: 12,
                    }}
                  ></YAxis>

                  <Tooltip
                    labelFormatter={formatLocalDateLong}
                    contentStyle={{
                      borderRadius: 14,
                      border: `1px solid ${theme.palette.divider}`,
                      backgroundColor: "#111a2d",
                      color: theme.palette.text.primary,
                      boxShadow: "0 16px 32px rgba(2, 7, 15, 0.4)",
                    }}
                    formatter={(value, name) => {
                      if (name === "Lowest buyout") {
                        if (typeof value !== "number") {
                          return ["No data", "Lowest buyout"];
                        }

                        return [
                          <CurrencyAmount
                            key={value}
                            unitPrice={value}
                            iconSize={14}
                            amountSx={{ fontSize: "14px" }}
                          ></CurrencyAmount>,
                          "Lowest buyout",
                        ];
                      }

                      if (typeof value !== "number") {
                        return ["No data", "Quantity posted"];
                      }

                      return [value.toLocaleString(), "Quantity posted"];
                    }}
                  ></Tooltip>

                  <Legend
                    verticalAlign="top"
                    align="right"
                    iconType="circle"
                    formatter={(value) => {
                      if (value === "Lowest buyout") {
                        return (
                          <span style={{ color: theme.palette.info.main }}>
                            {value}
                          </span>
                        );
                      }

                      return value;
                    }}
                    wrapperStyle={{
                      paddingBottom: 16,
                      color: theme.palette.text.secondary,
                    }}
                  ></Legend>

                  <Bar
                    yAxisId="quantity"
                    dataKey="totalQuantityPosted"
                    name="Quantity posted"
                    fill="rgba(75, 141, 228, 0.68)"
                    radius={[8, 8, 0, 0]}
                  ></Bar>

                  <Line
                    yAxisId="price"
                    type="monotone"
                    dataKey="lowestUnitPrice"
                    name="Lowest buyout"
                    stroke={theme.palette.info.main}
                    strokeWidth={3}
                    dot={false}
                    activeDot={{
                      r: 5,
                      fill: theme.palette.info.dark,
                    }}
                    connectNulls
                  ></Line>
                </ComposedChart>
              </ResponsiveContainer>
            </Box>
          ) : null}
        </Stack>
      </CardContent>
    </Card>
  );
}
