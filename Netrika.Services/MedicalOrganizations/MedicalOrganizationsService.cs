using Microsoft.Extensions.Options;
using Netrika.Services.MedicalOrganizations;
using Netrika.Services.Utils;

namespace NetrikaTest.Services.MedicalOrganizations
{
    public interface IMedicalOrganizationsService
    {
        Task<MedicalOrganization> Get(Guid id);
        Task<IReadOnlyCollection<MedicalOrganization>> List(string? filter, int? skip = null, int? take = null, string? orderBy = null);
    }

    public class MedicalOrganizationsService : IMedicalOrganizationsService
    {
        private readonly IMedicalOrganizationsCache _cache;
        private readonly IMedicalOrganizationsValidator _medicalOrganizationsValidator;
        private readonly IPaginatorValidator _paginatorValidator;
        private readonly int _throttle;

        public MedicalOrganizationsService(IMedicalOrganizationsCache cache, 
            IOptions<MedicalOrganizationsParams> options, 
            IMedicalOrganizationsValidator medicalOrganizationsValidator, 
            IPaginatorValidator paginatorValidator)
        {
            _cache = cache;
            _throttle = options.Value.Throttling;
            _medicalOrganizationsValidator = medicalOrganizationsValidator;
            _paginatorValidator = paginatorValidator;
        }

        public async Task<MedicalOrganization> Get(Guid id)
        {
            var organizations = await _cache.List();
            var result = organizations.FirstOrDefault(x => x.Id == id);
            if (result is null)
            {
                throw new Exception($"Organization with id {id} not found");
            }
            return result;
        }

        public async Task<IReadOnlyCollection<MedicalOrganization>> List(string? filter, int? skip, int? take, string? orderBy)
        {
            _medicalOrganizationsValidator.Validate(filter);
            _paginatorValidator.Validate(skip, take);

            var result = await _cache.List();

            var searchChunks = (filter ?? "").Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var query = result.AsEnumerable();
            
            if (searchChunks.Any())
            {
                query = query
                    .AsParallel()
                    .Where(x =>
                    {
                        return searchChunks.All(y => x.Name.Contains(y, StringComparison.OrdinalIgnoreCase));
                    });
            }

            query = orderBy?.ToLower() switch
            {
                "name" => query.OrderBy(x => x.Name),
                "id" => query.OrderBy(x => x.Id),
                _ => query
            };

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            query = query.Take(take ?? _throttle);

            return query.ToList();
        }
    }
}
