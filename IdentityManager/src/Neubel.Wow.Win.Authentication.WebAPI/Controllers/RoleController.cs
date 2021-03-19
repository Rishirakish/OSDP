using System.Collections.Generic;
using AutoMapper;
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

        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.Role> roles = _roleService.Get();
            var roleDto = _mapper.Map<List<Core.Model.Role>, List<DTO.Role>>(roles);
            return Ok(roleDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.Role role = _roleService.Get(id);
            var roleDto = _mapper.Map<Core.Model.Role, DTO.Role>(role);
            return Ok(roleDto);
        }

        [HttpPost]
        public IActionResult Post(DTO.Role role)
        {
            var roleModel = _mapper.Map<DTO.Role, Core.Model.Role>(role);
            int id = _roleService.Insert(roleModel);
            return Ok(id);
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DTO.Role role)
        {
            var updatedRole = _mapper.Map<DTO.Role, Core.Model.Role>(role);
            int updatedId = _roleService.Update(id, updatedRole);
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _roleService.Delete(id);
            return Ok(result);
        }
    }
}
