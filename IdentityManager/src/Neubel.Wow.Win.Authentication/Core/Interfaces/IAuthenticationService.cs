using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IAuthenticationService
    {
        LoginToken Login(LoginRequest loginRequest);
        bool ChangePassword(ChangedPassword newPassword);
        bool LockUnlockUser(SessionContext sessionContext, LockUnlockUser lockUnlockUser);
        List<LoginHistory> GetLoginHistory(SessionContext sessionContext, int userId);
        RefreshedAccessToken RefreshToken(string authorization);
        bool ForgotPassword(ForgotPassword forgotPassword);
        bool SendOtp(string userName);
        bool ValidateOtp(string userName, string otp);
        bool UpdateMobileConfirmationStatus(string userName, string otp);
        bool UpdateEmailConfirmationStatus(string userName, string otp);
    }
}