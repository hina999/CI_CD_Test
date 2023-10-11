using FXAdminTransferConnex.Entities.LocalizationResource;
using System.ComponentModel.DataAnnotations;


namespace FXAdminTransferConnex.Entities
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = @"Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "PasswordRequired")]
        public string Password { get; set; }

        [Required]
        [Display(Name = @"Confirm Password")]
        [Compare("Password", ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "ConfirmPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }

        [Display(Name = @"Remember me?")]
        public bool RememberMe { get; set; }
    }
}
