using Microsoft.AspNetCore.Mvc;
using Netrika.Web.Models;
using NetrikaTest.Services.MedicalOrganizations;

namespace Netrika.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicalOrganizationsController : ControllerBase
    {
        private readonly IMedicalOrganizationsService _medicalOrganizations;
        
        public MedicalOrganizationsController(IMedicalOrganizationsService medicalOrganizations)
        {
            _medicalOrganizations = medicalOrganizations;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<MedicalOrganizationDto>> List()
        {
            var result = await _medicalOrganizations.ListByName("");
            return result.Select(x => new MedicalOrganizationDto(x)).ToList();
        }
    }
}