using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Interfaces
{
    public interface ISubjectService
    {
        public Task<GetAllSubjectsResponse> GetAllSubjects();
        public Task<SubjectModel?> GetSubjectById(IdRequest request);
        public Task<OperationStatusResponse> CreateSubject(CreateSubjectRequest request);
        public Task<OperationStatusResponse> UpdateSubject(UpdateSubjectRequest request);
        public Task<OperationStatusResponse> DeleteSubject(IdRequest request);
    }
}
