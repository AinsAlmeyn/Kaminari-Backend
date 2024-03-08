using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.AuthController
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User Name can not be empty")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "User Name must be between 8-30 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can not be empty")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8-50 characters")]
        public string Password { get; set; }
    }

    public class UpdatePassword : LoginRequest
    {
        public string? UpdatedPassword { get; set; }
    }

    public class LoginRespose
    {
        public string? NameSurname { get; set; }
        public string? MongoId { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
