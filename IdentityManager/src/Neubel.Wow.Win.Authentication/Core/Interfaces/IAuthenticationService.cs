using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IAuthenticationService
    {
        LoginToken Login(LoginRequest loginRequest);
        bool ChangePassword(ChangedPassword newPassword);
        bool LockUnlockUser(LockUnlockUser lockUnlockUser);
    }
}