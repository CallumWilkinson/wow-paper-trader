import { useState } from "react";
import type { ReactNode } from "react";
import { CssBaseline, ThemeProvider, createTheme } from "@mui/material";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

interface AppProvidersProps {
  children: ReactNode;
}

const theme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#f0b400",
      light: "#ffd15a",
      dark: "#b47a00",
      contrastText: "#120c06",
    },
    secondary: {
      main: "#2f67b1",
      light: "#5090dd",
      dark: "#1d3e69",
      contrastText: "#ffffff",
    },
    background: {
      default: "#090f18",
      paper: "#121a2c",
    },
    text: {
      primary: "#ffffff",
      secondary: "#d0d6e0",
    },
    divider: "rgba(255, 194, 54, 0.16)",
    success: {
      main: "#5fa96e",
    },
    info: {
      main: "#4b8de4",
    },
    error: {
      main: "#dd6848",
    },
    warning: {
      main: "#f0b400",
    },
  },
  shape: {
    borderRadius: 20,
  },
  typography: {
    fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
    h1: {
      fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
      fontWeight: 800,
      letterSpacing: "-0.05em",
      lineHeight: 1.05,
    },
    h2: {
      fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
      fontWeight: 800,
      letterSpacing: "-0.045em",
      lineHeight: 1.02,
    },
    h3: {
      fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
      fontWeight: 800,
      letterSpacing: "-0.04em",
      lineHeight: 1.08,
    },
    h4: {
      fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
      fontWeight: 800,
      letterSpacing: "-0.03em",
    },
    h5: {
      fontFamily: '"Arial", "Helvetica Neue", "Segoe UI", sans-serif',
      fontWeight: 800,
      letterSpacing: "-0.02em",
    },
    overline: {
      fontWeight: 700,
      letterSpacing: "0.14em",
    },
    body1: {
      lineHeight: 1.6,
    },
    body2: {
      lineHeight: 1.55,
    },
    button: {
      fontWeight: 700,
      textTransform: "uppercase",
      letterSpacing: "0.03em",
    },
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          backgroundRepeat: "no-repeat",
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: "none",
          backgroundColor: "rgba(14, 21, 35, 0.94)",
          border: "1px solid rgba(255, 194, 54, 0.12)",
          boxShadow: "0 24px 54px rgba(2, 7, 15, 0.42)",
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          backgroundImage: "none",
          backgroundColor: "rgba(14, 21, 35, 0.94)",
          border: "1px solid rgba(255, 194, 54, 0.12)",
          boxShadow: "0 24px 54px rgba(2, 7, 15, 0.42)",
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 999,
          paddingInline: 18,
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: 999,
          border: "1px solid rgba(255, 194, 54, 0.28)",
          backgroundColor: "rgba(255, 194, 54, 0.08)",
          color: "#ffffff",
          fontWeight: 600,
        },
      },
    },
    MuiAlert: {
      styleOverrides: {
        root: {
          borderRadius: 16,
          alignItems: "center",
          border: "1px solid rgba(255, 255, 255, 0.08)",
        },
      },
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          borderRadius: 16,
          backgroundColor: "rgba(9, 14, 23, 0.96)",
          transition: "box-shadow 160ms ease, border-color 160ms ease",
          "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: "#f0b400",
          },
          "&.Mui-focused": {
            boxShadow: "0 0 0 4px rgba(240, 180, 0, 0.16)",
          },
        },
        notchedOutline: {
          borderColor: "rgba(255, 194, 54, 0.2)",
        },
        input: {
          paddingTop: 16,
          paddingBottom: 16,
        },
      },
    },
    MuiInputLabel: {
      styleOverrides: {
        root: {
          color: "#d0d6e0",
        },
      },
    },
    MuiFormHelperText: {
      styleOverrides: {
        root: {
          color: "#aeb8c9",
        },
      },
    },
  },
});

export default function AppProviders(props: AppProvidersProps) {
  const { children } = props;

  const [queryClient] = useState(() => {
    return new QueryClient({
      defaultOptions: {
        queries: {
          refetchOnWindowFocus: false,
        },
      },
    });
  });

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline></CssBaseline>
        {children}
      </ThemeProvider>
      <ReactQueryDevtools initialIsOpen={false}></ReactQueryDevtools>
    </QueryClientProvider>
  );
}
