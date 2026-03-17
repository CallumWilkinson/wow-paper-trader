using Microsoft.AspNetCore.Mvc;

namespace wow_paper_trader.Api.Read.Controllers;


[ApiController]
[Route("api/commodities")]
public sealed class CommoditiesController : ControllerBase
{
    private readonly GetCurrentLowestUnitPriceByItemIdUseCase _useCase;

    public CommoditiesController(GetCurrentLowestUnitPriceByItemIdUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{itemId:long}/current-lowest-unit-price")]
    public async Task<ActionResult<CurrentLowestUnitPriceResult>> GetCurrentLowestUnitPrice(
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
