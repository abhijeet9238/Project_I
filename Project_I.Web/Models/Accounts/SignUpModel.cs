using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Project_I.Web.Models.Accounts
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6,ErrorMessage ="Password required  6 char long")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage = "Password required  6 char long")]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Don't match")]
        public string ConfirmPassowrd { get; set; }
    }
}
