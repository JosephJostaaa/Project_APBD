using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("/company")]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await _clientService.AddCompanyAsync(createDto, cancellationToken);
        if (result.Success)
        {
            return Created();
        }
        return BadRequest(result.Message);
    }
    
    [HttpPost("/individual")]
    public async Task<IActionResult> CreateIndividual([FromBody] PersonCreateDto individualDto,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await _clientService.AddPersonAsync(individualDto, cancellationToken);
        if (result.Success)
        {
            return Created();
        }
        return BadRequest(result.Message);
    }

    [HttpPut("/company/{id}")]
    public async Task<IActionResult> UpdateCompany([FromBody] CompanyUpdateDto updateDto,
        int id,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await _clientService.UpdateCompanyAsync(id, updateDto, cancellationToken);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result.Message);
    }
    
    [HttpPut("/individual/{id}")]
    public async Task<IActionResult> UpdateCompany([FromBody] PersonUpdateDto updateDto,
        int id,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var result = await _clientService.UpdatePersonAsync(id, updateDto, cancellationToken);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result.Message);
    }

    [HttpDelete("/{id}")]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        await _clientService.DeleteClientAsync(id, cancellationToken);
        return NoContent();
    }
}