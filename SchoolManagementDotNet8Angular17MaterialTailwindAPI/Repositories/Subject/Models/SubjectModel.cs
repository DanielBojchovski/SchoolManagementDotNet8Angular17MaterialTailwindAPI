using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StudentDto> Students { get; set; } = new();
    }
}
