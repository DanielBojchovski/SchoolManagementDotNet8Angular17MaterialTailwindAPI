using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Interfaces
{
    public interface IProfessorService
    {
        public Task<GetAllProfessorsResponse> GetAllProfessors();
        public Task<ProfessorModel?> GetProfessorById(IdRequest request);
        public Task<GetAllProfessorsResponse> GetProfessorsBySchoolId(IdRequest request);
        public Task<OperationStatusResponse> CreateProfessor(CreateProfessorRequest request);
        public Task<OperationStatusResponse> UpdateProfessor(UpdateProfessorRequest request);
        public Task<DeleteProfessorResponse> DeleteProfessor(IdRequest request);
        public Task<DropDownResponse> GetSchoolsDropDown();
    }
}
