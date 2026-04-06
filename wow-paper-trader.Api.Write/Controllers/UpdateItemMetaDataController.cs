using Microsoft.AspNetCore.Mvc;

namespace wow_paper_trader.Api.Write.Controllers;


[ApiController]
[Route("api/metadata")]
public sealed class UpdateItemMetaDataController : ControllerBase
{
    private readonly UpdateItemMetaDataUseCase _useCase;

    public UpdateItemMetaDataController(UpdateItemMetaDataUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateItemMetaData(CancellationToken cancellationToken)
    {
        await _useCase.ExecuteAsync(cancellationToken);

        return Ok();
    }

}