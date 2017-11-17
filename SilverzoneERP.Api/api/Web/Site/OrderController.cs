using Newtonsoft.Json;
using SilverzoneERP.Entities.ViewModel.Site;
using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.Site
{
    [Authorize]          // to check user is authenticate or not
    public class OrderController : ApiController
    {
        IUserShippingAddressRepository userShippingAddressRepository;
        IOrderDetailRepository orderDetailRepository;
        IOrderRepository orderRepository;
        IAccountRepository accountRepository;
        IErrorLogsRepository errorLogsRepository;

        //  *********************   For User shipping address ************

        public IHttpActionResult get_shipping_Address_byUserId()
        {
            var userId = Convert.ToInt32(User.Identity.Name);           // to get forma authrntication current user login Info

            if (userId == 0)
                return NotFound();      // user not found > throw 404 error

            var shipping_address = shippingAdresModel.Parse(
                userShippingAddressRepository.GetByUserId(userId)
                );

            return Ok(new { result = shipping_address });
        }

        [HttpPost]
        public IHttpActionResult create_uesr_shipping_Address(shippingAdresModel _model)
        {
            string _result = string.Empty;

            var model = shippingAdresModel.Parse(_model);       // return new 
            model.UserId = Convert.ToInt32(User.Identity.Name);
            model.create_date = userShippingAddressRepository.get_DateTime();
            model.Status = true;

            userShippingAddressRepository.Create(model);
            _result = "success";

            return Ok(new { result = _result });
        }

        [HttpPost]
        public void update_uesr_shipping_Address(shippingAdresModel model)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            var entity = userShippingAddressRepository.FindBy(
                x => x.Id == model.Id &&
                x.UserId == userId              // dont write line > Convert.ToInt32(User.Identity.Name); // will throw error
                ).SingleOrDefault();            // we have unique adress id, so will get always 1 or 0 record

            entity.Address = model.Address;
            entity.City = model.City;
            entity.CountryType = model.Country;
            entity.Email = model.Email;
            entity.Mobile = model.Mobile;
            entity.PinCode = model.PinCode;
            entity.State = model.State;
            entity.Username = model.Username;

            userShippingAddressRepository.Update(entity);

            //return Ok(new { result = _result });
        }

        [HttpPost]
        public void remove_uesr_shipping_Address(int adresId)
        {
            var entity = userShippingAddressRepository.GetById(adresId);

            if (entity != null)
            {
                //_userShippingAddressRepository.Delete(entity);
                entity.Status = false;

                userShippingAddressRepository.Update(entity);
            }
        }

        //  *********************   For orders ************

        [HttpPost]
        public IHttpActionResult create_order(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { result = "error" });

            using (var transaction = orderRepository.BeginTransaction())
            {
                try
                {
                    var order = orderRepository.Create(new Order()      // insert data in order table
                    {
                        UserId = Convert.ToInt32(User.Identity.Name),
                        Shipping_addressId = model.Shipping_addressId,
                        Total_Shipping_Amount = model.Total_Shipping_Amount,
                        Total_Shipping_Charges = model.Total_Shipping_Charges,
                        OrderDate = orderRepository.get_DateTime(),
                    });              // getting new inserted Id in the table

                    order.OrderNumber = orderRepository.get_Order_Number(order.Id);
                    orderRepository.Update(order);

                    foreach (var entity in model.bookViewModel)     // insert data in order detail table
                    {
                        var _model = new OrderDetail()
                        {
                            Quantity = entity.Quantity,
                            OrderId = order.Id,
                            bookType = entity.bookType,
                            UnitPrice = entity.unitPrice
                        };

                        if (entity.bookType == bookType.Book)
                            _model.BookId = entity.BookId;
                        else
                            _model.BundleId = entity.BookId;

                        orderDetailRepository.Create(_model, false);
                    }

                    orderDetailRepository.Save();
                    transaction.Commit();       // it must be there if want to save record :)

                    int userid = Convert.ToInt32(User.Identity.Name);
                    var points = accountRepository.GetById(userid).TotalPoint;
                    return Ok(new { result = "success", orderId = order.Id, quizPoints = points });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult confirm_order(
            int oderId, 
            orderType orderMode,
            int? Quiz_Points_Deduction = null
            )
        {
            var order = orderRepository.GetById(oderId);

            if (order == null)
                return Ok(new { status = "notfound" });

            var data = orderPriceViewModel.Parse(orderDetailRepository.GetByOrderId(oderId));

            if (!orderPriceViewModel.validate_orderAmount(data))     // validate amount of order
                return Ok(new { status = "notmatched" });            // if amount is not matched/validated

            var user = accountRepository.FindById(Convert.ToInt32(User.Identity.Name));
            if (Quiz_Points_Deduction > user.TotalPoint)
                return Ok(new { status = "error" });    // Redeem point is less than total points > so payment could not process

            user.TotalPoint -= Convert.ToInt32(Quiz_Points_Deduction) * 10;
            if (Quiz_Points_Deduction != null)
                accountRepository.Update(user);

            // ********************  Proceed to order confirmation  **********

            order.Payment_ModeType = orderMode;
            order.Payment_Status = orderMode == orderType.Online ? true : false;        // 0 if payment not made, 1 if made
            order.Order_Deliver_StatusType = orderStatusType.Pending;
            order.Quiz_Points_Deduction = Quiz_Points_Deduction;

            orderRepository.Update(order);

            // send order confirmaton by sms on mobile
            orderRepository.send_sms_orderConfirmation(
                userShippingAddressRepository.GetById(order.Shipping_addressId).Mobile,
                order.OrderNumber,
                order.Total_Shipping_Amount + order.Total_Shipping_Charges,
                "http://www.silverzone.org"
                );

            // new anonymoustype object  data
            var _result = new { OrderNumber = order.OrderNumber, OrderDate = order.OrderDate };
            return Ok(new
            {
                status = "success",
                result = _result
            });

        }

        [HttpPost]
        public void sendEmail(emailViewModel model)
        {
            //string path = HostingEnvironment.MapPath("~/templates/EmailTemplates/orderConfirmation_offline.html");
            orderRepository.sendEmail_Payment_Confirmation(model.HtmlTemplate, model.emailId);
        }

        [HttpGet]
        public IHttpActionResult get_location(string pincode)
        {
            var result = string.Empty;
            try
            {
                WebRequest myWebRequest = WebRequest.Create(string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=true", Uri.EscapeDataString(pincode)));

                using (var response = myWebRequest.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var data = js.DeserializeObject(reader.ReadToEnd());
                        result = JsonConvert.SerializeObject(data);
                        //return Ok(1);
                    }
                }
            }

            catch (Exception ex)
            {
                errorLogsRepository.logError(ex);
            }
            return Ok(result);
        }

        // *****************  Constructors  ********************************

        public OrderController(
            IUserShippingAddressRepository _userShippingAddressRepository,
            IOrderDetailRepository _orderDetailRepository,
            IOrderRepository _orderRepository,
            IAccountRepository _accountRepository,
            IErrorLogsRepository _errorLogsRepository
            )
        {
            userShippingAddressRepository = _userShippingAddressRepository;
            orderDetailRepository = _orderDetailRepository;
            orderRepository = _orderRepository;
            accountRepository = _accountRepository;
            errorLogsRepository = _errorLogsRepository;
        }

    }
}
