using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Core.Interfaces.IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(Core.Interfaces.IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.User> users = _userService.Get();
            var userDto = _mapper.Map<List<Core.Model.User>, List<DTO.User>>(users);
            return Ok(userDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.User user = _userService.Get(id);
            var userDto = _mapper.Map<Core.Model.User, DTO.User>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public IActionResult Post(DTO.User user)
        {
            var userModel = _mapper.Map<DTO.User, Core.Model.User>(user);
            var id = _userService.Insert(userModel, user.Password);
            return Ok(id);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DTO.User user)
        {
            var updatedUser = _mapper.Map<DTO.User, Core.Model.User>(user);
            return Ok(_userService.Update(id, updatedUser));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _userService.Delete(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("activate")]
        public IActionResult Activate(string userName)
        {
            bool result = _userService.ActivateDeactivateUser(new Core.Model.ActivateDeactivateUser{ UserName = userName, IsActive = true});
            return Ok(result);
        }

        [HttpPost]
        [Route("deactivate")]
        public IActionResult Deactivate(string userName)
        {
            bool result = _userService.ActivateDeactivateUser(new Core.Model.ActivateDeactivateUser { UserName = userName, IsActive = false });
            return Ok(result);
        }

    }
}
