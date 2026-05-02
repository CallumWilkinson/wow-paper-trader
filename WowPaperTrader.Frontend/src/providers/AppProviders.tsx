import { useState } from "react";
import type { ReactNode } from "react";
import { CssBaseline, ThemeProvider, alpha, createTheme } from "@mui/material";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

interface AppProvidersProps {
  children: ReactNode;
}

const midnightBlue = "#4b8de4";
const midnightBlueLight = "#7fb4ff";
const midnightBlueDark = "#2d5f9f";
const midnightPurple = "#9278ff";
const midnightPurpleLight = "#bbadff";
const midnightPurpleDark = "#6851d4";

const theme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: midnightPurple,
      light: midnightPurpleLight,
      dark: midnightPurpleDark,
      contrastText: "#ffffff",
    },
    secondary: {
      main: midnightBlue,
      light: midnightBlueLight,
      dark: midnightBlueDark,
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
    divider: alpha(midnightBlue, 0.18),
    success: {
      main: "#5fa96e",
    },
    info: {
      main: midnightPurple,
      light: midnightPurpleLight,
      dark: midnightPurpleDark,
    },
    error: {
      main: "#dd6848",
    },
    warning: {
      main: midnightPurple,
      light: midnightPurpleLight,
      dark: midnightPurpleDark,
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
          border: `1px solid ${alpha(midnightBlue, 0.14)}`,
          boxShadow: "0 24px 54px rgba(2, 7, 15, 0.42)",
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          backgroundImage: "none",
          backgroundColor: "rgba(14, 21, 35, 0.94)",
          border: `1px solid ${alpha(midnightBlue, 0.14)}`,
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
          border: `1px solid ${alpha(midnightPurple, 0.28)}`,
          backgroundColor: alpha(midnightPurple, 0.12),
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
          transition: "border-color 160ms ease",
          "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: midnightPurple,
          },
          "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
            borderColor: alpha(midnightBlue, 0.22),
            borderWidth: 1,
          },
        },
        notchedOutline: {
          borderColor: alpha(midnightBlue, 0.22),
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
