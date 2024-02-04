using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Responses
{
    public class GetAllStudentsResponse
    {
        public List<StudentModel> List { get; set; } = new();
    }
}
