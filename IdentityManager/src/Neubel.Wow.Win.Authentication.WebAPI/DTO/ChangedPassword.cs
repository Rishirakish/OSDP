using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    public class ChangedPassword
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
