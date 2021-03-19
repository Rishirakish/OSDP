using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class LockUnlockUser : Entity
    {
        public string UserName { get; set; }
        public bool Locked { get; set; }
    }
}
