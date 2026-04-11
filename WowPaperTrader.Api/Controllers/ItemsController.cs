using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Domain.Features.Read.GetMetadata;
using WowPaperTrader.Domain.Features.Read.ItemSearch;
using WowPaperTrader.Domain.Features.Read.LowestPrice;
using WowPaperTrader.Domain.Features.Write.UpdateItems;

namespace WowPaperTrader.Api.Controllers;

[ApiController]
[Route("api/v1/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly ItemSearchQueryHandler _itemSearchQueryHandler;
    private readonly LowestPriceQueryHandler _getLowestPriceQueryHandler;
    private readonly GetMetadataQueryHandler _getMetadataQueryHandler;
    private readonly UpdateItemMetaDataUseCase _updateMetadataUseCase;

    public ItemsController(
        LowestPriceQueryHandler getLowestPriceQueryHandler,
        ItemSearchQueryHandler itemSearchQueryHandler,
        GetMetadataQueryHandler getMetadataQueryHandler,
        UpdateItemMetaDataUseCase updateMetadataUseCase)
    {
        _getLowestPriceQueryHandler = getLowestPriceQueryHandler;
        _itemSearchQueryHandler = itemSearchQueryHandler;
        _getMetadataQueryHandler = getMetadataQueryHandler;
        _updateMetadataUseCase = updateMetadataUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemSearchResponse>>> SearchItems([FromQuery] string itemName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName)) return BadRequest("Name is required.");
        
        var query = new ItemSearchQuery(itemName);

        var result = await _itemSearchQueryHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{itemId:long}")]
    public async Task<ActionResult<MetadataResponse>> GetMetadata(long itemId, CancellationToken cancellationToken)
    {
        if (itemId <= 0) return BadRequest("Invalid itemId.");
        
        var query = new GetMetadataQuery(itemId);

        var result = await _getMetadataQueryHandler.HandleAsync(query, cancellationToken);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpGet("{itemId:long}/auctions/lowest")]
    public async Task<ActionResult<LowestPriceResponse>> GetLowestPrice(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0) return BadRequest("Invalid itemId.");
        
        var query = new LowestPriceQuery(itemId);

        var result = await _getLowestPriceQueryHandler.HandleAsync(query, cancellationToken);

        if (result is null) return NotFound();

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateItemMetaData(CancellationToken cancellationToken)
    {
        await _updateMetadataUseCase.ExecuteAsync(cancellationToken);

        return Ok();
    }
}