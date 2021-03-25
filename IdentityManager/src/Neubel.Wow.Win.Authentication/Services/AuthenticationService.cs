using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Core.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IConfiguration configuration, IRoleService roleService, IAuthenticationRepository authenticationRepository, IUserRepository userRepository)
        {
            _configuration = configuration;
            _roleService = roleService;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
        }


        #region Public Methods

        public LoginToken Login(LoginRequest loginRequest)
        {
            LoginToken token = new LoginToken();
            var passwordLogin = _authenticationRepository.GetLoginPassword(loginRequest.UserName);
            string valueHash = string.Empty;
            if (passwordLogin != null && Hasher.ValidateHash(loginRequest.Password, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out valueHash))
            {
                loginRequest.Id = passwordLogin.UserId;
                token = GenerateTokens(loginRequest.UserName);
            }
            
            //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
            loginRequest.LoginDate = DateTime.Now;
            loginRequest.PasswordHash = valueHash;
            _authenticationRepository.LoginLog(loginRequest);

            return token;
        }

        public bool ChangePassword(ChangedPassword newPassword)
        {
            PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(newPassword.UserName);
            if (Hasher.ValidateHash(newPassword.CurrentPassword, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out _))
            {
                var newPasswordLogin = Hasher.HashPassword(newPassword.NewPassword);
                newPasswordLogin.UserId = passwordLogin.UserId;
                _authenticationRepository.UpdatePasswordLogin(newPasswordLogin);
            }
            //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
            _authenticationRepository.PasswordChangeLog(passwordLogin);
            return false;
        }

        public bool LockUnlockUser(LockUnlockUser lockUnlockUser)
        {
            var isSuccess = _authenticationRepository.LockUnlockUser(lockUnlockUser);
            _authenticationRepository.LockedUserLog(lockUnlockUser);
            return isSuccess;
        }

        public List<LoginHistory> GetLoginHistory(int userId)
        {
            return _authenticationRepository.GetLoginHistory(userId);
        }
        #endregion

        #region Private Methods
        private LoginToken GenerateTokens(string userName)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            DateTime now = DateTime.Now;
            var claims = GetTokenClaims(userName, now);

            var accessJwt = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessJwt);

            var refreshJwt = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddDays(30),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            var encodedRefreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshJwt);
            
            var loginToken = new LoginToken
            {
                UserName = userName,
                AccessToken = encodedAccessJwt,
                AccessTokenExpiry = DateTime.Now.AddDays(1),
                RefreshToken = encodedRefreshJwt,
                RefreshTokenExpiry = DateTime.Now.AddDays(30),
            };

            _authenticationRepository.SaveLoginToken(loginToken);

            //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
            _authenticationRepository.LoginTokenLog(loginToken);

            return loginToken;
        }
        private List<Claim> GetTokenClaims(string sub, DateTime dateTime)
        {
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Helpers.ToUnixEpochDate(dateTime).ToString(), ClaimValueTypes.Integer64)
            };

            var roles = _roleService.Get(sub);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public LoginToken RefreshToken(string authorization)
        {
            var token = authorization.Substring(authorization.IndexOf(' ') + 1);

            if (new JwtSecurityTokenHandler().ReadToken(token) is JwtSecurityToken jwt) 
                return GenerateTokens(jwt.Subject);

            return null;
        }

        public bool SendOtp(string userName)
        {
            PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
            var user = _userRepository.Get(passwordLogin.UserId);
            string otp = GenericUtil.GenerateOTP().ToString();
            new KromeEmail().SendEmail(user.Email, otp, GenericUtil.OTPTransType.PasswordReset);
            new KromeSMS().SendSMS(user.Mobile, otp, GenericUtil.OTPTransType.PasswordReset);

            UserValidationOtp userValidationOtp = new UserValidationOtp
            {
                UserId = passwordLogin.UserId,
                OrgId = user.OrgId,
                otp = otp,
                OtpAuthenticatedTime = DateTime.Now,
                OtpGeneratedTime = DateTime.Now,
                Status = (int)GenericUtil.OTPTransType.PasswordReset,
                Type = GenericUtil.OTPTransType.PasswordReset.ToString()
            };

            _authenticationRepository.SaveOtp(userValidationOtp);

            return true;
        }

        public bool ForgotPassword(string userName)
        {
            PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
            var user = _userRepository.Get(passwordLogin.UserId);
            string otp = GenericUtil.GenerateOTP().ToString();
            new KromeEmail().SendEmail(user.Email, otp, GenericUtil.OTPTransType.PasswordReset);
            new KromeSMS().SendSMS(user.Mobile, otp, GenericUtil.OTPTransType.PasswordReset);

            UserValidationOtp userValidationOtp = new UserValidationOtp
            {
                UserId = passwordLogin.UserId,
                OrgId = user.OrgId,
                otp = otp,
                OtpAuthenticatedTime = DateTime.Now,
                OtpGeneratedTime = DateTime.Now,
                Status = (int) GenericUtil.OTPTransType.PasswordReset,
                Type = GenericUtil.OTPTransType.PasswordReset.ToString()
            };

            _authenticationRepository.SaveOtp(userValidationOtp);

            return true;
        }

        public bool validateOtp(string userName, string otp)
        {
            PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
            var otpDetails = _authenticationRepository.GetOtp(passwordLogin.UserId);
            if (otpDetails.otp == otp)
                return true;
            return false;
        }

        #endregion
    }
}
