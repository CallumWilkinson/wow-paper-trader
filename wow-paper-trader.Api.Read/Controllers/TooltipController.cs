using Microsoft.AspNetCore.Mvc;

namespace wow_paper_trader.Api.Read.Controllers;


[ApiController]
[Route("api/items/commodities")]
public sealed class TooltipController : ControllerBase
{
    private readonly GetMetadataAndPriceByItemIdUseCase _useCase;

    public TooltipController(GetMetadataAndPriceByItemIdUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("metadata/{itemId:long}")]
    public async Task<ActionResult<ItemMetadataAndPriceResult>> GetTooltipAndLowestBuyoutPrice(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0)
        {
            return BadRequest("Invalid itemId.");
        }

        var result = await _useCase.ExecuteAsync(itemId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
