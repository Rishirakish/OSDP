using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface ISecurityParameterRepository
    {
        List<SecurityParameter> Get();
        SecurityParameter Get(int id);
        int Insert(SecurityParameter securityParameter);
        int Update(SecurityParameter updatedSecurityParameter);
        bool Delete(int id);
    }
}
