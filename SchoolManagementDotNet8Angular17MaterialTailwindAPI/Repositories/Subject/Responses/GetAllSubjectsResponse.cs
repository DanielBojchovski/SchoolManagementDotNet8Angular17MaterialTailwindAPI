using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Responses
{
    public class GetAllSubjectsResponse
    {
        public List<SubjectModel> List { get; set; } = new();
    }
}
