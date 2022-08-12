using Microsoft.AspNetCore.Mvc;

using Taxes.Api.Requests;
using Taxes.Api.Services;

namespace Taxes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TaxesController : ControllerBase
{
    private readonly ILogger<TaxesController> _logger;

    public TaxesController(ILogger<TaxesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Selic")]
    public async Task<IActionResult> SearchSelicTaxAsync
    (
        [FromQuery] TaxSearchRequest request, 
        [FromServices] SelicTaxSearchService service
    )
    {
        var result = await service.SearchByAsync(request);

        if (result == null || !result.Any()) return NoContent();

        return Ok(result);
    }
}
