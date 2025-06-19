using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models;
using APBD_Project.Models.RrsDbModels;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.Services;

public class ClientService : IClientService
{
    private readonly RrsContext _rrsContext;

    public ClientService(RrsContext rrsContext)
    {
        _rrsContext = rrsContext;
    }

    public async Task<Response> AddCompanyAsync(CompanyCreateDto? createDto, CancellationToken cancellationToken)
    {
        if (createDto == null)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid company data."
            };
        }

        var company = new Company
        {
            Email = createDto.Email,
            Address = createDto.Address,
            PhoneNumber = createDto.PhoneNumber,
            CompanyName = createDto.CompanyName,
            Krs = createDto.Krs
        };

        _rrsContext.Companies.Add(company);
        await _rrsContext.SaveChangesAsync(cancellationToken);

        return new Response
        {
            Success = true,
            Message = "Company created successfully."
        };
    }

    public async Task<Response> AddPersonAsync(PersonCreateDto? createDto, CancellationToken cancellationToken)
    {
        if (createDto == null)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid person data."
            };
        }

        var person = new Person
        {
            Email = createDto.Email,
            Address = createDto.Address,
            PhoneNumber = createDto.PhoneNumber,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Pesel = createDto.Pesel
        };

        _rrsContext.Persons.Add(person);
        await _rrsContext.SaveChangesAsync(cancellationToken);

        return new Response
        {
            Success = true,
            Message = "Person created successfully."
        };
    }

    public async Task<Response> UpdatePersonAsync(int id, PersonUpdateDto? updateDto,
        CancellationToken cancellationToken)
    {
        if (updateDto == null)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid person data."
            };
        }

        var person = await _rrsContext.Persons.FirstOrDefaultAsync(p => p.ClientId == id, cancellationToken);
        if (person == null || person.IsDeleted)
        {
            throw new NotFoundException("Person not found.");
        }

        if (updateDto.Email != null)
        {
            
                if (! await IsUniqueEmailAsync(updateDto.Email, person, cancellationToken))
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Email already exists."
                    };
                }

                person.Email = updateDto.Email;
            
        }

        if (updateDto.Address != null)
            person.Address = updateDto.Address;

        if (updateDto.PhoneNumber != null)
            person.PhoneNumber = updateDto.PhoneNumber;

        if (updateDto.FirstName != null)
            person.FirstName = updateDto.FirstName;

        if (updateDto.LastName != null)
            person.LastName = updateDto.LastName;

        _rrsContext.Persons.Update(person);

        await _rrsContext.SaveChangesAsync(cancellationToken);

        return new Response
        {
            Success = true,
            Message = "Person updated successfully."
        };
    }

    public async Task<Response> UpdateCompanyAsync(int id, CompanyUpdateDto? updateDto,
        CancellationToken cancellationToken)
    {
        if (updateDto == null)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid company data."
            };
        }

        var company = await _rrsContext.Companies.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);
        if (company == null || company.IsDeleted)
        {
            throw new NotFoundException("Company not found.");
        }

        if (updateDto.Email != null)
        {
            if (!await IsUniqueEmailAsync(updateDto.Email, company, cancellationToken))
            {
                return new Response
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }
            company.Email = updateDto.Email;
        }

        if (updateDto.Address != null)
            company.Address = updateDto.Address;

        if (updateDto.PhoneNumber != null)
            company.PhoneNumber = updateDto.PhoneNumber;

        if (updateDto.CompanyName != null)
            company.CompanyName = updateDto.CompanyName;

        _rrsContext.Companies.Update(company);

        await _rrsContext.SaveChangesAsync(cancellationToken);

        return new Response
        {
            Success = true,
            Message = "Company updated successfully."
        };
    }
    
    public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
    {
        var found = await _rrsContext.Clients.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);
        if (found == null)
        {
            throw new NotFoundException("Client not found.");
        }
        found.IsDeleted = true;
        await _rrsContext.SaveChangesAsync(cancellationToken:cancellationToken);
    }

    private async Task<bool> IsUniqueEmailAsync(string email, Client client, CancellationToken cancellationToken)
    {
        return ! await _rrsContext.Clients.AnyAsync(c => c.Email == email && c.ClientId != client.ClientId, cancellationToken);
    }
}