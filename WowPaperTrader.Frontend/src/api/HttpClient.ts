import axios from "axios";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

if (!apiBaseUrl) {
  throw new Error(
    "VITE_API_BASE_URL is missing. Ensure .env file exists for HTTPClient setup",
  );
}

export const httpClient = axios.create({
  baseURL: apiBaseUrl,
  timeout: 10_000,
});
