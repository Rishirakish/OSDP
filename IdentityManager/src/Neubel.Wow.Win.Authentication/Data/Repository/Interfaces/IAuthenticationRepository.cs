using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IAuthenticationRepository
    {
        List<LoginHistory> GetLoginHistory(int userId);
        int PasswordChangeLog(PasswordLogin passwordLogin);
        int LoginTokenLog(LoginToken loginToken);
        int LoginLog(LoginRequest loginRequest);
        int LockedUserLog(LockUnlockUser lockUnlockUser);
        int SaveLoginToken(LoginToken loginToken);
        bool UpdatePasswordLogin(PasswordLogin passwordLogin);
        bool LockUnlockUser(LockUnlockUser lockUnlockUser);
        PasswordLogin GetLoginPassword(string userName);
        int SaveOtp(UserValidationOtp userValidationOtp);
        UserValidationOtp GetOtp(int userId);
    }
}
