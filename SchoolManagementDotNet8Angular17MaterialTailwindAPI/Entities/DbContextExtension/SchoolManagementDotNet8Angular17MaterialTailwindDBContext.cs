using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities
{
    public partial class SchoolManagementDotNet8Angular17MaterialTailwindDBContext : IdentityDbContext<ApplicationUser>
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
