namespace WowPaperTrader.Domain.Architecture;

public interface IQueryHandler<in IQuery, TResponse>
{
    Task<TResponse> ExecuteAsync(IQuery query, CancellationToken cancellationToken);
}