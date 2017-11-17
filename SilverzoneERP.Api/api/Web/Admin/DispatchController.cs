using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using SilverzoneERP.Entities.ViewModel.Site;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.ViewModel.Inventory;
using Silverzone.Web.ViewModel.Admin;

namespace SilverzoneERP.Api.api.Admin
{
    public class DispatchController : ApiController
    {
        IUserShippingAddressRepository userShippingAddressRepository;
        IOrderDetailRepository orderDetailRepository;
        IOrderRepository orderRepository;
        IDispatch_MasterRepository dispatch_MasterRepository;
        IDispatchRepository dispatchRepository;
        private IErrorLogsRepository errorLogsRepository;
        private ICourierRepository courierRepository;
        private ICourierModeRepository courierModeRepository;
        private IStock_MasterRepository stockMasterRepository;
        IinventorySourceDetailRepository inventorySourceDetailRepository { get; set; }
        private IPackage_MasterRepository package_MasterRepository;
        private IPacket_BundleInfoRepository packet_BundleInfoRepository;
        IEventManagementRepository eventManagementRepository { get; set; }
        IDispatch_otherItem_addressRepository dispatch_otherItem_addressRepository { get; set; }
        ICityRepository cityRepository { get; set; }
        IDispatchLogsRepository dispatchLogsRepository { get; set; }
        IOrderStatusReasonRepository orderStatusReasonRepository { get; set; }

        [HttpGet]
        public IHttpActionResult getordersInfo(DateTime date, int orderType) // 0 > Not printed || 1 > Printed
        {
            if (orderType == 0)     // challan has always a pckt id so not come in this portion
            {
                var _orderResult = from Order in orderRepository.GetAll()
                                   join dispatch in dispatch_MasterRepository.GetAll()
                                   on Order.Id equals dispatch.OrderId
                                   into Dispatch_join
                                   from myDispatch in Dispatch_join.DefaultIfEmpty()           // to create left join with dispatch Table
                                   where myDispatch.DispatchInfo.Packet_Id == null && Order.OrderDate >= date
                                   select new
                                   {
                                       Id = Order.Id,
                                       OrderNumber = Order.OrderNumber,
                                       orderAmount = Order.Total_Shipping_Amount + Order.Total_Shipping_Charges,
                                       OrderDate = Order.OrderDate,
                                       custmer_name = Order.UserShippingAddress.Username,
                                       order_source = myDispatch == null ? orderSourceType.Online.ToString() : myDispatch.DispatchType.ToString(),
                                       packetNumber = myDispatch.DispatchInfo.Packet_Id,
                                       orderType = orderSourceType.Online
                                   };

                var _otherDispatchResult = from otherDispatch in dispatch_otherItem_addressRepository.GetAll()
                                           join dispatch in dispatch_MasterRepository.GetAll()
                                           on otherDispatch.Id equals dispatch.OtherItemId
                                           into Dispatch_join
                                           from myDispatch in Dispatch_join.DefaultIfEmpty()           // to create left join with dispatch Table
                                           where myDispatch.DispatchInfo.Packet_Id == null && otherDispatch.Dispatch_otherItem_Master.UpdationDate >= date
                                           select new
                                           {
                                               //otherDispatch,
                                               //myDispatch
                                               Id = otherDispatch.Id,
                                               OrderNumber = otherDispatch.Id.ToString(),
                                               orderAmount = (decimal)0,
                                               OrderDate = otherDispatch.Dispatch_otherItem_Master.UpdationDate,
                                               custmer_name = otherDispatch.Name,
                                               order_source = myDispatch == null ? orderSourceType.Other.ToString() : myDispatch.DispatchType.ToString(),
                                               packetNumber = myDispatch == null ? string.Empty : myDispatch.DispatchInfo.Packet_Id,
                                               orderType = orderSourceType.Other
                                           };

                return Ok(new { result = _orderResult.Union(_otherDispatchResult) });
            }
            else   // challan info always show here coz it will have printed pckt id..
            {
                var _result = (from dispatch in dispatch_MasterRepository.GetAll()
                               join order in orderRepository.GetAll()         // for Online Orders
                               on dispatch.OrderId equals order.Id
                               into orderList
                               from order in orderList.DefaultIfEmpty()       // to create left join with dispatch Table
                               join stock in stockMasterRepository.GetAll()   // for challan items
                               on dispatch.ChallanId equals stock.Id
                               into stockList
                               from stock in stockList.DefaultIfEmpty()
                               join otherItem_dispatch in dispatch_otherItem_addressRepository.GetAll()    // for other dispatch items
                               on dispatch.OtherItemId equals otherItem_dispatch.Id
                               into otherItem_dispatchList
                               from otherItem in otherItem_dispatchList.DefaultIfEmpty()
                               where dispatch.DispatchInfo.CreationDate >= date
                               select new
                               {
                                   order,   // use these 3 values to check/debug code :)
                                   stock,
                                   dispatch,
                                   otherItem
                               }).ToList()
                               .Select(x => new
                               {
                                   Id = x.order == null ? (x.otherItem != null ? x.otherItem.Id : x.dispatch.ChallanId) : x.dispatch.OrderId,
                                   OrderNumber = x.order == null ? (x.otherItem != null ? x.otherItem.Id.ToString() : x.stock.ChallanNo) : x.order.OrderNumber,
                                   orderAmount = x.order == null ? (x.otherItem != null ? 0 : Inventory_create_ViewModel.getStock_TotalPrice(x.stock)) : (x.order.Total_Shipping_Amount + x.order.Total_Shipping_Charges),
                                   OrderDate = x.order == null ? (x.otherItem != null ? x.otherItem.Dispatch_otherItem_Master.CreationDate : x.stock.Stocks.FirstOrDefault().CreationDate) : x.order.OrderDate,
                                   custmer_name = x.order == null
                                                ? (x.otherItem != null ? x.otherItem.Name : (x.stock.SourceDetail == null
                                                ? eventManagementRepository.FindBy(y => y.SchId == x.stock.SourceInfo_Id).FirstOrDefault().School.SchName
                                                : x.stock.SourceDetail.Contact_Person_Name))
                                                : x.order.UserShippingAddress.Username,
                                   order_source = x.dispatch.DispatchType.ToString(),
                                   packetNumber = x.dispatch.DispatchInfo.Packet_Id,
                                   orderType = x.dispatch.DispatchType
                               });


                return Ok(new { result = _result });
            }
        }

        [HttpPost]
        public IHttpActionResult create_orderLabel(orderViewModel model)
        {
            using (var transaction = dispatch_MasterRepository.BeginTransaction())
            {
                try
                {
                    Dispatch packetInfo = null;
                    if (model.sourceType == orderSourceType.Online)
                        packetInfo = dispatchRepository.GetByorderId(model.Id);
                    else if (model.sourceType == orderSourceType.Other)            // for other dispatchItem
                        packetInfo = dispatchRepository.GetBy_otherItemId(model.Id);
                    else if (model.sourceType != orderSourceType.Online)            // for challans
                        packetInfo = dispatchRepository.GetBychallanId(model.Id);

                    string _status = "exist";

                    #region Create
                    if (packetInfo == null)
                    {
                        var master = dispatch_MasterRepository.Create(orderViewModel.parse(model));

                        packetInfo = dispatchRepository.Create(new Dispatch()
                        {
                            Id = master.Id,
                            Packet_Id = dispatchRepository.get_Packet_Number(master.Id, model.sourceType),
                            Invoice_No = dispatchRepository.get_Invoice_Number(master.Id),
                            Order_StatusType = orderStatusType.Pending,
                            Status = true
                        });
                        _status = "success";

                        dispatchLogsRepository.Create(new DispatchLogs()
                        {
                            DispatchId = master.Id,
                            Action = "Packet is created",
                            Action_Date = DateTime.Now,
                            Status = true
                        });
                    }
                    #endregion

                    dynamic orderInfo = null;

                    if (model.sourceType == orderSourceType.Online)        // for orders
                    {
                        orderInfo = orderDetailViewModel.Parse(
                           orderRepository.GetById(model.Id)
                           );
                    }
                    else if (model.sourceType == orderSourceType.Other)               // get record by challan no > means from stock table
                    {
                        var entity = dispatch_otherItem_addressRepository.FindById(model.Id);
                        var cityModel = cityRepository.FindById(entity.CityId);

                        orderInfo = DispatchViewModel.parse(entity, cityModel);
                    }
                    else if (model.sourceType != orderSourceType.Online)               // get record by challan no > means from stock table
                    {
                        var stock_Master = stockMasterRepository
                                               .FindById(Convert.ToInt64(packetInfo.Dispatch_Master.ChallanId));

                        var fromModel = inventorySourceDetailRepository
                                        .FindById(stock_Master
                                                  .Stocks.FirstOrDefault()
                                                  .PO_Master.srcTo);

                        var Inventory_Src = inventorySourceDetailRepository.GetAll();
                        var Event_SrcModel = eventManagementRepository.GetAll();

                        orderInfo = Inventory_create_ViewModel
                                    .Parse(stock_Master, Inventory_Src, Event_SrcModel);
                    }

                    Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                    var barCodeImg = barcode.Draw(packetInfo.Packet_Id, 50);

                    var _result = new
                    {
                        packetInfo = new { packetInfo.Invoice_No, packetInfo.Id, packetInfo.Packet_Id, packetInfo.CreationDate, imgBase = getbase64(barCodeImg) },     // new objects
                        orderInfo = orderInfo,
                        status = _status
                    };

                    transaction.Commit();
                    return Ok(new { result = _result });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpPost]
        public IHttpActionResult update_orderLabael(int Id, string reason)
        {
            var packetInfo = dispatchRepository.FindById(Id);
            packetInfo.print_reason = reason;

            dispatchRepository.Update(packetInfo);

            return Ok();
        }

        // ******************************  Dispatch Wheight  ******************************

        [HttpGet]
        public IHttpActionResult get_printedOrder(DateTime date, int wheightType) // 0 > Not printed || 1 > Printed
        {
            var packet_bundleInfos = packet_BundleInfoRepository.GetAll();
            var list = dispatchRepository.GetAll();

            var _result = (from dispatch in dispatchRepository.GetAll()
                           where (wheightType == 0 && !packet_bundleInfos.Any(a => a.dispatch_mId == dispatch.Id))       // multiple cases
                              || (wheightType == 1 && (!dispatch.Wheight_isVerified && packet_bundleInfos.Any(a => a.dispatch_mId == dispatch.Id)))
                              || (wheightType == 2 && (dispatch.Wheight_isVerified && packet_bundleInfos.Any(a => a.dispatch_mId == dispatch.Id)))
                              && dispatch.Packet_Consignment == null && dispatch.UpdationDate >= date
                           select new
                           {
                               dispatch
                           }).ToList()
                          .Select(x => new
                          {
                              Id = x.dispatch.Id,
                              order_challnId = x.dispatch.Dispatch_Master.Order == null ? x.dispatch.Dispatch_Master.ChallanId : x.dispatch.Dispatch_Master.OrderId,
                              packetNumber = x.dispatch.Packet_Id,
                              bookWheight = orderViewModel.parse(x.dispatch.Dispatch_Master, packet_bundleInfos),
                              bundleWheight = orderViewModel.parse(x.dispatch.Dispatch_Master.Id, packet_bundleInfos),
                              packet_bundleList = orderViewModel.parseA(x.dispatch.Dispatch_Master.Id, packet_bundleInfos),
                              createBy = x.dispatch.UpdatedBy,
                              createDate = x.dispatch.UpdationDate,
                              order_isOnine = x.dispatch.Dispatch_Master.Order != null ? true : false
                          });

            return Ok(new { result = _result });
        }

        [HttpPost]
        public IHttpActionResult add_packetWheight(bundleWheight_ViewModel model)
        {
            var dispatch = dispatchRepository.FindById(model.dispatch_mId);
            if (dispatch == null)
                return Ok(new { result = "notfound" });

            using (var transaction = packet_BundleInfoRepository.BeginTransaction())
            {
                try
                {
                    foreach (var entity in model.wheightModel)
                    {
                        packet_BundleInfoRepository.Create(bundleWheight_ViewModel.parse(model.dispatch_mId, entity), false);
                    }

                    packet_BundleInfoRepository.Save();

                    dispatchLogsRepository.Create(new DispatchLogs()
                    {
                        DispatchId = dispatch.Id,
                        Action = "Wheight is added, not verified",
                        Action_Date = DateTime.Now,
                        Status = true
                    });
                    transaction.Commit();       // it must be there if want to save record :)

                    return Ok(new { result = "ok" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);      // to log this error & go to global error handler
                    return InternalServerError(ex);
                }
            }
        }

        [HttpPut]
        public IHttpActionResult verify_packetWheight(int dispatchId)
        {
            var packetInfo = dispatchRepository.FindById(dispatchId);

            if (packetInfo == null)
                return Ok(new { result = "notfound" });

            packetInfo.Wheight_isVerified = true;
            dispatchRepository.Update(packetInfo);

            dispatchLogsRepository.Create(new DispatchLogs()
            {
                DispatchId = packetInfo.Id,
                Action = "Wheight is verified",
                Action_Date = DateTime.Now,
                Status = true
            });

            return Ok(new { result = "success" });
        }


        [HttpGet]
        public IHttpActionResult get_packages()
        {
            var list = package_MasterRepository.GetAll().Select(x => new
            {
                x.Id,
                x.Name,
                x.wheight
            });

            return Ok(new { result = list });
        }

        //  ******************************  Dispatch consignment  ******************************

        [HttpGet]
        public IHttpActionResult get_wheightedOrder(DateTime date, int consignmentType)
        {
            var list = dispatchRepository.GetAll();
            var packet_bundleInfos = packet_BundleInfoRepository.GetAll();

            var _result = (from dispatch in dispatchRepository.GetAll()
                          where (consignmentType == 0 ? dispatch.Packet_Consignment == null : dispatch.Packet_Consignment != null)      // a single condison inside braket
                          && packet_bundleInfos.Any(a => a.dispatch_mId == dispatch.Id)
                          && dispatch.Wheight_isVerified
                          && dispatch.UpdationDate >= date
                          select dispatch)
                          .AsEnumerable()
                          .Select(dispatch => new
                          {
                              Id = dispatch.Id,
                              packetNumber = dispatch.Packet_Id,
                              packetConsignmentNo = dispatch.Packet_Consignment,
                              courierName = dispatch.CourierMode == null
                                          ? string.Empty
                                          : string.Format("{0} - {1}",                                            
                                            dispatch.CourierMode.Courier.Courier_Name,
                                            dispatch.CourierMode.Courier_Mode),
                              createBy = dispatch.UpdatedBy,
                              createDate = dispatch.UpdationDate
                          });

            return Ok(new { result = _result });
        }

        [HttpPut]
        public IHttpActionResult add_consignment(int dispatchId, string consignmentNo, int courierMode)
        {
            if (dispatchRepository.GetByConsignment(consignmentNo, dispatchId))
                return Ok(new { result = "exist" });

            var packetInfo = dispatchRepository.FindById(dispatchId);

            if (packetInfo == null)
                return Ok(new { result = "notfound" });

            using (var transaction = dispatchRepository.BeginTransaction())
            {
                try
                {
                    packetInfo.Packet_Consignment = consignmentNo;
                    packetInfo.CourierModeId = courierMode;
                    packetInfo.Dispatch_Date = dispatchRepository.get_DateTime();
                    packetInfo.Order_StatusType = orderStatusType.Shipped;

                    dispatchRepository.Update(packetInfo);
                    transaction.Commit();       // it must be there if want to save record :)

                    dispatchLogsRepository.Create(new DispatchLogs()
                    {
                        DispatchId = packetInfo.Id,
                        Action = "Packet is Disaptched",
                        Action_Date = DateTime.Now,
                        Status = true
                    });

                    return Ok(new { result = "success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);      // to log this error & go to global error handler
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult get_couriers()
        {
            var list = courierRepository.GetAll().Select(x => new
            {
                x.Id,
                x.Courier_Name
            });

            return Ok(new { result = list });
        }

        [HttpGet]
        public IHttpActionResult get_orderStatusList()
        {
            var res = Enum.GetValues(typeof(orderStatusType))
                          .Cast<orderStatusType>()
                          .Select(v => new
                          {
                              Id = v.GetHashCode(),
                              Name = v.ToString()
                          }).ToList();

            return Ok(new { result = res });
        }

        [HttpGet]
        public IHttpActionResult get_courierMode_by_courierId(int courierId)
        {
            var list = courierModeRepository.GetByCourierId(courierId).Select(x => new
            {
                x.Id,
                x.Courier_Mode
            });

            return Ok(new { result = list });
        }

        // ******************************  Track Order  ******************************

        [HttpGet]
        public IHttpActionResult get_orderStatus_Reasons()
        {
            var list = orderStatusReasonRepository.GetAll()
                      .Select(x => new
                      {
                          x.Id,
                          x.Reason
                      });

            return Ok(new { result = list });
        }


        [HttpGet]
        public IHttpActionResult get_orderTrackList(DateTime date)
        {
            var list = dispatchRepository
                .FindBy(x => x.Packet_Consignment != null && x.UpdationDate >= date)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    OrderNumber = x.Dispatch_Master.Order == null ? (x.Dispatch_Master.OtherItemId != null ? x.Dispatch_Master.Dispatch_otherItem_Master.Id.ToString() : x.Dispatch_Master.stockMasters.ChallanNo) : x.Dispatch_Master.Order.OrderNumber,
                    order_Source = x.Dispatch_Master.DispatchType.ToString(),
                    total_Amount = x.Dispatch_Master.Order == null ? (x.Dispatch_Master.OtherItemId != null ? 0 : Inventory_create_ViewModel.getStock_TotalPrice(x.Dispatch_Master.stockMasters)) : (x.Dispatch_Master.Order.Total_Shipping_Amount + x.Dispatch_Master.Order.Total_Shipping_Charges),
                    x.Packet_Id,
                    x.Packet_Consignment,
                    Courier_Name = string.Format("{0} - {1}",
                                   x.CourierMode.Courier.Courier_Name,
                                   x.CourierMode.Courier_Mode),
                    x.Dispatch_Date,
                    remarks = x.Track_Remarks,
                    dispatch_Status = x.Order_StatusType,
                    trackLink = x.CourierMode.Courier.Id == 4           // create URL for for fedex 
                              ? x.CourierMode.Courier.Courier_Link + "?action=track&trackingnumber=" + x.Packet_Consignment + "&cntry_code=us"
                              : x.CourierMode.Courier.Courier_Link,
                    orderHistory = dispatchLogsRepository.FindBy(a => a.DispatchId == x.Id) != null
                                 ? dispatchLogsRepository.FindBy(a => a.DispatchId == x.Id)
                                  .Select(y => new
                                  {
                                      y.Action,
                                      y.Action_Date,
                                  })
                                 : null
                });

            return Ok(new { result = list });
        }

        [HttpGet]
        public IHttpActionResult ResendOrder(long dispatchId)
        {
            using (var transaction = dispatch_MasterRepository.BeginTransaction())
            {
                try
                {
                    var packetInfo = dispatchRepository.FindById(dispatchId);
                    packetInfo.Order_StatusType = orderStatusType.Settled;

                    var packetMaster = packetInfo.Dispatch_Master;

                    var master = dispatch_MasterRepository.Create(orderViewModel.parse(packetMaster));

                    dispatchRepository.Create(new Dispatch()
                    {
                        Id = master.Id,
                        Packet_Id = dispatchRepository.get_Packet_Number(master.Id, packetMaster.DispatchType),
                        Invoice_No = dispatchRepository.get_Invoice_Number(master.Id),
                        Order_StatusType = orderStatusType.Pending,
                        RefrenceId = packetMaster.Id,
                        Status = true
                    });


                    dispatchLogsRepository.Create(new DispatchLogs()
                    {
                        DispatchId = packetInfo.Id,
                        Action = "Packet Resend",
                        Action_Date = DateTime.Now,
                        Status = true
                    });

                    transaction.Commit();
                    return Ok(new { result = "ok" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult changeOrder_TrackStatus(orderStatusType statusId, long dispatchId, string remarks)
        {
            var packetInfo = dispatchRepository.FindById(dispatchId);

            if (packetInfo == null)
                return Ok(new { result = "notfound" });

            packetInfo.Order_StatusType = statusId;
            packetInfo.Track_Remarks = remarks;

            dispatchLogsRepository.Create(new DispatchLogs()
            {
                DispatchId = packetInfo.Id,
                Action = statusId.ToString(),
                Action_Date = DateTime.Now,
                Status = true
            });

            dispatchRepository.Update(packetInfo);
            return Ok(new { result = "ok" });
        }
        
        [HttpPost]
        public IHttpActionResult search_dispatch(search_dispatchViewModel model)
        {
            var _result = (from dispatch in dispatch_MasterRepository.GetAll()
                           join order in orderRepository.GetAll()         // for Online Orders
                           on dispatch.OrderId equals order.Id
                           into orderList
                           from order in orderList.DefaultIfEmpty()       // to create left join with dispatch Table
                           join stock in stockMasterRepository.GetAll()   // for challan items
                           on dispatch.ChallanId equals stock.Id
                           into stockList
                           from stock in stockList.DefaultIfEmpty()
                           join otherItem_dispatch in dispatch_otherItem_addressRepository.GetAll()    // for other dispatch items
                           on dispatch.OtherItemId equals otherItem_dispatch.Id
                           into otherItem_dispatchList
                           from otherItem in otherItem_dispatchList.DefaultIfEmpty()
                               //where dispatch.DispatchInfo.CreationDate >= date
                           select new
                           {
                               order,   // use these 3 values to check/debug code :)
                               stock,
                               dispatch,
                               otherItem
                           });

            if (model.srcId != null)
                _result = _result.Where(a => a.dispatch.DispatchType == model.srcId);

            if (model.pcktSts != null)
                _result = _result.Where(a => a.dispatch.DispatchInfo.Order_StatusType == model.pcktSts);

            if (!string.IsNullOrEmpty(model.packetNo))
                _result = _result.Where(a => a.dispatch.DispatchInfo.Packet_Id == model.packetNo);

            if (!string.IsNullOrEmpty(model.invoiceNo))
                _result = _result.Where(a => a.dispatch.DispatchInfo.Invoice_No == model.invoiceNo);

            if (model.pcktType.HasValue)
            {
            var packet_bundleInfos = packet_BundleInfoRepository.GetAll();

                if (model.pcktType == 1)
                    _result = _result.Where(a => !packet_bundleInfos.Any(z => a.dispatch.Id == z.dispatch_mId));
                else if (model.pcktType == 2)
                    _result = _result.Where(a => packet_bundleInfos.Any(z => a.dispatch.Id == z.dispatch_mId)
                                              && !a.dispatch.DispatchInfo.Wheight_isVerified
                                              && a.dispatch.DispatchInfo.Packet_Consignment == null);
                else if (model.pcktType == 3)
                    _result = _result.Where(a => packet_bundleInfos.Any(z => a.dispatch.Id == z.dispatch_mId)
                                              && a.dispatch.DispatchInfo.Wheight_isVerified
                                              && a.dispatch.DispatchInfo.Packet_Consignment == null);
                else if (model.pcktType == 4)
                    _result = _result.Where(a => a.dispatch.DispatchInfo.Packet_Consignment != null);
            }

            if (model.courierId != 0)
            {
                var courier_modeIds = courierModeRepository.FindBy(a => a.CourierId == model.courierId).Select(x => x.Id).ToList();
                _result = _result.Where(a => courier_modeIds.Contains((long)a.dispatch.DispatchInfo.CourierModeId));
            }

            if (model.courier_modeId != 0)
            {
                _result = _result.Where(a => a.dispatch.DispatchInfo.CourierModeId == model.courier_modeId);
            }

             var Inventory_Src = inventorySourceDetailRepository.GetAll();
             var Event_SrcModel = eventManagementRepository.GetAll();

            var newRes = _result.ToList().Select(x => new
            {
                Id = x.order == null ? (x.otherItem != null ? x.otherItem.Id : x.dispatch.ChallanId) : x.dispatch.OrderId,
                OrderNumber = x.order == null ? (x.otherItem != null ? x.otherItem.Id.ToString() : x.stock.ChallanNo) : x.order.OrderNumber,
                orderAmount = x.order == null ? (x.otherItem != null ? 0 : Inventory_create_ViewModel.getStock_TotalPrice(x.stock)) : (x.order.Total_Shipping_Amount + x.order.Total_Shipping_Charges),
                OrderDate = x.order == null ? (x.otherItem != null ? x.otherItem.Dispatch_otherItem_Master.CreationDate : x.stock.Stocks.FirstOrDefault().CreationDate) : x.order.OrderDate,
                custmer_name = x.order == null
                             ? (x.otherItem != null ? x.otherItem.Name : (x.stock.SourceDetail == null
                             ? eventManagementRepository.FindBy(y => y.SchId == x.stock.SourceInfo_Id).FirstOrDefault().School.SchName
                             : x.stock.SourceDetail.Contact_Person_Name))
                             : x.order.UserShippingAddress.Username,
                order_source = x.dispatch.DispatchType.ToString(),
                UsrInfoModel = x.order == null 
                             ? (x.stock == null ? searchModel.parse(x.otherItem) : searchModel.parse(x.stock, Inventory_Src, Event_SrcModel))
                             : searchModel.parse(x.order.UserShippingAddress),
                packetNumber = x.dispatch.DispatchInfo.Packet_Id,
                orderType = x.dispatch.DispatchType,
                invoiceNo = x.dispatch.DispatchInfo.Invoice_No,
                orderStatus = x.dispatch.DispatchInfo.Order_StatusType.ToString()
            });

            if (!string.IsNullOrEmpty(model.srchTxt))
                if (model.is_likeSrch)
                    newRes = newRes.Where(a => a.custmer_name.ToLower().Contains(model.srchTxt)
                                            || a.UsrInfoModel.Address.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.PinCode.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.EmailId.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.PhoneNo.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.countryName.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.cityName.ToLower().Contains(model.srchTxt.ToLower())
                                            || a.UsrInfoModel.stateName.ToLower().Contains(model.srchTxt.ToLower()));
                else
                    newRes = newRes.Where(a => a.custmer_name.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.Address.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.PinCode.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.EmailId.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.PhoneNo.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.countryName.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.cityName.ToLower() == model.srchTxt.ToLower()
                                            || a.UsrInfoModel.stateName.ToLower() == model.srchTxt.ToLower());

            if (model.fromDate != null)
                newRes = newRes.Where(a => a.OrderDate >= model.fromDate);

            if (model.toDate != null)
                newRes = newRes.Where(a => a.OrderDate <= model.toDate);

            return Ok(new { result = newRes });
        }



        internal string getbase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Convert Image to byte[]
                image.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();

                //Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public DispatchController(
            IUserShippingAddressRepository _userShippingAddressRepository,
            IOrderDetailRepository _orderDetailRepository,
            IOrderRepository _orderRepository,
            IDispatch_MasterRepository _dispatch_MasterRepository,
            IDispatchRepository _dispatchRepository,
            IErrorLogsRepository _errorLogsRepository,
            ICourierRepository _courierRepository,
            ICourierModeRepository _courierModeRepository,
            IStock_MasterRepository _stockMasterRepository,
            IinventorySourceDetailRepository _inventorySourceDetailRepository,
            IPackage_MasterRepository _package_MasterRepository,
            IPacket_BundleInfoRepository _packet_BundleInfoRepository,
            IEventManagementRepository _eventManagementRepository,
            IDispatch_otherItem_addressRepository _dispatch_otherItem_addressRepository,
            ICityRepository _cityRepository,
            IDispatchLogsRepository _dispatchLogsRepository,
            IOrderStatusReasonRepository _orderStatusReasonRepository)
        {
            userShippingAddressRepository = _userShippingAddressRepository;
            orderDetailRepository = _orderDetailRepository;
            orderRepository = _orderRepository;
            dispatch_MasterRepository = _dispatch_MasterRepository;
            dispatchRepository = _dispatchRepository;
            errorLogsRepository = _errorLogsRepository;
            courierRepository = _courierRepository;
            courierModeRepository = _courierModeRepository;
            stockMasterRepository = _stockMasterRepository;
            inventorySourceDetailRepository = _inventorySourceDetailRepository;
            package_MasterRepository = _package_MasterRepository;
            packet_BundleInfoRepository = _packet_BundleInfoRepository;
            eventManagementRepository = _eventManagementRepository;
            dispatch_otherItem_addressRepository = _dispatch_otherItem_addressRepository;
            cityRepository = _cityRepository;
            dispatchLogsRepository = _dispatchLogsRepository;
            orderStatusReasonRepository = _orderStatusReasonRepository;
        }

    }
}
