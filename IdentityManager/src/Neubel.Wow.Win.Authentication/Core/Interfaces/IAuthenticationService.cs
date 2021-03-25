using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IAuthenticationService
    {
        LoginToken Login(LoginRequest loginRequest);
        bool ChangePassword(ChangedPassword newPassword);
        bool LockUnlockUser(LockUnlockUser lockUnlockUser);
        List<LoginHistory> GetLoginHistory(int userId);
        LoginToken RefreshToken(string authorization);
        bool ForgotPassword(string userName);
        bool SendOtp(string userName);
        bool validateOtp(string userName, string otp);
    }
}