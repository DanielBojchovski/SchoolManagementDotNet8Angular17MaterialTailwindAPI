using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Responses
{
    public class CreateSchoolResponse
    {
        public GetAllSchoolsResponse GetAllSchoolsResponse { get; set; } = new();
        public OperationStatusResponse OperationStatusResponse { get; set; } = new();
    }
}
