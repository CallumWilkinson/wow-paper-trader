using Microsoft.AspNetCore.Mvc;
using WowPaperTrader.Domain.UseCases;

namespace WowPaperTrader.Api.Controllers;


[ApiController]
[Route("api/v1/items")]
public sealed class UpdateItemMetaDataController : ControllerBase
{
    private readonly UpdateItemMetaDataUseCase _useCase;

    public UpdateItemMetaDataController(UpdateItemMetaDataUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost()]
    public async Task<IActionResult> UpdateItemMetaData(CancellationToken cancellationToken)
    {
        await _useCase.ExecuteAsync(cancellationToken);

        return Ok();
    }

}