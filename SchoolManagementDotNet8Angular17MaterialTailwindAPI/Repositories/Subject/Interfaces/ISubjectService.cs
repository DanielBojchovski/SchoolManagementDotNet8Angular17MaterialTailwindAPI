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
        public Task<CreateSubjectResponse> CreateSubject(CreateSubjectRequest request);
        public Task<UpdateSubjectResponse> UpdateSubject(UpdateSubjectRequest request);
        public Task<DeleteSubjectResponse> DeleteSubject(IdRequest request);
    }
}
