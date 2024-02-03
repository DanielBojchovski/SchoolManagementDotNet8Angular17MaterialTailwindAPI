using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Interfaces
{
    public interface ISchoolService
    {
        public Task<GetAllSchoolsResponse> GetAllSchools();
        public Task<SchoolModel?> GetSchoolById(IdRequest request);
        public Task<SchoolModel?> GetSchoolByPrincipalId(IdRequest request);
        public Task<SchoolModel?> GetSchoolByProfessorId(IdRequest request);
        public Task<CreateSchoolResponse> CreateSchool(CreateSchoolRequest request);
        public Task<UpdateSchoolResponse> UpdateSchool(UpdateSchoolRequest request);
        public Task<DeleteSchoolResponse> DeleteSchool(IdRequest request);
    }
}
