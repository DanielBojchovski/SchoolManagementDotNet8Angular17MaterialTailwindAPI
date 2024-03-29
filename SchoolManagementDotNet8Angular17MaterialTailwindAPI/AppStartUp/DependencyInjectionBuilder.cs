﻿using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Services;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Services;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.AppStartUp
{
    public static class DependencyInjectionBuilder
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ISchoolService, SchoolService>();

            services.AddScoped<IPrincipalService, PrincipalService>();

            services.AddScoped<IProfessorService, ProfessorService>();

            services.AddScoped<ISubjectService, SubjectService>();

            services.AddScoped<IStudentService, StudentService>();

            services.AddSingleton<ICacheService, CacheService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
