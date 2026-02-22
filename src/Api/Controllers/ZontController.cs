using CottageSensorAggregator.ZontApi;
using Microsoft.AspNetCore.Mvc;

namespace CottageSensorAggregator.Api.Controllers;

public class ZontController(ZontRepository zontRepository) : Controller
{
    [HttpDelete("delete-tokens")]
    public async Task<IActionResult> Authorize(
        [FromBody] string[] tokenIds,
        CancellationToken cancellationToken = default)
    {
        await zontRepository.DeleteTokensAsync(tokenIds, cancellationToken);
        return NoContent();
    }

    [HttpGet("authtokens")]
    public IAsyncEnumerable<string> AuthTokens(CancellationToken cancellationToken = default)
    {
        return zontRepository.GetTokensAsync(cancellationToken);
    }

    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices(CancellationToken cancellationToken = default)
    {
        return Ok(await zontRepository.GetDevicesAsync(cancellationToken));
    }
}
