public enum IngestionRunStatus
{
    Started,
    TokenRequested,
    Finished,
    Failed
}

public sealed class IngestionRun
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime StartedAtUtc { get; init; } = DateTime.UtcNow;
    public DateTime LastUpdatedAtUtc { get; private set; }

    public IngestionRunStatus Status { get; private set; } = IngestionRunStatus.Started;

    public void TransitionTo(IngestionRunStatus nextStatus, DateTime utcNow)
    {
        Status = nextStatus;
        LastUpdatedAtUtc = utcNow;
    }

}

