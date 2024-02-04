using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses
{
    public class GetAllProfessorsResponse
    {
        public List<ProfessorModel> List { get; set; } = new();
    }
}
