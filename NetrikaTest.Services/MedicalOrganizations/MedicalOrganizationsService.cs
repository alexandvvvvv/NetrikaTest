using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetrikaTest.Services.MedicalOrganizations
{
    public interface IMedicalOrganizationsService
    {
        Task<MedicalOrganization> Get(Guid id);
        Task<IReadOnlyCollection<MedicalOrganization>> ListByName(string name);
    }

    public class MedicalOrganizationsService : IMedicalOrganizationsService
    {
        private readonly IMedicalOrganizationsCache _cache;

        public MedicalOrganizationsService(IMedicalOrganizationsCache cache)
        {
            _cache = cache;
        }

        public Task<MedicalOrganization> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<MedicalOrganization>> ListByName(string name)
        {
            return _cache.List();
        }
    }
}
