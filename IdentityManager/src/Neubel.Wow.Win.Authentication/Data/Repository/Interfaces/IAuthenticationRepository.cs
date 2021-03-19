using Neubel.Wow.Win.Authentication.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IAuthenticationRepository
    {
        int LoginLog(LoginRequest loginRequest);
        bool UpdatePasswordLogin(PasswordLogin passwordLogin);
        bool LockUnlockUser(LockUnlockUser lockUnlockUser);
        PasswordLogin GetLoginPassword(string userName);
    }
}
