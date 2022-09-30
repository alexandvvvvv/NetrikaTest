using NetrikaTest.Services.MedicalOrganizations;

namespace Netrika.Web.Models
{
    public class MedicalOrganizationDto
    {
        public MedicalOrganizationDto(MedicalOrganization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
        }

        public Guid Id { get; }
        public string Name { get; } = default!;
    }
}