using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
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

        public SubjectService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreateSubject(CreateSubjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                Entities.Subject subjectDto = new() { Name = request.Name };

                _context.Add(subjectDto);
                await _context.SaveChangesAsync();

                return new OperationStatusResponse { IsSuccessful = true, Message = "Success. Subject created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<OperationStatusResponse> DeleteSubject(IdRequest request)
        {
            try
            {
                var subjectDto = await _context.Subject.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (subjectDto is null)
                    return new OperationStatusResponse { IsSuccessful = false, Message = $"Subject with ID {request.Id} not found." };

                _context.Remove(subjectDto);
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Subject with ID {subjectDto.Id} deleted successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<GetAllSubjectsResponse> GetAllSubjects()
        {
            var response = await _context.Subject
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

            return new GetAllSubjectsResponse { List = response };
        }

        public async Task<SubjectModel?> GetSubjectById(IdRequest request)
        {
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

            return response;
        }

        public async Task<OperationStatusResponse> UpdateSubject(UpdateSubjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                var subjectDto = await _context.Subject.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (subjectDto is null)
                    return new OperationStatusResponse { IsSuccessful = false, Message = $"Subject with ID {request.Id} not found." };

                subjectDto.Name = request.Name;

                await _context.SaveChangesAsync();
                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Subject with ID {subjectDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
