//
// Author   : Vinayak Ushakola
// Date     : 29 June 2020
// Purpose  : It Contains Model that User may Request
//

using System.ComponentModel.DataAnnotations;

namespace ChatAppCommonLayer.RequestModels
{
    public class RegistrationRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Your FirstName Length Should be more than 3")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Your LastName Length Should be more than 3")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please Enter a Proper Email-ID")]
        public string Email { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Your Password Should be Minimum Length of 5")]
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter a Proper Email-ID")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Your Password Should be Minimum Length of 8")]
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter a Proper Email-ID")]
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        [Required]
        [MinLength(8, ErrorMessage = "Your Password Should be Minimum Length of 8")]
        public string Password { get; set; }
    }
}
