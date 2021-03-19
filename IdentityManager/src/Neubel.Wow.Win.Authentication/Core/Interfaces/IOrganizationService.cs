using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IOrganizationService
    {
        List<Organization> Get();
        Organization Get(int id);
        int Insert(Organization organizationModel);
        int Update(int id, Organization updatedOrganization);
        bool Delete(int id);
    }
}
