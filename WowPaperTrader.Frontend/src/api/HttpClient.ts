import axios from "axios";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

if (!apiBaseUrl) {
  throw new Error(
    "VITE_API_BASE_URL is missing. Set it in your local .env file or GitHub deployment variables.",
  );
}

//axios
export const httpClient = axios.create({
  baseURL: apiBaseUrl,
  timeout: 10_000,
});
