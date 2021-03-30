using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Get all organization.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.Organization> organizations = _organizationService.Get();
            var organizationsDto = _mapper.Map<List<Core.Model.Organization>, List<DTO.Organization>>(organizations);
            return Ok(organizationsDto);
        }
        /// <summary>
        /// Get organization by organization Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.Organization organization = _organizationService.Get(id);
            var organizationDto = _mapper.Map<Core.Model.Organization, DTO.Organization>(organization);
            return Ok(organizationDto);
        }
        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpPost]
        public IActionResult Post(DTO.Organization organization)
        {
            var organizationModel = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            var id = _organizationService.Insert(organizationModel);
            return Ok(id);
        }
        /// <summary>
        /// Update existing organization by organization id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Put(int id, [FromBody] DTO.Organization organization)
        {
            var updatedOrganization = _mapper.Map<DTO.Organization, Core.Model.Organization>(organization);
            return Ok(_organizationService.Update(id, updatedOrganization));
        }
        /// <summary>
        /// Delete an organization (soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _organizationService.Delete(id);
            return Ok(result);
        }
    }
}
