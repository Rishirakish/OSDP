using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly Core.Interfaces.IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public UserRoleController(Core.Interfaces.IUserRoleService userRoleService, IMapper mapper)
        {
            _userRoleService = userRoleService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all user roles.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.ApplicationAdmin + "," + UserRoles.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.UserRole> userRoles = _userRoleService.Get();
            var userRolesDto = _mapper.Map<List<Core.Model.UserRole>, List<DTO.UserRole>>(userRoles);
            return Ok(userRolesDto);
        }
        /// <summary>
        /// Get user roles by user Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.ApplicationAdmin + "," + UserRoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            List<Core.Model.UserRole> userRoles = _userRoleService.Get(id);
            var userRolesDto = _mapper.Map< List<Core.Model.UserRole>, List<DTO.UserRole>>(userRoles);
            return Ok(userRolesDto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public IActionResult Post(DTO.UserRole userRole)
        {
            var userRoleModel = _mapper.Map<DTO.UserRole, Core.Model.UserRole>(userRole);
            int id = _userRoleService.Add(userRoleModel);
            return Ok(id);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _userRoleService.Delete(id);
            return Ok(result);
        }
    }
}
