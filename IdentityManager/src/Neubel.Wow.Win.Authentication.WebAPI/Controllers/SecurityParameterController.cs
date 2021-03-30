using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Interfaces;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityParameterController : ControllerBase
    {
        private readonly ISecurityParameterService _securityParameterService;
        private readonly IMapper _mapper;

        public SecurityParameterController(ISecurityParameterService securityParameterService, IMapper mapper)
        {
            _securityParameterService = securityParameterService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all security parameters/password policies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Get()
        {
            List<Core.Model.SecurityParameter> users = _securityParameterService.Get();
            var securityParameterDto = _mapper.Map<List<Core.Model.SecurityParameter>, List<DTO.SecurityParameter>>(users);
            return Ok(securityParameterDto);
        }
        /// <summary>
        /// Get a security parameters/password policy by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Get(int id)
        {
            Core.Model.SecurityParameter user = _securityParameterService.Get(id);
            var securityParameterDto = _mapper.Map<Core.Model.SecurityParameter, DTO.SecurityParameter>(user);
            return Ok(securityParameterDto);
        }

        /// <summary>
        /// Add a security parameters/password policy.
        /// </summary>
        /// <param name="securityParameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Post([FromBody] DTO.SecurityParameter securityParameter)
        {
            var securityParameterModel = _mapper.Map<DTO.SecurityParameter, Core.Model.SecurityParameter>(securityParameter);
            var id = _securityParameterService.Insert(securityParameterModel);
            return Ok(id);
        }
        /// <summary>
        /// Update security parameters/password policy.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="securityParameter"></param>
        /// <returns></returns>
        // PUT api/<SecurityParameterController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Put(int id, [FromBody] DTO.SecurityParameter securityParameter)
        {
            var updatedSecurityParameter = _mapper.Map<DTO.SecurityParameter, Core.Model.SecurityParameter>(securityParameter);
            _securityParameterService.Update(id, updatedSecurityParameter);
            return Ok(id);
        }
        /// <summary>
        /// Delete a security parameters/password policy (soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _securityParameterService.Delete(id);
            return Ok(result);
        }
    }
}
