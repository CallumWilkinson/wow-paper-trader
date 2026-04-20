//TODO: write unit test for this
export function formatPriceTimestamp(utcString: string): string {
  const normalizedUtcString = utcString.endsWith("Z")
    ? utcString
    : `${utcString}Z`;

  const date = new Date(normalizedUtcString);
  const now = new Date();

  const diffMs = now.getTime() - date.getTime();
  const diffSeconds = Math.floor(diffMs / 1000);

  let relative: string;

  if (diffSeconds < 60) {
    relative = "just now";
  } else {
    const diffMinutes = Math.floor(diffSeconds / 60);

    if (diffMinutes < 60) {
      relative = `${diffMinutes} minute${diffMinutes === 1 ? "" : "s"} ago`;
    } else {
      const diffHours = Math.floor(diffMinutes / 60);

      if (diffHours < 24) {
        relative = `${diffHours} hour${diffHours === 1 ? "" : "s"} ago`;
      } else {
        const diffDays = Math.floor(diffHours / 24);

        relative = `${diffDays} day${diffDays === 1 ? "" : "s"} ago`;
      }
    }
  }

  const exact = date.toLocaleTimeString(undefined, {
    hour: "numeric",
    minute: "2-digit",
  });

  return `Price updated ${relative} (${exact})`;
}
