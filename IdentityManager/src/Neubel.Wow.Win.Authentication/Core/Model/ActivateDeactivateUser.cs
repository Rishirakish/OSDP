using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class ActivateDeactivateUser
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
