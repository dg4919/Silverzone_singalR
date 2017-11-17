using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System.IO;
using System.Drawing.Imaging;
using System;
using System.Drawing;
using System.Data.Entity;
using SilverzoneERP.Entities;

namespace SilverzoneERP.Data
{
    public class Dispatch_otherItem_MasterRepository : BaseRepository<Dispatch_otherItem_Master>, IDispatch_otherItem_MasterRepository
    {
        public Dispatch_otherItem_MasterRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class Dispatch_otherItem_addressRepository : BaseRepository<Dispatch_otherItem_address>, IDispatch_otherItem_addressRepository
    {
        public Dispatch_otherItem_addressRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class Dispatch_MasterRepository : BaseRepository<Dispatch_Master>, IDispatch_MasterRepository
    {
        public Dispatch_MasterRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Dispatch_Master GetBychallanId(long challanId)
        {
            return _dbset.SingleOrDefault(x => x.ChallanId == challanId);  // there will be always 1 record
        }

        public Dispatch_Master GetByorderId(long orderid)
        {
            return _dbset.SingleOrDefault(x => x.OrderId == orderid);  // there will be always 1 record
        }

        public IQueryable<Dispatch_Master> FilterbyTxt(string srchTxt)
        {
            return FindBy(x => x.Order.User.UserName == srchTxt
                            || x.Order.User.MobileNumber == srchTxt
                            || x.Order.User.EmailID == srchTxt);
        }

    }

    public class DispatchRepository : BaseRepository<Dispatch>, IDispatchRepository
    {
        public DispatchRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Dispatch GetByorderId(long orderid)
        {
            return _dbset.SingleOrDefault(x => x.Dispatch_Master.OrderId == orderid);  // there will be always 1 record
        }

        public Dispatch GetBy_otherItemId(long itemId)
        {
            return _dbset.SingleOrDefault(x => x.Dispatch_Master.OtherItemId == itemId);  // there will be always 1 record
        }

        public Dispatch GetBychallanId(long challanId)
        {
            return _dbset.SingleOrDefault(x => x.Dispatch_Master.ChallanId == challanId);  // there will be always 1 record
        }

        public bool GetByConsignment(string consignmentNo, int dispatchId)
        {
            //return _dbset.Any(x => x.Packet_Consignment.Contains(consignmentNo) && x.Id != dispatchId);    // use for like condison
            return _dbset.Any(x => x.Packet_Consignment == consignmentNo && x.Id != dispatchId);
        }

        public Dispatch GetBypacketId(string packetNumber)
        {
            return FindBy(x => x.Packet_Id.Equals(packetNumber)).SingleOrDefault();
        }

        public bool is_orderExist(int orderId)
        {
            return _dbset.Any(x => x.CourierModeId == orderId);
        }

        public string get_Packet_Number(long identityValue, orderSourceType srcType)
        {
            int packet_number = 1000000;

            var order_date = string.Format("{0: yyyyMMdd}", get_DateTime()).Trim();

            string orderType = string.Empty;

            switch (srcType)
            {
                case orderSourceType.Online:
                    orderType = "ONL";
                    break;
                case orderSourceType.School:
                    orderType = "SCH";
                    break;
                case orderSourceType.Dealer:
                    orderType = "DLR";
                    break;
                case orderSourceType.Other:
                    orderType = "OTH";
                    break;
                default:
                    orderType = string.Empty;
                    break;
            }

            return (string.Format("PKT/{0}{1}/{2}",
                    orderType,
                    order_date,
                    (packet_number + identityValue)
                    ));
        }


        public string get_Invoice_Number(long identityValue)
        {
            int packet_number = 1000000;
            string invoiceNo = (packet_number + identityValue).ToString();

            return (invoiceNo);
        }

        public string getbase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }

        }

        public IQueryable<Dispatch> FilterByDate(DateTime fromDate, DateTime toDate)
        {
            return FindBy(x => DbFunctions.TruncateTime(x.CreationDate) >= fromDate
                             && DbFunctions.TruncateTime(x.CreationDate) <= toDate
                             && x.Status == true);
        }

        public IQueryable<Dispatch> FilterByInvoice(IQueryable<Dispatch> result, string invoiceNo)
        {
            return result.Where(x => x.Invoice_No.Equals(invoiceNo));
        }

        public IQueryable<Dispatch> FilterByInvoice(IQueryable<Dispatch> result, IQueryable<string> invoiceNos)
        {
            return result.Where(x => invoiceNos.Contains(x.Invoice_No));
        }

    }

    public class DispatchLogsRepository : BaseRepository<DispatchLogs>, IDispatchLogsRepository
    {
        public DispatchLogsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

}
