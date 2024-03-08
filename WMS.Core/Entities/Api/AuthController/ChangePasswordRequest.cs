using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.AuthController
{
    public class ChangePasswordRequest
    {
        public string? currentPassword { get; set; }
        public string? newPassword { get; set; }
        public string? userName { get; set; }
    } 

    public class ChangeUserNameRequest
    {
        public string? userId { get; set; }
        public string? newUserName { get; set; }
    }
}
