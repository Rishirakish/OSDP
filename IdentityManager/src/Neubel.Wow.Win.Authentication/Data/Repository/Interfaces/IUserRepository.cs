using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IUserRepository
    {
        int Insert(User user, PasswordLogin passwordLogin);
        int Update(User user);
        List<User> Get();
        User Get(int id);
        bool ActivateDeactivateUser(ActivateDeactivateUser activateDeactivateUser);
        bool Delete(int id);
    }
}
