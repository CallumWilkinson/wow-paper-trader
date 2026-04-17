import { useEffect, useState } from "react";

export function useAutoRefresh(intervalMs: number = 60_000): number {
  const [now, setNow] = useState(() => {
    return Date.now();
  });

  useEffect(() => {
    const interval = setInterval(() => {
      setNow(Date.now());
    }, intervalMs);

    return () => clearInterval(interval);
  }, [intervalMs]);

  return now;
}
