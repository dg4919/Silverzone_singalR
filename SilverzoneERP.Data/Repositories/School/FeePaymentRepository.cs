using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
using SilverzoneERP.Entities;
using System;
using System.Collections;

namespace SilverzoneERP.Data
{
    public class FeePaymentRepository : BaseRepository<FeePayment>, IFeePaymentRepository
    {
        public FeePaymentRepository(SilverzoneERPContext context) : base(context) { }

        
        public dynamic MiniStatement(long SchoolId, PaymentAgainst _paymentAgainst)
        {
            Hashtable letterMode = new Hashtable();
            letterMode.Add(1, "Courier Services");
            letterMode.Add(2, "Speed Post");
            letterMode.Add(3, "Registered Post");
            letterMode.Add(4, "By hand");
            letterMode.Add(5, "Adjust");

            Hashtable Mode = new Hashtable();
            Mode.Add(1, "Cash");
            Mode.Add(2, "Cheuque");
            Mode.Add(3, "DD");
            Mode.Add(4, "Online Transfer");
            Mode.Add(5, "Bank Deposit");
            Mode.Add(6, "Adjust");


            return _dbContext.EventManagements.Where(x => x.SchId == SchoolId && x.Status == true).ToList().Select(x => new
            {
                x.EventManagementYear,
                x.EventId,
                x.Event.EventCode,
                x.Event.SubjectName,
                IsFavour= x.FeePayment.Count(f => f.Status == true && f.PayAgainst == (int)_paymentAgainst) !=0?true:false,
                TotalPaid=x.FeePayment.Where(f => f.Status == true && f.PayAgainst == (int)_paymentAgainst).Sum(f=>f.Payment),
                TotalAmount = PaymentAgainst.Registration== _paymentAgainst? (x.TotalEnrollmentSummary * (_dbContext.EventYears.Where(ey => ey.Event_Year == x.EventManagementYear && ey.Status == true && ey.EventId == x.EventId).Select(ey => new { EventFee = ey.EventFee - ey.RetainFee }).FirstOrDefault().EventFee)):(PaymentAgainst.Both == _paymentAgainst ?0: TotalBookPrice(x.Id)) ,
                MiniStatement = x.FeePayment.Where(f=>f.Status==true && f.PayAgainst== (int)_paymentAgainst).OrderBy(or => or.CreationDate).Select(fp => new
                {
                    fp.DepositOnBank.AccountNo,
                   
                    fp.ReceiptNo,
                    fp.ReceiptDate,
                    fp.Payment,                    
                    Mode = Mode[fp.Mode] + (fp.Mode == 1 ? "(" + (fp.Type == 1 ? "Depositor" : "Receiver") + " By :- " + fp.Depositor_Receiver + ")" : (fp.Mode==6?"": "(No. : " + fp.Cheque_DD_Reference_Receipt_No + ")")),
                    LetterMode = letterMode[fp.LetterMode],
                    fp.Cheque_DD_Reference_Receipt_No,
                    fp.Depositor_Receiver,
                    fp.FavourOfId,
                    fp.DrawnOnBankId,
                    CreatedBy = _dbContext.ERPusers.FirstOrDefault(er => er.Id == fp.CreatedBy).UserName,
                    Type= Enum.GetName(typeof(PaymentType), fp.Type),
                })
                //,
                //NetBalance = x.FeePayment.Count()==0?x.TotalEnrollmentSummary * (_dbContext.EventYears.Where(ey => ey.Event_Year == x.EventManagementYear && ey.Status == true && ey.EventId == x.EventId).Select(ey => new { EventFee = ey.EventFee - ey.RetainFee }).FirstOrDefault().EventFee):(x.FeePayment.Where(f=>f.Status==true).OrderByDescending(or=>or.CreationDate).Select(y => new
                //{
                //    NetBalance = y.NetBalance - y.Payment,
                //}).FirstOrDefault().NetBalance)
            });            
        }
              
        private decimal TotalBookPrice(long EventManagementId)
        {
            try
            {
                decimal TotalPrice = 0;
                var data = _dbContext.PurchaseOrder_Masters.Where(x => x.From == orderSourceType.School && x.To == orderSourceType.Silverzone && x.srcFrom == EventManagementId).ToList();
                if (data.Count != 0)
                {
                    TotalPrice = data.Select(x => new {
                        TotalPrice = x.PurchaseOrders == null ? 0 : x.PurchaseOrders.Sum(po => po.Book.Price * po.Quantity)
                    }).Sum(x => x.TotalPrice);
                }
                return TotalPrice;
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
        public dynamic History(long SchoolId)
        {           
            var data = _dbContext.EventManagements.Where(x => x.SchId == SchoolId).ToList().Select(x => new
            {
                x.EventManagementYear,
                Event = x.Event.EventCode + x.EventManagementYear.ToString().Substring(2, 2),
                Students = x.TotalEnrollmentSummary,
                NetBalance = x.TotalEnrollmentSummary * (_dbContext.EventYears.Where(ey => ey.Event_Year == x.EventManagementYear && ey.Status == true && ey.EventId == x.EventId).Select(ey => new { EventFee = ey.EventFee - ey.RetainFee }).FirstOrDefault().EventFee),
                Payment = x.FeePayment.Where(f => f.Type == 1 && f.PayAgainst == (int)PaymentAgainst.Registration).Sum(f => f.Payment),
                Adjust = x.FeePayment.Where(f => f.Type == 3 && f.PayAgainst == (int)PaymentAgainst.Registration).Sum(f => f.Payment)
            });
            return new { data = data,Summary=new { Students =data.Sum(x=>x.Students), NetBalance = data.Sum(x => x.NetBalance), Payment = data.Sum(x => x.Payment), Adjust = data.Sum(x => x.Adjust) } };

        }
    }
}
