using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;

        public PrincipalService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context)
        {
            _context = context;
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
            var response = await _context.Principal
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            return new GetAllPrincipalsResponse { List = response };
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
                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Principal with ID {principalDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}