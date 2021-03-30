using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public interface IOrganizationRepository
    {
        int Insert(Organization user);
        int Update(Organization user);
        List<Organization> Get();
        Organization Get(int id);
        bool Delete(int id);
    }
}
