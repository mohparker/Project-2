using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Registration_with_email_validation.Models
{

     [MetadataType (typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; } //Adding additional field

    }

    public class UserMetadata
    {
        //Validating the user input 

        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public string username { get; set; }
     

        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string emailID { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = " Minimum 6 characters")]
        public string password { get; set; }


    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Sorry the passsword do not match")]
    public string confirmPassword { get; set; }
    }

}