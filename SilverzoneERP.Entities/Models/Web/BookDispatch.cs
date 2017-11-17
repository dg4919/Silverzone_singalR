using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class BookDispatch:Entity<long>
    {
        [MaxLength(50)]
        public string PacketID { get; set; }

        [MaxLength(150)]
        public string ItemDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime DispatchDate { get; set; }
        public decimal Weight { get; set; }

        [MaxLength(50)]
        public string DispatchMode { get; set; }

        [MaxLength(50)]
        public string CourierName { get; set; }

        [MaxLength(50)]
        public string ConsignmentNumber { get; set; }

        [MaxLength(200)]
        public string Remarks { get; set; }

        [MaxLength(15)]
        public string EventCode { get; set; }
        [MaxLength(15)]
        public string SchCode { get; set; }

        [MaxLength(50)]
        public string DeliveryStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime StatusDate { get; set; }

        [MaxLength(50)]
        public string ReceivedBy { get; set; }
    }
}
