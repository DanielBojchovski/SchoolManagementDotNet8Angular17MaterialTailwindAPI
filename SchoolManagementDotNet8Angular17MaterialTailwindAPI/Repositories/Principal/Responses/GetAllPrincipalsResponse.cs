using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses
{
    public class GetAllPrincipalsResponse
    {
        public List<PrincipalModel> List { get; set; } = new();
    }
}
