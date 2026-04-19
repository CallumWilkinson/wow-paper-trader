import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import AppProviders from "./providers/AppProviders";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AppProviders>
      <App></App>
    </AppProviders>
  </StrictMode>,
);
