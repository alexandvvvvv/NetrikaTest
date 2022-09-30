namespace Netrika.Services.MedicalOrganizations
{
    public class MedicalOrganizationsParams
    {
        public int CacheExpirationInSeconds { get; set; } = 120;
        public int Throttling { get; set; } = 100;
    }
}
