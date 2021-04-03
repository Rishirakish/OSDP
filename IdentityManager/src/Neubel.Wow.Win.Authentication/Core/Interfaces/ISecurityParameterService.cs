using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface ISecurityParameterService
    {
        List<SecurityParameter> Get(SessionContext sessionContext);
        SecurityParameter Get(SessionContext sessionContext, int id);
        int Insert(SessionContext sessionContext, SecurityParameter securityParameterModel);
        int Update(SessionContext sessionContext, int id, SecurityParameter updatedSecurityParameter);
        bool Delete(int id);
    }
}
