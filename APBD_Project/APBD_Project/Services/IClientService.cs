using APBD_Project.DAL;
using APBD_Project.Dto;

namespace APBD_Project.Services;

public interface IClientService
{
    public Task<Response> AddCompanyAsync(CompanyCreateDto? createDto, CancellationToken cancellationToken);
    public Task<Response> AddPersonAsync(PersonCreateDto? createDto, CancellationToken cancellationToken);

    public Task<Response> UpdatePersonAsync(int id, PersonUpdateDto? updateDto,
        CancellationToken cancellationToken);
    public Task<Response> UpdateCompanyAsync(int id, CompanyUpdateDto? updateDto,
        CancellationToken cancellationToken);

    public Task DeleteClientAsync(int id, CancellationToken cancellationToken);
}