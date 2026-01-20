public sealed class IngestionRun
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime StartedAtUtc { get; init; } = DateTime.UtcNow;

    public string Status { get; init; } = "Started";

}