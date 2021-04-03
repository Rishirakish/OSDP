﻿using System.Collections.Generic;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    public interface IOrganizationService
    {
        List<Organization> Get(SessionContext sessionContext);
        Organization Get(SessionContext sessionContext, int id);
        int Insert(SessionContext sessionContext, Organization organizationModel);
        int Update(SessionContext sessionContext, int id, Organization updatedOrganization);
        bool Delete(int id);
    }
}
