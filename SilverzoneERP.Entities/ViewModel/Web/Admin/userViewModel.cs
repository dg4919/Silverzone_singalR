using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.ViewModel.Admin
{
    public class userViewModel
    {
        [Required]
        public string[]  userName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}