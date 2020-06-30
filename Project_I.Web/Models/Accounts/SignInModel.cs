using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_I.Web.Models.Accounts
{
    public class SignInModel
    {
        public bool RememberMe { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage = "Password required  6 char long")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
