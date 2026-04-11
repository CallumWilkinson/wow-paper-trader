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
    private readonly GetCurrentLowestUnitPriceByItemIdUseCase _getLowestPriceUseCase;
    private readonly GetMetadataQueryHandler _getMetadataUseCase;
    private readonly UpdateItemMetaDataUseCase _updateMetadataUseCase;

    public ItemsController(
        GetCurrentLowestUnitPriceByItemIdUseCase getLowestPriceUseCase,
        ItemSearchQueryHandler itemSearchQueryHandler,
        GetMetadataQueryHandler getMetadataUseCase,
        UpdateItemMetaDataUseCase updateMetadataUseCase)
    {
        _getLowestPriceUseCase = getLowestPriceUseCase;
        _itemSearchQueryHandler = itemSearchQueryHandler;
        _getMetadataUseCase = getMetadataUseCase;
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
    public async Task<ActionResult<MetadataResponse>> GetTooltipAndLowestBuyoutPrice(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0) return BadRequest("Invalid itemId.");

        var result = await _getMetadataUseCase.ExecuteAsync(itemId, cancellationToken);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpGet("{itemId:long}/auctions/lowest")]
    public async Task<ActionResult<CurrentLowestUnitPriceResponse>> GetCurrentLowestUnitPrice(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0) return BadRequest("Invalid itemId.");

        var result = await _getLowestPriceUseCase.ExecuteAsync(itemId, cancellationToken);

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