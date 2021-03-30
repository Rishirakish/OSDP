using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IRoleService
    {
        List<Role> Get();
        Role Get(int id);
        List<string> Get(string userName);
        int Insert(Role roleModel);
        int Update(int id, Role updatedRole);
        bool Delete(int id);
    }
}
