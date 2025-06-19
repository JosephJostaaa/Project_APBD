using APBD_Project.Dto;
using APBD_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Employee")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddContract([FromBody] ContractCreateDto contractDto, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var result = await _contractService.CreateContractAsync(contractDto, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Created();
    }
    
    [HttpPost("/{id}/pay")]
    public async Task<IActionResult> PayContract(int id, [FromBody] PaymentDto paymentDto, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _contractService.MakePaymentAsync(id, paymentDto, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }
}