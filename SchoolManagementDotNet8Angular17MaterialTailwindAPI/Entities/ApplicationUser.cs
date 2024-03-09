using Microsoft.AspNetCore.Identity;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
