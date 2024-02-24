using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;
        private readonly ICacheService _cacheService;

        public SchoolService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<GetAllSchoolsResponse> GetAllSchools()
        {
            var cacheData = _cacheService.GetData<GetAllSchoolsResponse>("schools");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.School
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            GetAllSchoolsResponse response = new() { List = list };

            _cacheService.SetData("schools", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<SchoolModel?> GetSchoolById(IdRequest request)
        {
            var cacheData = _cacheService.GetData<SchoolModel>($"schoolId{request.Id}");

            if (cacheData is not null)
                return cacheData;

            SchoolModel? response = await _context.School
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefaultAsync();

            _cacheService.SetData($"schoolId{request.Id}", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<SchoolModel?> GetSchoolByPrincipalId(IdRequest request)
        {
            var cacheData = _cacheService.GetData<SchoolModel>($"schoolPrincipalId{request.Id}");

            if (cacheData is not null)
                return cacheData;

            SchoolModel? response = await _context.Principal
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.School.Id,
                    Name = x.School.Name
                }).FirstOrDefaultAsync();

            _cacheService.SetData($"schoolPrincipalId{request.Id}", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<SchoolModel?> GetSchoolByProfessorId(IdRequest request)
        {
            var cacheData = _cacheService.GetData<SchoolModel>($"schoolProfessorId{request.Id}");

            if (cacheData is not null)
                return cacheData;

            SchoolModel? response = await _context.Professor
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.School.Id,
                    Name = x.School.Name
                }).FirstOrDefaultAsync();

            _cacheService.SetData($"schoolProfessorId{request.Id}", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<CreateSchoolResponse> CreateSchool(CreateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new CreateSchoolResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = "Invalid name."
                    },
                    GetAllSchoolsResponse = await GetAllSchools()
                };
            }

            try
            {
                Entities.School schoolDto = new() { Name = request.Name };

                var addedObj = _context.Add(schoolDto);

                int rowsChanged = await _context.SaveChangesAsync();

                _cacheService.SetData($"schoolId{schoolDto.Id}", addedObj.Entity, DateTimeOffset.Now.AddMonths(1));
                _cacheService.RemoveData("schools");              
                _cacheService.RemoveData("schoolsDropDownForProfessor");

                if (rowsChanged > 0)
                {
                    return new CreateSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. School created successfully."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
                else
                {
                    return new CreateSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
            }
            catch (Exception ex)
            {
                return new CreateSchoolResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSchoolsResponse = await GetAllSchools()
                };
            }
        }

        public async Task<UpdateSchoolResponse> UpdateSchool(UpdateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new UpdateSchoolResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = "Invalid name."
                    },
                    GetAllSchoolsResponse = await GetAllSchools()
                };
            }

            try
            {
                var schoolDto = await _context.School.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (schoolDto is null)
                {
                    return new UpdateSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"School with ID {request.Id} not found."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }

                schoolDto.Name = request.Name;

                int rowsChanged = await _context.SaveChangesAsync();

                _cacheService.SetData($"schoolId{request.Id}", schoolDto, DateTimeOffset.Now.AddMonths(1));
                _cacheService.RemoveData("schools");
                _cacheService.RemoveData("professors");
                _cacheService.RemoveData("schoolsDropDownForProfessor");

                if (rowsChanged > 0)
                {
                    return new UpdateSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. School updated successfully."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
                else
                {
                    return new UpdateSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
            }
            catch (Exception ex)
            {
                return new UpdateSchoolResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSchoolsResponse = await GetAllSchools()
                };
            }
        }

        public async Task<DeleteSchoolResponse> DeleteSchool(IdRequest request)
        {
            try
            {
                var schoolDto = await _context.School.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (schoolDto is null)
                {
                    return new DeleteSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"School with ID {request.Id} not found."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }

                _context.Remove(schoolDto);

                int rowsChanged = await _context.SaveChangesAsync();

                _cacheService.RemoveData($"schoolId{schoolDto.Id}");
                _cacheService.RemoveData("schools");
                _cacheService.RemoveData("schoolsDropDownForProfessor");

                if (rowsChanged > 0)
                {
                    return new DeleteSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. School deleted successfully."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
                else
                {
                    return new DeleteSchoolResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSchoolsResponse = await GetAllSchools()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteSchoolResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSchoolsResponse = await GetAllSchools()
                };
            }
        }
    }
}
