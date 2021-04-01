using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IUserRoleService
    {
        List<UserRole> Get();
        List<UserRole> Get(int userId);
        int Add(UserRole userRole);
        bool Delete(int id);
    }
}
