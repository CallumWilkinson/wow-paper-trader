//TODO: write unit test for this
export function formatLocalDate(utcString: string): string {
  const normalizedUtcString = utcString.endsWith("Z")
    ? utcString
    : `${utcString}Z`;

  const date = new Date(normalizedUtcString);

  const exactDate = date.toLocaleTimeString(undefined, {
    hour: "numeric",
    minute: "2-digit",
  });

  return exactDate;
}
