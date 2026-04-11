using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.GetMetadata;

public sealed class GetMetadataQuery(long itemId) : IQuery<MetadataResponse>
{
    public readonly long ItemId = itemId;
}