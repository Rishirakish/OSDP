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
        private readonly IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IConfiguration configuration, IRoleService roleService, IUserRepository userRepository, IAuthenticationRepository authenticationRepository)
        {
            _configuration = configuration;
            _roleService = roleService;
            _authenticationRepository = authenticationRepository;
        }


        #region Public Methods

        public LoginToken Login(LoginRequest loginRequest)
        {
            var passwordLogin = _authenticationRepository.GetLoginPassword(loginRequest.UserName);
            string valueHash = string.Empty;
            if (passwordLogin != null && Hasher.ValidateHash(loginRequest.Password, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out valueHash))
            {
                return GenerateTokens(loginRequest.UserName);
            }

            loginRequest.LoginDate = DateTime.Now;
            loginRequest.PasswordHash = valueHash;
            _authenticationRepository.LoginLog(loginRequest);

            return null;
        }

        public bool ChangePassword(ChangedPassword newPassword)
        {
            PasswordLogin passwordLogin = _authenticationRepository.GetLoginPassword(newPassword.UserName);
            string valueHash;
            if (Hasher.ValidateHash(newPassword.CurrentPassword, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out valueHash))
            {
                var newPasswordLogin = Hasher.HashPassword(newPassword.NewPassword);
                newPasswordLogin.UserId = passwordLogin.UserId;
                return _authenticationRepository.UpdatePasswordLogin(newPasswordLogin);
            }
            return false;
        }

        public bool LockUnlockUser(LockUnlockUser lockUnlockUser)
        {
            return _authenticationRepository.LockUnlockUser(lockUnlockUser);
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

            //var token = new TokenRepository(Startup.connString);
            //token.AddForUser(email, "access", encodedAccessJwt);
            //token.AddForUser(email, "refresh", encodedRefreshJwt);

            var result = new LoginToken
            {
                AccessToken = encodedAccessJwt,
                AccessTokenExpiry = DateTime.Now.AddDays(1),
                RefreshToken = encodedRefreshJwt,
                RefreshTokenExpiry = DateTime.Now.AddDays(30),
            };

            return result;
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

        #endregion
    }
}
