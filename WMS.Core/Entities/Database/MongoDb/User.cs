using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Database.MongoDb
{
    public class User : MongoDbBaseEntity
    {
        public string? NameSurname { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public UserRole? Role { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class UserRole : MongoDbBaseEntity
    {
        public string? RoleType { get; set; }
    }
}
