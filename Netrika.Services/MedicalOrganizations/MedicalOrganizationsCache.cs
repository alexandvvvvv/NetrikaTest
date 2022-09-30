using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Netrika.Services.Utils;

namespace Netrika.Services.MedicalOrganizations
{
    public interface IMedicalOrganizationsCache
    {
        Task<IReadOnlyCollection<MedicalOrganization>> List();
    }

    internal class MedicalOrganizationsCache : IMedicalOrganizationsCache
    {
        private readonly Cached<IReadOnlyCollection<MedicalOrganization>> _cached;
        private readonly IServiceProvider _serviceProvider;

        public MedicalOrganizationsCache(IOptions<MedicalOrganizationsParams> cacheParams, IServiceProvider serviceProvider)
        {
            _cached = new(cacheParams.Value.CacheExpirationInSeconds);
            _serviceProvider = serviceProvider;
        }

        public Task<IReadOnlyCollection<MedicalOrganization>> List()
        {
            return _cached.GetOrAdd(async () =>
            {
                using var scope = _serviceProvider.CreateScope(); //todo refactor. scope is created because the class is registered as singleton

                var medicalOrganizations = scope.ServiceProvider.GetRequiredService<IMedicalOrganizationsClient>();
                var result = await medicalOrganizations.List();

                return result.Select(x => new MedicalOrganization
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            });
        }
    }
}
