using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Responses
{
    public class GetAvailableSubjectsResponse
    {
        public List<SubjectDto> List { get; set; } = new();
    }
}
