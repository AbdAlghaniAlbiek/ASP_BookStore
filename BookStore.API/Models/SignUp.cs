using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Models
{
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
