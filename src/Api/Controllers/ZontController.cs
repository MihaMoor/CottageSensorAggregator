using Microsoft.AspNetCore.Mvc;
using ZontApi;

namespace Api.Controllers;

public class ZontController(ZontRepository zontRepository) : Controller
{
    [HttpGet("auth")]
    public async Task<IActionResult> Authorize()
    {
        await zontRepository.AuthorizeAsync();
        return Ok(zontRepository.token);
    }

    [HttpGet("authtokens")]
    public async Task<IActionResult> AuthTokens()
    {
        var result = zontRepository.GetTokensAsync();
        return Ok(result);
    }
}
