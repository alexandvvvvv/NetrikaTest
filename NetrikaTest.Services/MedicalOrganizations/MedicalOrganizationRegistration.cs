using Microsoft.Extensions.DependencyInjection;

namespace NetrikaTest.Services.MedicalOrganizations
{
    public static class MedicalOrganizationRegistration
    {
        public static void RegisterMedicalOrganizationsServices(this IServiceCollection services)
        {
            services.AddScoped<IMedicalOrganizationsClient, MedicalOrganizationsClient>();
            services.AddScoped<IMedicalOrganizationsService, MedicalOrganizationsService>();
            services.AddSingleton<IMedicalOrganizationsCache, MedicalOrganizationsCache>();
        }
    }
}
