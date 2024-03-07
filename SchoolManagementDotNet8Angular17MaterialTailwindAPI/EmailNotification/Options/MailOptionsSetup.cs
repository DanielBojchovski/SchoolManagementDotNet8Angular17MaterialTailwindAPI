using Microsoft.Extensions.Options;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Options
{
    public class MailOptionsSetup : IConfigureOptions<MailOptions>
    {
        private const string SectionName = "MailOptions";
        private readonly IConfiguration _configuration;

        public MailOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MailOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
