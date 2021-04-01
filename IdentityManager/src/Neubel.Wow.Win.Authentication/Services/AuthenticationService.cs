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
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ILogger _logger;
        public AuthenticationService(IConfiguration configuration,
            IRoleRepository roleRepository,
            IAuthenticationRepository authenticationRepository,
            IUserRepository userRepository, ILogger logger)
        {
            _configuration = configuration;
            _roleRepository = roleRepository;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _logger = logger;
        }


        #region Public Methods

        public LoginToken Login(LoginRequest loginRequest)
        {
            try
            {
                LoginToken token = new LoginToken();
                var passwordLogin = _authenticationRepository.GetLoginPassword(loginRequest.UserName);
                string valueHash = string.Empty;
                if (passwordLogin != null && Hasher.ValidateHash(loginRequest.Password, passwordLogin.PasswordSalt,
                    passwordLogin.PasswordHash, out valueHash))
                {
                    loginRequest.Id = passwordLogin.UserId;
                    token = GenerateTokens(loginRequest.UserName);
                }

                //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
                loginRequest.LoginDate = DateTime.Now;
                loginRequest.PasswordHash = valueHash;
                _logger.LoginLog(loginRequest);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }

        public bool ChangePassword(ChangedPassword newPassword)
        {
            bool result = false;
            try
            {
                if (newPassword.NewPassword == newPassword.ConfirmPassword)
                {
                    PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(newPassword.UserName);
                    if (Hasher.ValidateHash(newPassword.CurrentPassword, passwordLogin.PasswordSalt,
                        passwordLogin.PasswordHash, out _))
                    {
                        PasswordLogin newPasswordLogin = Hasher.HashPassword(newPassword.NewPassword);
                        newPasswordLogin.UserId = passwordLogin.UserId;
                        newPasswordLogin.ChangeDate = DateTime.Now;
                        passwordLogin = newPasswordLogin;
                        _authenticationRepository.UpdatePasswordLogin(newPasswordLogin);
                        result = true;
                    }

                    //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
                    _logger.PasswordChangeLog(passwordLogin);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return result;
            }

            return result;
        }

        public bool LockUnlockUser(LockUnlockUser lockUnlockUser)
        {
            try
            {
                var isSuccess = _authenticationRepository.LockUnlockUser(lockUnlockUser);
                _logger.LockedUserLog(lockUnlockUser);
                return isSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }

        public List<LoginHistory> GetLoginHistory(int userId)
        {
            try
            {
                return _authenticationRepository.GetLoginHistory(userId);
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }

        public RefreshedAccessToken RefreshToken(string authorization)
        {
            try
            {
                var refreshToken = authorization.Substring(authorization.IndexOf(' ') + 1);

                if (new JwtSecurityTokenHandler().ReadToken(refreshToken) is JwtSecurityToken jwt)
                {
                    return GenerateRefreshedAccessToken(jwt.Subject);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return null;
            }
        }

        public bool SendOtp(string userName)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }

        public bool ForgotPassword(ForgotPassword forgotPassword)
        {
            bool result = false;
            try
            {
                if (forgotPassword.NewPassword == forgotPassword.ConfirmPassword)
                {
                    PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(forgotPassword.UserName);
                    var otpDetails = _authenticationRepository.GetOtp(passwordLogin.UserId);
                    if (otpDetails.otp == forgotPassword.otp)
                    {
                        var newPasswordLogin = Hasher.HashPassword(forgotPassword.NewPassword);
                        newPasswordLogin.UserId = passwordLogin.UserId;
                        newPasswordLogin.ChangeDate = DateTime.Now;
                        passwordLogin = newPasswordLogin;
                        _authenticationRepository.UpdatePasswordLogin(newPasswordLogin);
                        result = true;
                    }

                    //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
                    _logger.PasswordChangeLog(passwordLogin);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return result;
            }

            return result;
        }

        public bool ValidateOtp(string userName, string otp)
        {
            try
            {
                PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
                var otpDetails = _authenticationRepository.GetOtp(passwordLogin.UserId);
                if (otpDetails.otp == otp)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }

        public bool UpdateMobileConfirmationStatus(string userName, string otp)
        {
            try
            {
                PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
                var otpDetails = _authenticationRepository.GetOtp(passwordLogin.UserId);
                if (otpDetails.otp == otp)
                {
                    _authenticationRepository.UpdateMobileConfirmationStatus(passwordLogin.UserId, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
        }
        public bool UpdateEmailConfirmationStatus(string userName, string otp)
        {
            try
            {
                PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(userName);
                var otpDetails = _authenticationRepository.GetOtp(passwordLogin.UserId);
                if (otpDetails.otp == otp)
                {
                    _authenticationRepository.UpdateEmailConfirmationStatus(passwordLogin.UserId, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogException(new ExceptionLog
                {
                    ExceptionDate = DateTime.Now,
                    ExceptionMsg = ex.Message,
                    ExceptionSource = ex.Source,
                    ExceptionType = "UserService",
                    FullException = ex.StackTrace
                });
                return false;
            }
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
            _logger.LoginTokenLog(loginToken);

            return loginToken;
        }
        private RefreshedAccessToken GenerateRefreshedAccessToken(string userName)
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

            var refreshedAccessToken = new RefreshedAccessToken
            {
                UserName = userName,
                AccessToken = encodedAccessJwt,
                AccessTokenExpiry = DateTime.Now.AddDays(1)
            };

            _authenticationRepository.UpdateAccessToken(refreshedAccessToken);

            //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
            _logger.LoginTokenLogForRefreshToken(refreshedAccessToken);

            return refreshedAccessToken;
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

            var roles = _roleRepository.GetRoleWithOrg(sub);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Item2));
            }

            claims.Add(new Claim("OrganizationId", roles[0].Item1.ToString()));
            return claims;
        }
        #endregion
    }
}
