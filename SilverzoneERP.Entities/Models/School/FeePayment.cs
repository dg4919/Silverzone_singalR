using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SilverzoneERP.Entities.Models
{
    public class FeePayment : AuditableEntity<long>
    {
        [Required]
        public long EventmanagementId { set; get; }

        public long ReceiptNo { set; get; }

        public DateTime ReceiptDate { set; get; } = DateTime.Now;

        [Required]
        public int Mode { set; get; }


        public Nullable<long> MICRCode { set; get; }

        [Required]
        public long FavourOfId { set; get; }

        public Nullable<long> DrawnOnBankId { set; get; }

        [NotMapped]
        public string OtherBank { set; get; }

        public long DepositOnBankId { set; get; }

        [Required]
        public int LetterMode { set; get; }

        public string Depositor_Receiver { set; get; }

        public string Cheque_DD_Reference_Receipt_No { set; get; }

        [Required]
        public int PayAgainst { set; get; }

        public long Payment { set; get; }

        [Required]
        public DateTime PaymentDate { set; get; }

        [MaxLength(200)]
        public string Remarks { set; get; }

        [Required]
        public int Type { set; get; }

        [ForeignKey("FavourOfId")]
        public virtual InFavourOf InFavourOf { get; set; }

        [ForeignKey("DepositOnBankId")]
        public virtual DepositOnBank DepositOnBank { get; set; }

        [ForeignKey("DrawnOnBankId")]
        public virtual DrownOnBank DrownOnBank { get; set; }

        [ForeignKey("EventmanagementId")]
        public virtual EventManagement Eventmanagement { get; set; }
    }
}
