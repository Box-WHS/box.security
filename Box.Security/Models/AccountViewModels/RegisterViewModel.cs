using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Box.Security.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Benutzername")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und maximal {1} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort bestätigen")]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name="Vorname")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
    }
}
