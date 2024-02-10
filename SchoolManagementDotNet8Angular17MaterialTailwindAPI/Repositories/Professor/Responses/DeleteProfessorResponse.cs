using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses
{
    public class DeleteProfessorResponse
    {
        public GetAllProfessorsResponse GetAllProfessorsResponse { get; set; } = new();
        public OperationStatusResponse OperationStatusResponse { get; set; } = new();
    }
}
