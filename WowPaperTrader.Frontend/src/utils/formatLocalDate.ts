//TODO: write unit test for this
export function formatLocalDateLong(
  utcString: string | React.ReactNode,
): string | React.ReactNode {
  if (typeof utcString == "string") {
    const normalizedUtcString = utcString.endsWith("Z")
      ? utcString
      : `${utcString}Z`;

    const date = new Date(normalizedUtcString);

    return date.toLocaleString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "numeric",
      minute: "2-digit",
    });
  }
}
//TODO: write unit test for this
export function formatLocalDateShort(utcString: string): string {
  const normalizedUtcString = utcString.endsWith("Z")
    ? utcString
    : `${utcString}Z`;

  const date = new Date(normalizedUtcString);

  return date.toLocaleString(undefined, {
    month: "numeric",
    day: "numeric",
  });
}
