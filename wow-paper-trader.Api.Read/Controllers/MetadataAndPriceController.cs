using Microsoft.AspNetCore.Mvc;

namespace wow_paper_trader.Api.Read.Controllers;


[ApiController]
[Route("api/commodities")]
public sealed class MetadataAndPriceController : ControllerBase
{
    private readonly GetMetadataAndPriceByItemIdUseCase _useCase;

    public MetadataAndPriceController(GetMetadataAndPriceByItemIdUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{itemId:long}/metadata-and-price")]
    public async Task<ActionResult<ItemMetadataAndPriceResult>> GetMetadataAndPrice(
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
