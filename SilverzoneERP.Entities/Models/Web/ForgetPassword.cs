using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    [Table("Forget_Password")]
    public class ForgetPassword : verification
    {
        public verificationMode verificationMode { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public abstract class verification : Entity<long>
    {
        public int sms_code { get; set; }
        public int max_attempt { get; set; }
        public DateTime valid_time { get; set; }
        public DateTime max_attempt_unlock_date { get; set; }
    }


}
