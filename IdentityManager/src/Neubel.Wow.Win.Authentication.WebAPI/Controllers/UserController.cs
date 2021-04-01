using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Model.Roles;

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
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.ApplicationAdmin + "," + UserRoles.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            List<Core.Model.User> users = _userService.Get();
            var userDto = _mapper.Map<List<Core.Model.User>, List<DTO.User>>(users);
            return Ok(userDto);
        }
        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.ApplicationAdmin + "," + UserRoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Core.Model.User user = _userService.Get(id);
            var userDto = _mapper.Map<Core.Model.User, DTO.User>(user);
            return Ok(userDto);
        }
        /// <summary>
        /// Add a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public IActionResult Post(DTO.User user)
        {
            string authorization = HttpContext.Request.Headers["Authorization"].SingleOrDefault();
            var userModel = _mapper.Map<DTO.User, Core.Model.User>(user);
            var result = _userService.Insert(authorization, userModel, user.Password);
            if (result.IsSuccessful)
                return Ok(result.RequestedObject);
            return Ok(result.ValidationMessages);
        }
        /// <summary>
        /// Update an existing user by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Put(int id, [FromBody] DTO.User user)
        {
            var updatedUser = _mapper.Map<DTO.User, Core.Model.User>(user);
            return Ok(_userService.Update(id, updatedUser));
        }
        /// <summary>
        /// Delete an user(soft delete).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Delete(int id)
        {
            bool result = _userService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// Activate user by username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activate")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Activate(string userName)
        {
            bool result = _userService.ActivateDeactivateUser(new Core.Model.ActivateDeactivateUser{ UserName = userName, IsActive = true});
            return Ok(result);
        }
        /// <summary>
        /// Deactivate user by user name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deactivate")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Deactivate(string userName)
        {
            bool result = _userService.ActivateDeactivateUser(new Core.Model.ActivateDeactivateUser { UserName = userName, IsActive = false });
            return Ok(result);
        }

    }
}
