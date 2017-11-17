using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.ViewModel.Site
{

    public class loginModel
    {
        [Required]
        public string userName { get; set; }

        // input type use to check > user enter emailId/mobile at client side
        // "email" for EmailId OR  "mobile" for mobile No
        [Required]
        public verificationMode verificationMode { get; set; }
    }

    public class LoginViewModel : loginModel
    {
        [Required]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }      
    }

    public class OTPViewModel
    {
        [Required]
        public string mobileNumber { get; set; }
        public int sms_OTP { get; set; }
    }

    public class RegisterViewModel : OTPViewModel
    {
        public int RoleId { get; set; }
        public string Password { get; set; }
    }

    public class passwordRecoveryViewModel : loginModel
    {
        public long userId { get; set; }
        public int OTP_code { get; set; }

        public verficationType verficationType { get; set; }
    }

}
