using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neubel.Wow.Win.Authentication.Core.Model;
using Neubel.Wow.Win.Authentication.WebAPI.DTO;

namespace Neubel.Wow.Win.Authentication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly Core.Interfaces.IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public SecurityController(Core.Interfaces.IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the access and refresh token after authentication.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [Route("token")]
        [HttpPost]
        public IActionResult Token(DTO.LoginRequest loginRequest)
        {
            var loginReq = _mapper.Map<DTO.LoginRequest, Core.Model.LoginRequest>(loginRequest);
            LoginToken loginToken = _authenticationService.Login(loginReq);
            if (loginToken == null)
                return Unauthorized();

            var userToken = _mapper.Map<LoginToken, UserToken>(loginToken);

            return Ok(userToken);
        }

        [Route("refreshToken")]
        [Authorize]
        [HttpPut]
        public IActionResult RefreshToken()
        {
            string authorization = HttpContext.Request.Headers["Authorization"].SingleOrDefault();

            LoginToken loginToken = _authenticationService.RefreshToken(authorization);
            if (loginToken == null)
                return Unauthorized();

            var userToken = _mapper.Map<LoginToken, UserToken>(loginToken);

            return Ok(userToken);
        }

        [Route("forgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(string userName)
        {
           bool isSuccess = _authenticationService.ForgotPassword(userName);
           return Ok(isSuccess);
        }

        [Route("changePassword")]
        [HttpPost]
        public IActionResult ChangePassword(DTO.ChangedPassword changedPassword)
        {
            var newPassword = _mapper.Map<DTO.ChangedPassword, Core.Model.ChangedPassword>(changedPassword);
            bool result = _authenticationService.ChangePassword(newPassword);
            return Ok(result);
        }

        [Route("sendOtp")]
        [HttpPost]
        public IActionResult SendOTP(string userName)
        {
            bool result = _authenticationService.SendOtp(userName);
            return Ok(result);
        }

        [Route("validateOtp")]
        [HttpPost]
        public IActionResult ValidateOtp(string userName, string otp)
        {
            bool result = _authenticationService.validateOtp(userName, otp);
            return Ok(result);
        }

        [Route("confirmMobile")]
        [HttpPost]
        public IActionResult ConfirmMobile(string userName, string otp)
        {
            bool result = _authenticationService.validateOtp(userName, otp);
            return Ok(result);
        }

        [Route("confirmEmail")]
        [HttpPost]
        public IActionResult ConfirmEmail(string userName, string otp)
        {
            bool result = _authenticationService.validateOtp(userName, otp);
            return Ok(result);
        }

        [Route("loginHistory")]
        [HttpGet]
        public IActionResult LoginHistory(int userId)
        {
            return Ok(_authenticationService.GetLoginHistory(userId));
        }
        
        [Route("lock")]
        [HttpPost]
        public IActionResult Lock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(new LockUnlockUser { UserName = userName, Locked = true });
            return Ok(result);
        }

        [Route("unLock")]
        [HttpPost]
        public IActionResult UnLock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(new LockUnlockUser { UserName = userName, Locked = false});
            return Ok(result);
        }
    }
}