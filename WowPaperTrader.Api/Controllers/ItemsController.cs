using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Domain.UseCases;

namespace WowPaperTrader.Api.Controllers;

[ApiController]
[Route("api/v1/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly ItemSearchUseCase _itemSearchUseCase;
    private readonly GetCurrentLowestUnitPriceByItemIdUseCase _lowestPriceUseCase;
    private readonly GetMetadataAndPriceByItemIdUseCase _metadataUseCase;

    public ItemsController(
        GetCurrentLowestUnitPriceByItemIdUseCase lowestPriceUseCase,
        ItemSearchUseCase itemSearchUseCase,
        GetMetadataAndPriceByItemIdUseCase metadataUseCase)
    {
        _lowestPriceUseCase = lowestPriceUseCase;
        _itemSearchUseCase = itemSearchUseCase;
        _metadataUseCase = metadataUseCase;
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

        var result = await _metadataUseCase.ExecuteAsync(itemId, cancellationToken);

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

        var result = await _lowestPriceUseCase.ExecuteAsync(itemId, cancellationToken);

        if (result is null) return NotFound();

        return Ok(result);
    }
}