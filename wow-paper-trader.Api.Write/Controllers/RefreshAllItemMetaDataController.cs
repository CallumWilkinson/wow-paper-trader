using Microsoft.AspNetCore.Mvc;

namespace wow_paper_trader.Api.Write.Controllers;


[ApiController]
[Route("api/metadata")]
public sealed class RefreshAllItemMetaDataController : ControllerBase
{
    private readonly RefreshAllItemMetaDataUseCase _useCase;

    public RefreshAllItemMetaDataController(RefreshAllItemMetaDataUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("refresh-all")]
    public async Task<IActionResult> RefreshAllItemMetaData(CancellationToken cancellationToken)
    {
        await _useCase.ExecuteAsync(cancellationToken);

        return Ok();
    }

}