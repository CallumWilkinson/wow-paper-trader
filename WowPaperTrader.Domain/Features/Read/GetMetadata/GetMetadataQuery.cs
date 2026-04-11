using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Read.GetMetadata;

public sealed class GetMetadataQuery(long itemId) : IQuery<MetadataResponse>
{
    public readonly long ItemId = itemId;
}