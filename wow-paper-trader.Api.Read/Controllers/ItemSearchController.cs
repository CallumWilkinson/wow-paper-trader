using Microsoft.AspNetCore.Mvc;
namespace wow_paper_trader.Api.Read.Controllers;

[ApiController]
[Route("api/items")]
public sealed class ItemSearchController : ControllerBase
{
    private readonly ItemSearchUseCase _useCase;

    public ItemSearchController(ItemSearchUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<ItemSearchResult>>> SearchItems([FromQuery] string itemName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            return BadRequest("Name is required.");
        }

        var result = await _useCase.ExecuteAsync(itemName, cancellationToken);

        return Ok(result);
    }
}