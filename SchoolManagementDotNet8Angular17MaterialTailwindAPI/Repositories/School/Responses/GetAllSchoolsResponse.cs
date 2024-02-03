using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Responses
{
    public class GetAllSchoolsResponse
    {
        public List<SchoolModel> List { get; set; } = new();
    }
}
