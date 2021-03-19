using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface ISecurityParameterService
    {
        List<SecurityParameter> Get();
        SecurityParameter Get(int id);
        int Insert(SecurityParameter securityParameterModel);
        int Update(int id, SecurityParameter updatedSecurityParameter);
        bool Delete(int id);
    }
}
