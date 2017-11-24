using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Box.Security.Data.Types;
using Box.Security.Data.Types.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Box.Security.Data.TransferData
{
    public class UserData : IUser
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        public bool Enabled { get; set; }

        [Required]
        [StringLength(255)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        
        [Required]
        public string Captcha { get; set; }
    }
}
