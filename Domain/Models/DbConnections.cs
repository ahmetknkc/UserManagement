using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DbConnections
    {
        public static string IdentityDB = "Server=DESKTOP-N8PHKH2\\TEW_SQLEXPRESS;Database=um_IdentityUserManagement;TrustServerCertificate=True;Trusted_Connection=True;";
        public static string EntityDB = "Server=DESKTOP-N8PHKH2\\TEW_SQLEXPRESS;Database=um_EntityUserManagement;TrustServerCertificate=True;Trusted_Connection=True;";
    }
}
