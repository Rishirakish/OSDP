using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IRoleRepository
    { 
        int Insert(Role role);
        int Update(Role role);
        List<Role> Get();
        Role Get(int id);
        List<string> Get(string userName);
        bool Delete(int id);
    }
}
