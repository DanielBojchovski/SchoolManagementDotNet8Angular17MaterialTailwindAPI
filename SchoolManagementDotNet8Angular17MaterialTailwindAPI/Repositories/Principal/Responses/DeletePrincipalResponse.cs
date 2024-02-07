using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses
{
    public class DeletePrincipalResponse
    {
        public GetAllPrincipalsResponse GetAllPrincipalsResponse { get; set; } = new();
        public OperationStatusResponse OperationStatusResponse { get; set; } = new();
    }
}
