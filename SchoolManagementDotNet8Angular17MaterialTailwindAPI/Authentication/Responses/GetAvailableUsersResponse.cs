using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses
{
    public class GetAvailableUsersResponse
    {
        public List<UserDropDownModel> List { get; set; } = new();  
    }
}
