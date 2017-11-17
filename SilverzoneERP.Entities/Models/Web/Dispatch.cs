using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Dispatch_otherItem_Master : AuditableEntity<long>
    {
        // schoool & other will be come here :)
        public orderSourceType sourceType { get; set; }
        public long? sourceId { get; set; }     // for school => schoolId & other => null

        // for broucher & other items
        public itemType ItemType { get; set; }
        public string Items { get; set; }

        public bool packet_isOn_Hold { get; set; }
    }

    public class Dispatch_otherItem_address : Entity<long>
    {
        [ForeignKey("Id")]      // 1 to 1 relation => so we directly find this table and go to base tbl from here
        public virtual Dispatch_otherItem_Master Dispatch_otherItem_Master { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

        //public string City { get; set; }
        //public string State { get; set; }
        //public string District { get; set; }

        public string PhoneNo { get; set; }
        public int PinCode { get; set; }
        public string ContactPerson { get; set; }

        public labelType LabelType { get; set; }
        public string LabelName { get; set; }
        public long? CordinatorId { get; set; }
        public long CityId { get; set; }

        public virtual City CityInfo { get; set; }


    }

    public class Dispatch_Master : Entity<long>
    {
        public long? ChallanId { get; set; }
        [ForeignKey("ChallanId")]
        public virtual Stock_Master stockMasters { get; set; }

        public long? OrderId { get; set; }
        public virtual Order Order { get; set; }

        public long? OtherItemId { get; set; }
        [ForeignKey("OtherItemId")]
        public virtual Dispatch_otherItem_Master Dispatch_otherItem_Master { get; set; }

        // source of order
        public orderSourceType DispatchType { get; set; }

        public virtual Dispatch DispatchInfo { get; set; }
    }

    public class Dispatch : AuditableEntity<long>
    {
        [ForeignKey("Id")]
        public virtual Dispatch_Master Dispatch_Master { get; set; }

        public string Packet_Id { get; set; }
        public string Invoice_No { get; set; }
        
        public string Packet_Consignment { get; set; }

        public string print_reason { get; set; }
        public DateTime? Dispatch_Date { get; set; }

        public string Recieved_By { get; set; }
        public DateTime? Recieved_Date { get; set; }

        public long? CourierModeId { get; set; }
        public virtual CourierMode CourierMode { get; set; }

        public bool Wheight_isVerified { get; set; }
        public orderStatusType Order_StatusType { get; set; }

        public string Track_Remarks { get; set; }

        // when packet is lost then resend the same pckt, so take its refrence
        public long RefrenceId{ get; set; }
    }

    public class DispatchLogs : Entity<long>
    {
        public long DispatchId { get; set; }

        [ForeignKey("DispatchId")]
        public virtual Dispatch Dispatch { get; set; }

        public string Action { get; set; }
        public DateTime Action_Date { get; set; }
    }

}
