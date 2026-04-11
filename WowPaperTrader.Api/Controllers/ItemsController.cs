using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Application.Features.Read.GetMetadata;
using WowPaperTrader.Application.Features.Read.ItemSearch;
using WowPaperTrader.Application.Features.Read.LowestPrice;
using WowPaperTrader.Application.Features.Write.UpdateItems;

namespace WowPaperTrader.Api.Controllers;

[ApiController]
[Route("api/v1/items")]
public sealed class ItemsController(
    LowestPriceQueryHandler getLowestPriceQueryHandler,
    ItemSearchQueryHandler itemSearchQueryHandler,
    GetMetadataQueryHandler getMetadataQueryHandler,
    UpdateItemsCommandHandler updateItemsCommandHandler)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ItemSearchResponse>>> SearchItems([FromQuery] string itemName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName)) return BadRequest("Name is required.");
        
        var query = new ItemSearchQuery(itemName);

        var result = await itemSearchQueryHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{itemId:long}")]
    public async Task<ActionResult<MetadataResponse>> GetMetadata(long itemId, CancellationToken cancellationToken)
    {
        if (itemId <= 0) return BadRequest("Invalid itemId.");
        
        var query = new GetMetadataQuery(itemId);

        var result = await getMetadataQueryHandler.HandleAsync(query, cancellationToken);

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

        var result = await getLowestPriceQueryHandler.HandleAsync(query, cancellationToken);

        if (result is null) return NotFound();

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateItemMetaData(CancellationToken cancellationToken)
    {
        var command = new UpdateItemsCommand();
        
        await updateItemsCommandHandler.HandleAsync(command, cancellationToken);

        return Ok();
    }
}