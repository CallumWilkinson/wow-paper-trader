using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Application.Read.UseCases;

namespace WowPaperTrader.Api.Read.Controllers;


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