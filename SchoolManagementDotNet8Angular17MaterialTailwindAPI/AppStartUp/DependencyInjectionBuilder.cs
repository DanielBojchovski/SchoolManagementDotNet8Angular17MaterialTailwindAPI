using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Services;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.AppStartUp
{
    public static class DependencyInjectionBuilder
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ISchoolService, SchoolService>();

            services.AddScoped<IPrincipalService, PrincipalService>();

            //services.AddScoped<IProfessorService, ProfessorService>();

            //services.AddScoped<ISubjectService, SubjectService>();

            //services.AddScoped<IStudentService, StudentService>();

            return services;
        }
    }
}
