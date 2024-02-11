using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses
{
    public class InitUpdateProfessorResponse
    {
        public ProfessorDto Professor { get; set; } = new();
        public List<DropDownItem> SchoolOptions { get; set; } = new();
    }
}
