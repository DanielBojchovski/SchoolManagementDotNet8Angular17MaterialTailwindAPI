using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses
{
    public class InitUpdatePrincipalResponse
    {
        public PrincipalDto Principal { get; set; } = new();
        public List<DropDownItem> SchoolOptions { get; set; } = new();
    }
}
