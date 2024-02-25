using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;
        private readonly ICacheService _cacheService;

        public SubjectService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<CreateSubjectResponse> CreateSubject(CreateSubjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new CreateSubjectResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = "Invalid name."
                    },
                    GetAllSubjectsResponse = await GetAllSubjects()
                };
            }

            try
            {
                Entities.Subject subjectDto = new() { Name = request.Name };

                var addedObj = _context.Add(subjectDto);

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    _cacheService.SetData($"subjectId{subjectDto.Id}", addedObj.Entity, DateTimeOffset.Now.AddMonths(1));
                    _cacheService.RemoveData("subjects");
                    _cacheService.RemoveData("availableSubjects");

                    return new CreateSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Subject created successfully."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
                else
                {
                    return new CreateSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
            }
            catch (Exception ex)
            {
                return new CreateSubjectResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSubjectsResponse = await GetAllSubjects()
                };
            }
        }

        public async Task<DeleteSubjectResponse> DeleteSubject(IdRequest request)
        {
            try
            {
                var subjectDto = await _context.Subject.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (subjectDto is null)
                {
                    return new DeleteSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"Subject with ID {request.Id} not found."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }

                _context.Remove(subjectDto);

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    _cacheService.RemoveData($"subjectId{subjectDto.Id}");
                    _cacheService.RemoveData("subjects");
                    _cacheService.RemoveData("availableSubjects");

                    return new DeleteSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Subject deleted successfully."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
                else
                {
                    return new DeleteSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteSubjectResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSubjectsResponse = await GetAllSubjects()
                };
            }
        }

        public async Task<GetAllSubjectsResponse> GetAllSubjects()
        {
            var cacheData = _cacheService.GetData<GetAllSubjectsResponse>("subjects");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.Subject
                .AsNoTracking()
                .Select(x => new SubjectModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Students = x.StudentSubject.Where(y => y.SubjectId == x.Id).Select(y => new StudentDto
                    {
                        Id = y.StudentId,
                        Name = y.Student.Name
                    }).ToList()
                }).ToListAsync();

            GetAllSubjectsResponse response = new() { List = list };

            _cacheService.SetData("subjects", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<SubjectModel?> GetSubjectById(IdRequest request)
        {
            var cacheData = _cacheService.GetData<SubjectModel>($"subjectId{request.Id}");

            if (cacheData is not null)
                return cacheData;

            var response = await _context.Subject
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SubjectModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Students = x.StudentSubject.Where(y => y.SubjectId == x.Id).Select(y => new StudentDto
                    {
                        Id = y.StudentId,
                        Name = y.Student.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            _cacheService.SetData($"subjectId{request.Id}", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<UpdateSubjectResponse> UpdateSubject(UpdateSubjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new UpdateSubjectResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = "Invalid name."
                    },
                    GetAllSubjectsResponse = await GetAllSubjects()
                };
            }

            try
            {
                var subjectDto = await _context.Subject.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (subjectDto is null)
                {
                    return new UpdateSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"Subject with ID {request.Id} not found."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }

                subjectDto.Name = request.Name;

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    _cacheService.SetData($"subjectId{request.Id}", subjectDto, DateTimeOffset.Now.AddMonths(1));
                    _cacheService.RemoveData("subjects");
                    _cacheService.RemoveData("students");
                    _cacheService.RemoveData("availableSubjects");

                    return new UpdateSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Subject updated successfully."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
                else
                {
                    return new UpdateSubjectResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllSubjectsResponse = await GetAllSubjects()
                    };
                }
            }
            catch (Exception ex)
            {
                return new UpdateSubjectResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllSubjectsResponse = await GetAllSubjects()
                };
            }
        }
    }
}
