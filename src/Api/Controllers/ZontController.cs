using CottageSensorAggregator.ZontApi;
using Microsoft.AspNetCore.Mvc;

namespace CottageSensorAggregator.Api.Controllers;

public class ZontController(ZontRepository zontRepository) : Controller
{
    [HttpGet("auth")]
    public async Task<IActionResult> Authorize()
    {
        await zontRepository.AuthorizeAsync();
        return Ok();
    }

    [HttpGet("authtokens")]
    public IAsyncEnumerable<string> AuthTokens()
    {
        return zontRepository.GetTokensAsync();
    }

    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices()
    {
        return Ok(await zontRepository.GetDevices());
    }
}
