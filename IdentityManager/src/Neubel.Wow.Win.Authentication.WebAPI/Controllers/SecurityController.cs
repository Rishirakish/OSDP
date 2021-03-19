using AutoMapper;
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
        public ActionResult<UserToken> Token(DTO.LoginRequest loginRequest)
        {
            var loginReq = _mapper.Map<DTO.LoginRequest, Core.Model.LoginRequest>(loginRequest);
            var loginToken = _authenticationService.Login(loginReq);
            if (loginToken == null)
                return Unauthorized();

            var userToken = _mapper.Map<LoginToken, UserToken>(loginToken);

            return Ok(userToken);
        }

        [Route("forgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword()
        {
            return null;
        }

        [Route("changePassword")]
        [HttpPost]
        public ActionResult ChangePassword(DTO.ChangedPassword changedPassword)
        {
            var newPassword = _mapper.Map<DTO.ChangedPassword, Core.Model.ChangedPassword>(changedPassword);
            bool result = _authenticationService.ChangePassword(newPassword);
            return Ok(result);
        }

        [Route("confirmEmail")]
        [HttpPost]
        public ActionResult ConfirmEmail()
        {
            return null;
        }

        [Route("confirmMobile")]
        [HttpPost]
        public ActionResult ConfirmMobile()
        {
            return null;
        }

        [Route("sendOtp")]
        [HttpPost]
        public ActionResult SendOTP()
        {
            return null;
        }

        [Route("loginHistory")]
        [HttpGet]
        public ActionResult LoginHistory()
        {
            return null;
        }
        
        [Route("lock")]
        [HttpPost]
        public ActionResult Lock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(new LockUnlockUser { UserName = userName, Locked = true });
            return Ok(result);
        }

        [Route("unLock")]
        [HttpPost]
        public ActionResult UnLock(string userName)
        {
            bool result = _authenticationService.LockUnlockUser(new LockUnlockUser { UserName = userName, Locked = false});
            return Ok(result);
        }
    }
}