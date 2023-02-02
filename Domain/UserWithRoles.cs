using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserWithRoles
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
