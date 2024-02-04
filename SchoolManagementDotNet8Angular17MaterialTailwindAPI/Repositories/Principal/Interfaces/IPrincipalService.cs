using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces
{
    public interface IPrincipalService
    {
        public Task<GetAllPrincipalsResponse> GetAllPrincipals();
        public Task<PrincipalModel?> GetPrincipalById(IdRequest request);
        public Task<PrincipalModel?> GetPrincipalBySchoolId(IdRequest request);
        public Task<OperationStatusResponse> CreatePrincipal(CreatePrincipalRequest request);
        public Task<OperationStatusResponse> UpdatePrincipal(UpdatePrincipalRequest request);
        public Task<OperationStatusResponse> DeletePrincipal(IdRequest request);
    }
}
