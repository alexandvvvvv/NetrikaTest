using Microsoft.Extensions.DependencyInjection;

namespace Netrika.Services.Utils
{
    public static class UtilsRegistration
    {
        public static void RegisterUtilityValidators(this IServiceCollection services)
        {
            services.AddScoped<IPaginatorValidator, PaginatorValidator>();
        }
    }
}
