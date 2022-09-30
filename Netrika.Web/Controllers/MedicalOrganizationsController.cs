using Microsoft.AspNetCore.Mvc;
using Netrika.Web.Models;
using Netrika.Services.MedicalOrganizations;

namespace Netrika.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalOrganizationsController : ControllerBase
    {
        private readonly IMedicalOrganizationsService _medicalOrganizations;
        
        public MedicalOrganizationsController(IMedicalOrganizationsService medicalOrganizations)
        {
            _medicalOrganizations = medicalOrganizations;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<MedicalOrganizationDto>> List([FromQuery] string? query,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string? orderBy
            )
        {
            var result = await _medicalOrganizations.List(query, skip, take, orderBy);
            return result.Select(x => new MedicalOrganizationDto(x)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<MedicalOrganizationDto> List([FromRoute] Guid id)
        {
            var result = await _medicalOrganizations.Get(id);
            return new MedicalOrganizationDto(result);
        }
    }
}