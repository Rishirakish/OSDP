using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly Core.Interfaces.IOrganizationService _organizationService;
        private readonly IMapper _mapper;

        public OrganizationController(Core.Interfaces.IOrganizationService organizationService, IMapper mapper)
        {
            _organizationService = organizationService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.Organization> organizations = _organizationService.Get();
            var organizationsDto = _mapper.Map<List<Core.Model.Organization>, List<DTO.Organization>>(organizations);
            return Ok(organizationsDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.Organization organization = _organizationService.Get(id);
            var organizationDto = _mapper.Map<Core.Model.Organization, DTO.Organization>(organization);
            return Ok(organizationDto);
        }

        [HttpPost]
        public IActionResult Post(DTO.Organization organization)
        {
            var organizationModel = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            var id = _organizationService.Insert(organizationModel);
            return Ok(id);
        }

        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DTO.Organization organization)
        {
            var updatedOrganization = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            return Ok(_organizationService.Update(id, updatedOrganization));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _organizationService.Delete(id);
            return Ok(result);
        }
    }
}
