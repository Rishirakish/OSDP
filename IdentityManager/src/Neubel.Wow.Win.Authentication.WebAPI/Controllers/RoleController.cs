using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly Core.Interfaces.IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(Core.Interfaces.IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.Role> roles = _roleService.Get();
            var roleDto = _mapper.Map<List<Core.Model.Role>, List<DTO.Role>>(roles);
            return Ok(roleDto);
        }
        /// <summary>
        /// Get role by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.Role role = _roleService.Get(id);
            var roleDto = _mapper.Map<Core.Model.Role, DTO.Role>(role);
            return Ok(roleDto);
        }
        /// <summary>
        /// Add a new role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        [HttpPost]
        public IActionResult Post(DTO.Role role)
        {
            var roleModel = _mapper.Map<DTO.Role, Core.Model.Role>(role);
            int id = _roleService.Insert(roleModel);
            return Ok(id);
        }
        /// <summary>
        /// update and existing role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Put(int id, [FromBody] DTO.Role role)
        {
            var updatedRole = _mapper.Map<DTO.Role, Core.Model.Role>(role);
            int updatedId = _roleService.Update(id, updatedRole);
            return Ok(updatedId);
        }
        /// <summary>
        /// Delete a role (Soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Core.Model.UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _roleService.Delete(id);
            return Ok(result);
        }
    }
}
