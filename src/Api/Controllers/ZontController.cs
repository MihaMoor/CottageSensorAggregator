using CottageSensorAggregator.ZontApi;
using Microsoft.AspNetCore.Mvc;

namespace CottageSensorAggregator.Api.Controllers;

public class ZontController(ZontRepository zontRepository) : Controller
{
    [HttpGet("auth")]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken = default)
    {
        await zontRepository.AuthorizeAsync(cancellationToken);
        return Ok();
    }

    [HttpGet("authtokens")]
    public IAsyncEnumerable<string> AuthTokens(CancellationToken cancellationToken = default)
    {
        return zontRepository.GetTokensAsync(cancellationToken);
    }

    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices(CancellationToken cancellationToken = default)
    {
        return Ok(await zontRepository.GetDevices(cancellationToken));
    }
}
