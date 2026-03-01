using CottageSensorAggregator.ZontApi;
using Microsoft.AspNetCore.Mvc;

namespace CottageSensorAggregator.Api.Controllers;

/// <summary>
///
/// </summary>
/// <param name="zontRepository"></param>
[ApiController]
[Route("api/[controller]")]
public class ZontController(ZontRepository zontRepository) : ControllerBase
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="tokenIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("delete-tokens")]
    public async Task<IActionResult> DeleteTokens(
        [FromBody] string[] tokenIds,
        CancellationToken cancellationToken = default
    )
    {
        await zontRepository.DeleteTokensAsync(tokenIds, cancellationToken);
        return NoContent();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("authtokens")]
    public IAsyncEnumerable<string> AuthTokens(CancellationToken cancellationToken = default)
    {
        return zontRepository.GetTokensAsync(cancellationToken);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices(CancellationToken cancellationToken = default)
    {
        return Ok(await zontRepository.GetDevicesAsync(cancellationToken));
    }
}
