using APBD_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Employee")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [HttpGet("/{productId}")]
    public async Task<IActionResult> GetRevenueByProductId(int productId, [FromQuery] string? currencyCode, CancellationToken cancellationToken)
    {
        var revenue = await _revenueService.GetRevenueAsync(currencyCode, productId, cancellationToken);
        return Ok(revenue);
    }
    
    [HttpGet("total")]
    public async Task<IActionResult> GetTotalRevenue([FromQuery] string? currencyCode, CancellationToken cancellationToken)
    {
        var totalRevenue = await _revenueService.GetRevenueAsync(currencyCode, null, cancellationToken);
        return Ok(totalRevenue);
    }
    
    [HttpGet("/predicted/{productId}")]
    public async Task<IActionResult> GetPredictedRevenueByProductId(int productId, [FromQuery] string? currencyCode, CancellationToken cancellationToken)
    {
        var revenue = await _revenueService.GetPredictedRevenueAsync(currencyCode, productId, cancellationToken);
        return Ok(revenue);
    }
    
    [HttpGet("predicted/total")]
    public async Task<IActionResult> GetPredictedTotalRevenue([FromQuery] string? currencyCode, CancellationToken cancellationToken)
    {
        var totalRevenue = await _revenueService.GetPredictedRevenueAsync(currencyCode, null, cancellationToken);
        return Ok(totalRevenue);
    }
}