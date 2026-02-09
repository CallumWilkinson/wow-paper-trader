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

    public DateTime FinishedAtUtc { get; private set; }

    public IngestionRunStatus Status { get; private set; } = IngestionRunStatus.Started;

    public string? ErrorMessage { get; private set; }

    public string? ErrorStack { get; private set; }

    public void TransitionTo(IngestionRunStatus nextStatus, DateTime utcNow)
    {
        Status = nextStatus;
        LastUpdatedAtUtc = utcNow;

        if (Status == IngestionRunStatus.Finished || Status == IngestionRunStatus.Failed)
        {
            FinishedAtUtc = utcNow;
        }
    }

    public void MarkFailed(Exception ex, DateTime utcNow)
    {
        TransitionTo(IngestionRunStatus.Failed, utcNow);

        ErrorMessage = ex.Message;
        ErrorStack = ex.ToString();
    }

}

