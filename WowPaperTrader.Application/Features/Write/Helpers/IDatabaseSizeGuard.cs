namespace WowPaperTrader.Application.Features.Write.Helpers;

public interface IDatabaseSizeGuard
{
    Task<bool> IsDatabaseAboveAzureFreeLimit(CancellationToken cancellationToken);
}