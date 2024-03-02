using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;
        private readonly ICacheService _cacheService;

        public PrincipalService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<OperationStatusResponse> CreatePrincipal(CreatePrincipalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                Entities.Principal principalDto = new() { Name = request.Name, SchoolId = request.SchoolId };

                _context.Add(principalDto);
                await _context.SaveChangesAsync();

                _cacheService.RemoveData("principals");
                _cacheService.RemoveData("schoolsDropDownForPrincipal");

                return new OperationStatusResponse { IsSuccessful = true, Message = "Success. Principal created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<DeletePrincipalResponse> DeletePrincipal(IdRequest request)
        {
            try
            {
                var principalDto = await _context.Principal.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (principalDto is null)
                {
                    return new DeletePrincipalResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"Principal with ID {request.Id} not found."
                        },
                        GetAllPrincipalsResponse = await GetAllPrincipals()
                    };
                }

                _context.Remove(principalDto);

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    _cacheService.RemoveData("principals");

                    return new DeletePrincipalResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Principal deleted successfully."
                        },
                        GetAllPrincipalsResponse = await GetAllPrincipals()
                    };
                }
                else
                {
                    return new DeletePrincipalResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllPrincipalsResponse = await GetAllPrincipals()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeletePrincipalResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllPrincipalsResponse = await GetAllPrincipals()
                };
            }
        }

        public async Task<GetAllPrincipalsResponse> GetAllPrincipals()
        {
            var cacheData = _cacheService.GetData<GetAllPrincipalsResponse>("principals");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.Principal
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            GetAllPrincipalsResponse response = new() { List = list };

            _cacheService.SetData("principals", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<PrincipalModel?> GetPrincipalById(IdRequest request)
        {
            var response = await _context.Principal
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<PrincipalModel?> GetPrincipalBySchoolId(IdRequest request)
        {
            var response = await _context.Principal
                .Where(x => x.SchoolId == request.Id)
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<OperationStatusResponse> UpdatePrincipal(UpdatePrincipalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                var principalDto = await _context.Principal.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (principalDto is null)
                    return new OperationStatusResponse { IsSuccessful = false, Message = $"Principal with ID {request.Id} not found." };

                principalDto.Name = request.Name;
                principalDto.SchoolId = request.SchoolId;
                await _context.SaveChangesAsync();

                _cacheService.RemoveData("principals");

                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Principal with ID {principalDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<DropDownResponse> GetSchoolsDropDown()
        {
            var cacheData = _cacheService.GetData<DropDownResponse>("schoolsDropDownForPrincipal");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.School
                .Where(x => !_context.Principal.Any(y => y.SchoolId == x.Id))
                .AsNoTracking()
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            DropDownResponse response = new() { List = list };

            _cacheService.SetData("schoolsDropDownForPrincipal", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<InitUpdatePrincipalResponse> InitUpdatePrincipal(IdRequest request)
        {
            var principal = await _context.Principal
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new PrincipalDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolId = x.School.Id
                }).FirstOrDefaultAsync();

            var schoolAvailableOptions = await _context.School
                .Where(x => !_context.Principal.Any(y => y.SchoolId == x.Id))
                .AsNoTracking()
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            var currentSchool = await _context.School
                .Where(x => x.Id == principal!.SchoolId)
                .AsNoTracking()
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return new InitUpdatePrincipalResponse
            {
                Principal = principal!,
                SchoolOptions = [.. schoolAvailableOptions, .. currentSchool]
            };
        }
    }
}