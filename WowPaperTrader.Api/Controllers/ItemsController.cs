using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Domain.CommandHandlers;
using WowPaperTrader.Domain.Features.ItemSearch;
using WowPaperTrader.Domain.QueryHandlers;
using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Api.Controllers;

[ApiController]
[Route("api/v1/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly ItemSearchUseCase _itemSearchUseCase;
    private readonly GetCurrentLowestUnitPriceByItemIdUseCase _getLowestPriceUseCase;
    private readonly GetMetadataAndPriceByItemIdUseCase _getMetadataUseCase;
    private readonly UpdateItemMetaDataUseCase _updateMetadataUseCase;

    public ItemsController(
        GetCurrentLowestUnitPriceByItemIdUseCase getLowestPriceUseCase,
        ItemSearchUseCase itemSearchUseCase,
        GetMetadataAndPriceByItemIdUseCase getMetadataUseCase,
        UpdateItemMetaDataUseCase updateMetadataUseCase)
    {
        _getLowestPriceUseCase = getLowestPriceUseCase;
        _itemSearchUseCase = itemSearchUseCase;
        _getMetadataUseCase = getMetadataUseCase;
        _updateMetadataUseCase = updateMetadataUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemSearchResult>>> SearchItems([FromQuery] string itemName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName)) return BadRequest("Name is required.");

        var result = await _itemSearchUseCase.ExecuteAsync(itemName, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{itemId:long}")]
    public async Task<ActionResult<ItemMetadataAndPriceResult>> GetTooltipAndLowestBuyoutPrice(
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
    public async Task<ActionResult<CurrentLowestUnitPriceResult>> GetCurrentLowestUnitPrice(
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