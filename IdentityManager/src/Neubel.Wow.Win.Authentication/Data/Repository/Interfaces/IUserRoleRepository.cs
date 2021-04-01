﻿using System;
using System.Collections.Generic;
using System.Text;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces
{
    public interface IUserRoleRepository
    {
        List<UserRole> Get();
        List<UserRole> Get(int userId);
        int Add(UserRole userRole);
        bool Delete(int id);
    }
}
