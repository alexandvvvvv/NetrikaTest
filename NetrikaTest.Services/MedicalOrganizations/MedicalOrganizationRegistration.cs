using Microsoft.Extensions.DependencyInjection;
using Netrika.Services.MedicalOrganizations;

namespace NetrikaTest.Services.MedicalOrganizations
{
    public static class MedicalOrganizationRegistration
    {
        public static void RegisterMedicalOrganizationsServices(this IServiceCollection services)
        {
            services.AddScoped<IMedicalOrganizationsClient, MedicalOrganizationsClient>();
            services.AddScoped<IMedicalOrganizationsService, MedicalOrganizationsService>();
            services.AddScoped<IMedicalOrganizationsValidator, MedicalOrganizationsValidator>();
            services.AddSingleton<IMedicalOrganizationsCache, MedicalOrganizationsCache>();
        }
    }
}
