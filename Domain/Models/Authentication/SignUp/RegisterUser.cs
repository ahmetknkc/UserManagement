using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Authentication.SignUp
{
    public class RegisterUser
    {

        [Required(ErrorMessage = "Username is required.")]
        public string? userName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-Mail is required.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? password { get; set; }



    }
}
