using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel;
using SilverzoneERP.Entities.ViewModel.Site;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Inventory
{
    public class StockController : ApiController
    {
        IinventorySourceRepository inventorySourceRepository { get; set; }
        IPurchaseOrder_MasterRepository purchaseOrder_MasterRepository { get; set; }
        IPurchaseOrderRepository purchaseOrderRepository { get; set; }
        IinventorySourceDetailRepository inventorySourceDetailRepository { get; set; }
        IStockRepository stockRepository { get; set; }
        IStock_MasterRepository stock_MasterRepository { get; set; }
        IErrorLogsRepository errorLogsRepository { get; set; }
        IBookRepository bookRepository { get; set; }
        IUserRepository userRepository { get; set; }
        IDealerBookDiscountRepository dealerBookDiscountRepository { get; set; }
        IDealerSceondaryAddressRepository dealerSceondaryAddressRepository { get; set; }
        ICityRepository cityRepository { get; set; }
        IDispatchRepository dispatchRepository { get; set; }
        IDispatch_MasterRepository dispatch_MasterRepository { get; set; }
        ISchoolRepository schoolRepository { get; set; }
        IEventManagementRepository eventManagementRepository { get; set; }
        ICounterCustomerRepository counterCustomerRepository { get; set; }

        #region *****************  For Inventory  *******************

        [HttpGet]
        public IHttpActionResult get_inventorySource()
        {
            var sources = inventorySourceRepository
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.SourceName
                });

            return Ok(new { result = sources });
        }

        [HttpGet]
        public IHttpActionResult get_bookCategory(string bookISBN)
        {
            var bookModel = bookRepository
                          .FindBy(x => x.ISBN.Equals(bookISBN) && x.Status == true)
                          .Select(x => new
                          {
                              bookId = x.Id,
                              classId = x.ItemTitle_Master.ClassId,
                              subjectId = x.ItemTitle_Master.SubjectId
                          })
                           .SingleOrDefault();

            return Ok(new { result = bookModel });
        }

        [HttpGet]
        public IHttpActionResult get_bookISBN(long bookId)
        {
            return Ok(new
            {
                result = bookRepository
                       .GetById(bookId).ISBN
            });
        }

        [HttpPost]
        public IHttpActionResult getAll_pendingPO(search_pendingPO_ViewModel model)
        {
            IQueryable<PurchaseOrder_Master> POModel = null;
            if (model.type == inventoryType.Inward)
            {
                POModel = purchaseOrder_MasterRepository.findRecordTo(model.srcId, model.SourceInfo_Id);
            }
            else if (model.type == inventoryType.Outward)
            {
                if (model.srcId == orderSourceType.School)
                {
                    var SourceInfo_Ids = eventManagementRepository.FindBy(x => x.SchId == model.SourceInfo_Id
                                                        && x.Status == true)
                                              .Select(y => y.Id);

                    POModel = purchaseOrder_MasterRepository.findRecordFrom(model.srcId, SourceInfo_Ids);
                }
                else
                    POModel = purchaseOrder_MasterRepository.findRecordFrom(model.srcId, model.SourceInfo_Id);
            }


            if (POModel == null)
                return Ok(new { result = string.Empty });

            var stockList = stockRepository.GetAll();

            dynamic res = null;
            if (model.type == inventoryType.Inward)
                res = PurchaseOrder_ViewModel.parse(POModel, stockList);
            else if (model.type == inventoryType.Outward)
            {
                var stock_in = stockRepository.getRecord(stock_MasterRepository.getRecord(inventoryType.Inward).Select(x => x.Id));
                var stock_out = stockRepository.getRecord(stock_MasterRepository.getRecord(inventoryType.Outward).Select(x => x.Id));

                res = Inventory_outward_ViewModel.parse(POModel, stock_in, stock_out, stockList);
            }

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult create_inventory(Inventory_create_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = stock_MasterRepository.BeginTransaction())
                {
                    try
                    {
                        PurchaseOrder_Master PO_master = null;
                        if (model.InventoryType == inventoryType.Inward)
                            PO_master = purchaseOrder_MasterRepository.getRecordTo(Convert.ToInt64(model.PO_Number), model.SourceInfo_Id);
                        if (model.InventoryType == inventoryType.Outward)
                        {
                            if (model.srcId == 6)
                            {
                                var SourceInfo_Ids = eventManagementRepository.FindBy(x => x.SchId == model.SourceInfo_Id
                                                                    && x.Status == true)
                                                          .Select(y => y.Id);

                                PO_master = purchaseOrder_MasterRepository.getRecordFrom(Convert.ToInt64(model.PO_Number), SourceInfo_Ids);
                            }
                            else
                                PO_master = purchaseOrder_MasterRepository.getRecordFrom(Convert.ToInt64(model.PO_Number), model.SourceInfo_Id);
                        }


                        if (PO_master == null)
                            return Ok(new { result = "notfound" });     // PO master not found               

                        var PO = PO_master.PurchaseOrders
                                     .SingleOrDefault(x => x.BookId == model.BookId
                                                        && x.PO_mId == PO_master.Id);
                        if (PO == null)
                            return Ok(new { result = "book_notfound" });     // Book is not fount against a PO

                        if (model.Quantity > PO.Quantity && model.chekQty)
                            return Ok(new { result = "maxQty" });     // quantity error

                        //********************* return on above conditions before create of data in stock Master  *****

                        if (model.stockId == 0)         // create stock 
                        {
                            if (model.stock_mId == 0)   // for first time create 
                            {
                                var stockM = stock_MasterRepository.Create(Inventory_create_ViewModel.ParseM(model));
                                model.stock_mId = stockM.Id;

                                if (model.InventoryType == inventoryType.Outward)
                                {
                                    stockM.ChallanNo = string.Format("{0}", stockM.Id + 1000);
                                    stock_MasterRepository.Update(stockM);
                                    model.ChallanNo = stockM.ChallanNo;
                                    model.ChallanDate = stockM.ChallanDate;
                                }
                            }

                            var stockModel = stockRepository.getRecord(PO_master.Id, model.stock_mId, model.BookId);
                            if (stockModel != null)
                                return Ok(new
                                {
                                    result = "exist",
                                    entity = Inventory_create_ViewModel.getInventory_details(stockModel)
                                });     // record already exist

                            var _model = Inventory_create_ViewModel.Parse(model);
                            _model.Stock_mId = model.stock_mId;
                            _model.PO_Id = PO_master.Id;

                            stockRepository.Create(_model);

                            transaction.Commit();

                            if (model.InventoryType == inventoryType.Inward)
                                return Ok(new { result = "success", stock_mId = model.stock_mId });
                            else
                            {
                                var _challanInfo = new
                                {
                                    model.ChallanNo,
                                    model.ChallanDate
                                };
                                return Ok(new
                                {
                                    result = "success",
                                    stock_mId = model.stock_mId,
                                    challanInfo = _challanInfo
                                });
                            }
                        }
                        else    // update record
                        {
                            var entity = stockRepository.FindById(model.stockId);

                            stockRepository.Update(Inventory_create_ViewModel.Parse(entity, model));

                            transaction.Commit();

                            if (model.InventoryType == inventoryType.Inward)
                                return Ok(new { result = "success", stock_mId = model.stock_mId });
                            else
                            {
                                var _challanInfo = new
                                {
                                    entity.Stock_Master.ChallanNo,
                                    entity.Stock_Master.ChallanDate
                                };
                                return Ok(new
                                {
                                    result = "success",
                                    stock_mId = model.stock_mId,
                                    challanInfo = _challanInfo
                                });
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        errorLogsRepository.logError(ex);
                        transaction.Rollback();
                    }
                }
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult create_bulkinventory(Inventory_bulk_Create_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = stock_MasterRepository.BeginTransaction())
                {
                    try
                    {                                             
                        var stockM = stock_MasterRepository.Create(Inventory_bulk_Create_ViewModel.ParseM(model));
                        model.stock_mId = stockM.Id;

                        if (model.InventoryType == inventoryType.Outward)
                        {
                            stockM.ChallanNo = string.Format("{0}", stockM.Id + 1000);
                            stock_MasterRepository.Update(stockM);
                            model.ChallanNo = stockM.ChallanNo;
                            model.ChallanDate = stockM.ChallanDate;
                        }

                        foreach (var _model in model.stockList)
                        {
                            PurchaseOrder_Master PO_master = null;
                            if (model.srcId == 6)
                            {
                                var SourceInfo_Ids = eventManagementRepository.FindBy(x => x.SchId == model.SourceInfo_Id
                                                                    && x.Status == true)
                                                          .Select(y => y.Id);

                                PO_master = purchaseOrder_MasterRepository.getRecordFrom(Convert.ToInt64(_model.PO_Number), SourceInfo_Ids);
                            }
                            else
                                PO_master = purchaseOrder_MasterRepository.getRecordFrom(Convert.ToInt64(_model.PO_Number), model.SourceInfo_Id);

                            var stockModel = stockRepository.getRecord(PO_master.Id, model.stock_mId, _model.BookId);
                            if (stockModel != null)
                                return Ok(new
                                {
                                    result = "exist",
                                    entity = Inventory_create_ViewModel.getInventory_details(stockModel)
                                });     // record already exist

                            var _stockModel = Inventory_bulk_Create_ViewModel.Parse(_model);
                            _stockModel.Stock_mId = model.stock_mId;
                            _stockModel.PO_Id = PO_master.Id;

                            stockRepository.Create(_stockModel, false);
                        }

                        stockRepository.Save();
                        transaction.Commit();

                        var _challanInfo = new
                        {
                            model.ChallanNo,
                            model.ChallanDate
                        };
                        return Ok(new
                        {
                            result = "success",
                            stock_mId = model.stock_mId,
                            challanInfo = _challanInfo
                        });
                    }
                    catch (Exception ex)
                    {
                        errorLogsRepository.logError(ex);
                        transaction.Rollback();
                    }
                }
            }
            return Ok(new { result = "error" });
        }


        [HttpGet]
        public IHttpActionResult get_dealerAdressList(long SourceInfo_Id)
        {
            var inventry_src = inventorySourceDetailRepository.FindById(SourceInfo_Id);

            return Ok(new { result = PurchaseOrder_ViewModel.parse(inventry_src) });
        }


        [HttpPost]
        public IHttpActionResult delete_inventory(long Id)
        {
            // always get 1 record
            var model = stockRepository.FindById(Id);

            if (model == null)
                return Ok(new { result = "notFound" });

            model.Status = false;
            stockRepository.Update(model);
            return Ok(new { result = "success" });
        }

        [HttpGet]
        public IHttpActionResult get_Inventory_byStockId(long stockId)
        {
            var stockList = stock_MasterRepository
                            .FindById(stockId);

            var Inventory_Src = inventorySourceDetailRepository.GetAll();
            var Event_SrcModel = eventManagementRepository.GetAll();

            return Ok(new
            {
                result = Inventory_create_ViewModel.Parse(stockList, Inventory_Src, Event_SrcModel)
            });
        }

        [HttpGet]
        public IHttpActionResult get_Inventory_bychallanNo(string challanNo)
        {
            var stockMaster = stock_MasterRepository.getBy_challanNo(challanNo);

            if (stockMaster == null)
                return Ok(new { result = "notfound" });

            var dispatch_master = dispatch_MasterRepository.GetBychallanId(stockMaster.Id);

            var Inventory_Src = inventorySourceDetailRepository.GetAll();
            var Event_SrcModel = eventManagementRepository.GetAll();

            return Ok(new
            {
                result = Inventory_create_ViewModel.Parse(stockMaster, Inventory_Src, Event_SrcModel),
                dispatchInfo = dispatch_master == null
                              ? null
                              : new
                              {
                                  dispatch_mId = dispatch_master.Id,
                                  dispatch_master.DispatchInfo.Invoice_No
                              }
            });
        }

        [HttpPost]
        public void send_stockEmail(emailViewModel model)
        {
            long userId = Convert.ToInt64(User.Identity.Name);
            string emailId = userRepository.GetById(userId).EmailID;

            //purchaseOrderRepository.sendEmail_PO_Confirmation(model.HtmlTemplate, model.emailId);
            stockRepository.sendEmail_Stock_Confirmation(model.HtmlTemplate, emailId);
        }

        [HttpGet]
        public IHttpActionResult get_allstock()
        {
            var stockIn = stockRepository.filerBy_stockMid(stock_MasterRepository.getRecord(inventoryType.Inward).Select(x => x.Id));
            var stockOut = stockRepository.filerBy_stockMid(stock_MasterRepository.getRecord(inventoryType.Outward).Select(x => x.Id));

            var books = bookRepository.GetAll();

            var aa = searchPO_ViewModel.parse(stockIn, stockOut, books);
            return Ok(new { result = aa });
        }

        [HttpGet]
        public IHttpActionResult searchStock_byBookid(long bookId, inventoryType type)
        {
            var _result = searchPO_ViewModel
                          .parseA(stockRepository
                                  .filerBy_bookId(bookId, type));

            return Ok(new { result = _result });
        }

        [HttpPost]
        public IHttpActionResult verify_challan(challanVerify_ViewModel model)
        {
            var stockmaster = stock_MasterRepository.getBy_challanNo(model.ChallanNumber.ToString());

            if (stockmaster == null)
                return Ok(new { Status = "notFound" });

            if (stockmaster.isVerified)
                return Ok(new { Status = "verified" });

            var _challanInfo = stockmaster.Stocks
                .GroupBy(a => a.BookId)
                .Select(x => new challanInfo_ViewModel
                {
                    BookId = x.Key,
                    BookName = searchPO_ViewModel.get_BookName(x.FirstOrDefault().Book),
                    challanQty = x.Sum(b => b.Quantity)
                });

            var res = challanVerify_ViewModel.Parse(model.ChallanList, _challanInfo);
            var stock_isVerified = res.GetType().GetProperty("stock_isVerified").GetValue(res, null);

            if (stock_isVerified)      // to read value of propeties from dynamic variable using reflection
            {
                stockmaster.isVerified = true;
                stock_MasterRepository.Update(stockmaster);
            }

            return Ok(new
            {
                result = res.GetType().GetProperty("result").GetValue(res, null),       // result contain qty status is matched or not 
                Status = stock_isVerified,
                challanInfo = _challanInfo
            });
        }

        [HttpGet]
        public IHttpActionResult getPO_bypoNo(orderSourceType srcId, long PoNo)
        {
            var POModel = purchaseOrder_MasterRepository.findBypoNo(srcId, PoNo);

            if (!POModel.Any())
                return Ok(new
                {
                    result = string.Empty
                });

            var stockList = stockRepository.GetAll();

            var stock_in = stockRepository.getRecord(stock_MasterRepository.getRecord(inventoryType.Inward).Select(x => x.Id));
            var stock_out = stockRepository.getRecord(stock_MasterRepository.getRecord(inventoryType.Outward).Select(x => x.Id));

            return Ok(new
            {
                result = Inventory_outward_ViewModel.parse(POModel, stock_in, stock_out, stockList)
            });
        }


        #endregion

        #region ****************************   Inventory Source Details  *************************
        [HttpPost]
        public IHttpActionResult create_inventorySource_Detail(Inventory_source_ViewModel model)
        {
            if ((ModelState.IsValid))
            {
                if (inventorySourceDetailRepository.isRecordExist(model.name,
                                                                  model.panNo,
                                                                  model.tanNo,
                                                                  model.SourceId))
                    return Ok(new { result = "exist" });


                inventorySourceDetailRepository
                    .Create(Inventory_source_ViewModel.parse(model));

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult update_inventorySource_Detail(long Id, Inventory_source_ViewModel model)
        {
            if ((ModelState.IsValid))
            {
                if (inventorySourceDetailRepository.isRecordExist(Id,
                                                                  model.name,
                                                                  model.panNo,
                                                                  model.tanNo,
                                                                  model.SourceId))
                    return Ok(new { result = "exist" });

                var _model = inventorySourceDetailRepository.FindById(Id);

                if (_model == null)
                    return Ok(new { result = "notFound" });

                inventorySourceDetailRepository.Update(Inventory_source_ViewModel.parse(model, _model));

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult create_dealer_inventorySource_Detail(Dealer_inventory_source_ViewModel model)
        {
            if ((ModelState.IsValid))
            {
                using (var transaction = purchaseOrder_MasterRepository.BeginTransaction())
                {
                    try
                    {
                        if (inventorySourceDetailRepository.isRecordExist(model.name,
                                                                  model.panNo,
                                                                  model.tanNo,
                                                                  model.SourceId))
                            return Ok(new { result = "exist" });


                        var srcModel = Inventory_source_ViewModel.parse(model);
                        srcModel.City_Id = model.cityId;
                        srcModel.PinCode = model.pincode;

                        var sourceinfo = inventorySourceDetailRepository
                                         .Create(srcModel);

                        foreach (var discount in model.delaerBookDiscounts)
                        {
                            dealerBookDiscountRepository.Create(new DealerBookDiscount()
                            {
                                SourceDetailId = sourceinfo.Id,
                                BookCategoryId = discount.categoryId,
                                DiscountPercentage = discount.amount,
                                Status = true
                            }, false);
                        }

                        int i = 0; // for index
                        foreach (string adress in model.addressList)
                        {
                            dealerSceondaryAddressRepository.Create(new DealerSecondaryAddress()
                            {
                                SourceDetailId = sourceinfo.Id,
                                Address = adress,
                                isDefault = model.defaultAdres_radio != null && i == model.defaultAdres_radio ? true : false,
                                Status = true
                            }, false);
                            i++;
                        }

                        inventorySourceDetailRepository.Save();
                        transaction.Commit();
                        return Ok(new { result = "success" });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult update_dealer_inventorySource_Detail(long Id, Dealer_inventory_source_ViewModel model)
        {
            if ((ModelState.IsValid))
            {
                using (var transaction = purchaseOrder_MasterRepository.BeginTransaction())
                {
                    try
                    {
                        if (inventorySourceDetailRepository.isRecordExist(Id,
                                                                  model.name,
                                                                  model.panNo,
                                                                  model.tanNo,
                                                                  model.SourceId))
                            return Ok(new { result = "exist" });

                        var _model = inventorySourceDetailRepository.FindById(Id);

                        if (_model == null)
                            return Ok(new { result = "notFound" });


                        _model.City_Id = model.cityId;
                        _model.PinCode = model.pincode;

                        inventorySourceDetailRepository
                                 .Update(Inventory_source_ViewModel.parse(model, _model));

                        // delete exist records 
                        dealerBookDiscountRepository.
                            DeleteWhere(dealerBookDiscountRepository.FindBy(x => x.SourceDetailId == _model.Id));

                        foreach (var discount in model.delaerBookDiscounts)
                        {
                            dealerBookDiscountRepository.Create(new DealerBookDiscount()
                            {
                                SourceDetailId = Id,
                                BookCategoryId = discount.categoryId,
                                DiscountPercentage = discount.amount
                            }, false);
                        }

                        dealerSceondaryAddressRepository.
                            DeleteWhere(dealerSceondaryAddressRepository.FindBy(x => x.SourceDetailId == _model.Id));
                        int i = 0; // for index
                        foreach (string adress in model.addressList)
                        {
                            dealerSceondaryAddressRepository.Create(new DealerSecondaryAddress()
                            {
                                SourceDetailId = Id,
                                Address = adress,
                                isDefault = model.defaultAdres_radio != null && i == model.defaultAdres_radio ? true : false,
                                Status = true
                            }, false);
                            i++;
                        }

                        inventorySourceDetailRepository.Save();
                        transaction.Commit();
                        return Ok(new { result = "success" });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return Ok(new { result = "error" });
        }


        [HttpGet]
        public IHttpActionResult get_inventorySource_Details(orderSourceType sourceId,
                                                             bool show_allInfo,
                                                             inventoryType type)
        {
            // for school
            if (sourceId == orderSourceType.School)
            {

                IQueryable<Entities.Models.School> res = null;
                if (type == inventoryType.Inward)
                    res = schoolRepository.FindBy(x => x.Status == true);
                else if (type == inventoryType.Outward)
                {
                    var eventIds = purchaseOrder_MasterRepository.getRecordFrom(sourceId);

                    res = eventManagementRepository
                         .FindBy(x => eventIds.Contains(x.Id))
                         .Select(z => z.School);
                }

                return Ok(new
                {
                    result = res.Select(school => new
                    {
                        Id = school.Id,
                        SourceName = school.SchName
                    })
                });
            }

            IQueryable<InventorySourceDetail> list = null;
            if (type == inventoryType.Inward)
                list = inventorySourceDetailRepository.FilerBySourceId(sourceId.GetHashCode());
            else
            {
                var src_infoIds = purchaseOrder_MasterRepository.getRecordFrom(sourceId);

                list = inventorySourceDetailRepository
                              .FindBy(x => src_infoIds.Contains(x.Id));
            }

            var cityList = cityRepository.GetAll();

            if (show_allInfo)
            {
                var _newList = Inventory_source_ViewModel.parse(list, cityList);
                return Ok(new
                {
                    result = _newList
                });
            }

            var newList = list
                .Where(x => x.Status == true)
                .Select(x => new
                {
                    x.Id,
                    x.SourceName
                });

            return Ok(new { result = newList });
        }

        [HttpPost]
        public IHttpActionResult change_inventorySource_Status(long Id, bool status)
        {
            var _model = inventorySourceDetailRepository.FindById(Id);

            if (_model == null)
                return Ok(new { result = "notFound" });

            _model.Status = status;
            inventorySourceDetailRepository.Update(_model);

            return Ok(new { result = "success" });
        }

        #endregion

        #region *********************** purchase Order   *********************************

        [HttpPost]
        public IHttpActionResult save_purchaseOrder(PurchaseOrder_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                // for 1st time data will insert
                if (model.PO_masterId == null && model.POId == 0)
                {
                    using (var transaction = purchaseOrder_MasterRepository.BeginTransaction())
                    {
                        try
                        {
                            var PO_master = purchaseOrder_MasterRepository.Create(PurchaseOrder_ViewModel.parseM(model));

                            PO_master.PO_Number = PO_master.Id + 1000;
                            purchaseOrder_MasterRepository.Update(PO_master);
                            model.PO_masterId = PO_master.Id;

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }

                var purchaseModel = model.POId == 0
                                  ? purchaseOrderRepository.getRecord(model.BookId, Convert.ToInt64(model.PO_masterId))
                                  : purchaseOrderRepository.getRecord(model.POId, model.BookId, Convert.ToInt64(model.PO_masterId));

                if (purchaseModel != null)
                    return Ok(new
                    {
                        result = "exist",
                        entity = PurchaseOrder_ViewModel.getPO_details(purchaseModel)
                    });

                if (model.POId == 0)
                {
                    purchaseOrderRepository
                        .Create(PurchaseOrder_ViewModel.parse(model));
                }
                else
                {
                    var _model = purchaseOrderRepository.FindById(model.POId);

                    purchaseOrderRepository.Update(PurchaseOrder_ViewModel.parse(_model, model));
                }

                return Ok(new { result = "success", PO_masterId = model.PO_masterId });
            }
            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult get_purchaseOrder_byId(long Id)
        {
            // always get 1 record
            var model = purchaseOrder_MasterRepository.FindById(Id);
            var source = inventorySourceDetailRepository.GetAll();

            return Ok(new { result = PurchaseOrder_ViewModel.parse(model, source) });
        }

        [HttpGet]
        public IHttpActionResult get_POby_poNumber(long poNumber)
        {
            // always get 1 record
            var model = purchaseOrder_MasterRepository.getBy_Ponumber(poNumber);

            if (model == null)
                return Ok(new { result = "notfound" });

            if (stockRepository.FindBy(x => x.PO_Id == model.Id && x.Status == true).Any())
                return Ok(new { result = "stock_exist" });

            var source = inventorySourceDetailRepository.GetAll();

            return Ok(new { result = PurchaseOrder_ViewModel.parse(model, source) });
        }

        [HttpPost]
        public IHttpActionResult delete_purchaseOrder_byId(long Id)
        {
            // always get 1 record
            var model = purchaseOrderRepository.FindById(Id);

            if (model == null)
                return Ok(new { result = "notFound" });

            model.Status = false;
            purchaseOrderRepository.Update(model);
            return Ok(new { result = "success" });
        }

        [HttpPost]
        public void sendEmail(emailViewModel model)
        {
            long userId = Convert.ToInt64(User.Identity.Name);
            string emailId = userRepository.GetById(userId).EmailID;

            //purchaseOrderRepository.sendEmail_PO_Confirmation(model.HtmlTemplate, model.emailId);
            purchaseOrderRepository.sendEmail_PO_Confirmation(model.HtmlTemplate, emailId);
        }

        #endregion

        #region *********************** purchase Order Sarch  *********************************

        [HttpPost]
        public IHttpActionResult search_pendingPO(searchPO_ViewModel model)
        {
            var PO_List = purchaseOrder_MasterRepository.GetAll();

            if (model.poNo != 0)
            {
                PO_List = purchaseOrder_MasterRepository.FilterBypoNo(PO_List, model.poNo);
                if (!PO_List.Any())
                    return Ok(new { result = PO_List });        // if
            }

            if (model.SourceId != 0)
            {
                var sourceInfoIds = inventorySourceDetailRepository.FilerBySourceId(model.SourceId).Select(x => x.Id);
                PO_List = purchaseOrder_MasterRepository.FilterBySource(PO_List, sourceInfoIds);
                if (!PO_List.Any())
                    return Ok(new { result = PO_List });
            }

            if (model.SourceInfo_Id != 0)
            {
                PO_List = purchaseOrder_MasterRepository.FilterBySource(PO_List, model.SourceInfo_Id);
                if (!PO_List.Any())
                    return Ok(new { result = PO_List });
            }

            if (model.from != DateTime.MinValue)
            {
                PO_List = purchaseOrder_MasterRepository.FilterByfromDate(PO_List, model.from);
                if (!PO_List.Any())
                    return Ok(new { result = PO_List });
            }

            if (model.to != DateTime.MinValue)
            {
                PO_List = purchaseOrder_MasterRepository.FilterBytoDate(PO_List, model.to);     // POlist = master records
                if (!PO_List.Any())
                    return Ok(new { result = PO_List });
            }

            var bookIds = bookRepository.GetBooksId(model.subjectId, model.classId, model.CategoryId);
            var poIds = PO_List.Select(x => x.Id);
            // adjusted PO
            var PO_details = purchaseOrderRepository.get_pendingPO(poIds, bookIds, model.poType);     // get PO details
            var stock_details = stockRepository.get_pendingPO(poIds, bookIds);     // get Stock details
            var source = inventorySourceDetailRepository.GetAll();

            return Ok(new { result = searchPO_ViewModel.parse(PO_List, PO_details, stock_details, source) });
        }

        [HttpGet]
        public IHttpActionResult search_pendingPO_details(long srcId, long bookId)
        {
            var result = (from PO_M in purchaseOrder_MasterRepository.GetAll().ToList()
                          join PO in purchaseOrderRepository.GetAll().ToList()
                          on PO_M.Id equals PO.PO_mId
                          where PO_M.srcTo == srcId
                             && PO.BookId == bookId
                          group new { PO, PO_M } by new { PO.BookId, PO_M.srcTo }
                          into newList
                          select new
                          {
                              srcName = inventorySourceDetailRepository.FindById(newList.FirstOrDefault().PO_M.srcTo).SourceName,
                              Book = PurchaseOrder_ViewModel.get_BookInfo(newList.FirstOrDefault().PO.Book),
                              POdetails = newList.Select(x => new
                              {
                                  x.PO.Id,
                                  x.PO.PO_mId,
                                  x.PO_M.PO_Number,
                                  x.PO_M.PO_Date,
                                  poQty = x.PO.Quantity,
                                  x.PO.is_Adjusted,
                                  stockQty = PurchaseOrder_ViewModel
                                             .getstock_totalQuantity(stockRepository.GetAll(), x.PO)
                              })
                          });

            return Ok(new { result = result });
        }

        [HttpPost]
        public IHttpActionResult adjust_pendingPO(long Id, string adjustRemarks)
        {
            var PO = purchaseOrderRepository.FindById(Id);

            if (PO == null)
                return Ok(new { result = "notfound" });

            PO.is_Adjusted = true;
            PO.Adjust_Remarks = adjustRemarks;

            purchaseOrderRepository.Update(PO);
            return Ok(new { result = "success" });
        }

        #endregion

        #region ****************** For Orders/Invoice  ****************************

        [HttpGet]
        public IHttpActionResult getCustomer_BooksPrice(long stockId)
        {
            var stock_Master = stock_MasterRepository
                                      .FindById(stockId);

            return Ok(new
            {
                result = orderViewModel.parse(stock_Master)
            });
        }

        [HttpPost]
        public IHttpActionResult create_Counterorder(CounterCustomer_ViewModel model)
        {
            if(counterCustomerRepository.FindBy(x => x.StockId == model.StockId).Any())
                return Ok(new { result = "exist" });

            var id = counterCustomerRepository.Create(CounterCustomer_ViewModel.parse(model)).Id;

            return Ok(new { result = "ok" });
        }

        [HttpPost]
        public IHttpActionResult create_order(orderViewModel model)
        {
            using (var transaction = dispatch_MasterRepository.BeginTransaction())
            {
                try
                {
                    // return new Id > but data still not save in DB bcoz of transaction :)
                    var master = dispatch_MasterRepository.Create(orderViewModel.parse(model));

                    var dispatch = dispatchRepository.Create(new Dispatch()
                    {
                        Id = master.Id,
                        Packet_Id = dispatchRepository.get_Packet_Number(master.Id, model.sourceType),
                        Invoice_No = dispatchRepository.get_Invoice_Number(master.Id),
                        Status = true
                    });

                    dynamic orderInfo = null;
                    var stock_Master = stock_MasterRepository
                                       .FindById(model.Id);

                    var Inventory_Src = inventorySourceDetailRepository.GetAll();
                    var Event_SrcModel = eventManagementRepository.GetAll();

                    if (model.sourceType != orderSourceType.Online)
                        orderInfo = Inventory_create_ViewModel
                                    .Parse(stock_Master, Inventory_Src, Event_SrcModel);


                    Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                    var barCodeImg = barcode.Draw(dispatch.Packet_Id, 50);


                    var _result = new
                    {
                        dispatchInfo = new
                        {
                            dispatch_mId = master.Id,
                            master.DispatchInfo.Invoice_No
                        },
                        packetInfo = new { dispatch.Invoice_No, master.Id, dispatch.Packet_Id, dispatch.CreationDate, imgBase = dispatchRepository.getbase64(barCodeImg) },     // new objects
                        orderInfo = orderInfo,
                        status = "success"
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
        public IHttpActionResult get_allInvoice(invoiceViewModel model)
        {
            var _dispatchResult = dispatchRepository
                                  .FilterByDate(model.from, model.to);

            if (!_dispatchResult.Any())
                return Ok(new { result = _dispatchResult });

            if (!string.IsNullOrEmpty(model.invoiceNo))
                _dispatchResult = dispatchRepository
                                  .FilterByInvoice(_dispatchResult, model.invoiceNo);       // final filteration get here 

            if (!_dispatchResult.Any())
                return Ok(new { result = _dispatchResult });

            var challanIds = _dispatchResult.Select(x => x.Dispatch_Master.ChallanId);

            // get all dealers challan
            var dealersStock = stock_MasterRepository.FindBy(x => x.SourceDetail.SourceId == 2
                                                                && challanIds.Contains(x.Id));
            if (model.dealerId != null)
                dealersStock = dealersStock.Where(x => x.SourceInfo_Id == model.dealerId);

            var stockIds = dealersStock.Select(x => x.Id);

            var _result = stockRepository
                    .FindBy(x => stockIds.Contains(x.Stock_mId))
                    .GroupBy(y => y.Stock_Master.SourceInfo_Id)
                    .Select(z => new
                    {
                        source_infoId = z.Key,
                        z.FirstOrDefault().Stock_Master.SourceDetail.SourceName,
                        model = z.Select(y => new
                        {
                            y.BookId,
                            y.Book.Price,
                            y.Quantity,
                            discount = y.Stock_Master.SourceDetail.DealerBookDiscounts.Any(x => x.BookCategoryId == y.BookId) ?
                                       y.Stock_Master.SourceDetail.DealerBookDiscounts.FirstOrDefault(x => x.BookCategoryId == y.BookId).DiscountPercentage : 0
                        }),
                    })
                    .Select(y => new
                    {
                        y.source_infoId,
                        y.SourceName,
                        totalPrice = y.model.Sum(a => (a.Price * a.Quantity) - (a.Price * a.Quantity * a.discount / 100))
                    });


            return Ok(new { result = _result });
        }

        [HttpPost]
        public IHttpActionResult get_InvoiceInfo(invoiceViewModel model)
        {
            var _dispatchResult = dispatchRepository
                                  .FilterByDate(model.from, model.to);

            if (!_dispatchResult.Any())
                return Ok(new { result = _dispatchResult });

            if (!string.IsNullOrEmpty(model.invoiceNo))
                _dispatchResult = dispatchRepository
                                  .FilterByInvoice(_dispatchResult, model.invoiceNo);       // final filteration get here 

            if (!_dispatchResult.Any())
                return Ok(new { result = _dispatchResult });

            var challanIds = _dispatchResult.Select(x => x.Dispatch_Master.ChallanId);

            // get all dealers challan
            var dealersStock = stock_MasterRepository.FindBy(x => x.SourceDetail.SourceId == 2
                                                                && challanIds.Contains(x.Id));
            if (model.dealerId != null)
                dealersStock = dealersStock.Where(x => x.SourceInfo_Id == model.dealerId);

            var stockIds = dealersStock.Select(x => x.Id);

            var _result = stock_MasterRepository
                   .FindBy(x => stockIds.Contains(x.Id))
                   //.GroupBy(y => y.Stock_Master.SourceInfo_Id)
                   .Select(z => new
                   {
                       //challanId = z.Id,
                       z.Dispatch_Masters.FirstOrDefault().DispatchInfo.Invoice_No,
                       Invoice_Id = z.Dispatch_Masters.FirstOrDefault().Id,
                       Invoice_Date = z.Dispatch_Masters.FirstOrDefault().DispatchInfo.CreationDate,
                       z.SourceDetail.SourceName,
                       model = z.Stocks.Select(y => new
                       {
                           y.BookId,
                           y.Book.Price,
                           y.Quantity,
                           discount = y.Stock_Master.SourceDetail.DealerBookDiscounts.Any(x => x.BookCategoryId == y.BookId) ?
                                      y.Stock_Master.SourceDetail.DealerBookDiscounts.FirstOrDefault(x => x.BookCategoryId == y.BookId).DiscountPercentage : 0
                       }),
                   })
                   .Select(y => new
                   {
                       y.SourceName,
                       y.Invoice_Id,
                       y.Invoice_No,
                       y.Invoice_Date,
                       totalPrice = y.model.Sum(a => (a.Price * a.Quantity) - (a.Price * a.Quantity * a.discount / 100))
                   })
                   .GroupBy(a => a.SourceName)
                   .Select(b => new
                   {
                       SourceName = b.Key,
                       invoiceInfo = b.Select(x => new
                       {
                           x.Invoice_Id,
                           x.Invoice_No,
                           x.Invoice_Date,
                           x.totalPrice
                       })
                   });

            return Ok(new { result = _result });
        }

        [HttpGet]
        public IHttpActionResult get_invoice_printInfo(long invoiceId)
        {
            // return new Id > but data still not save in DB bcoz of transaction :)
            var master = dispatch_MasterRepository.FindById(invoiceId);

            dynamic orderInfo = null;
            if (master.ChallanId != null)               // get record by challan no > means from stock table
            {
                var stock_Master = stock_MasterRepository
                                       .FindById(Convert.ToInt64(master.ChallanId));

                var Inventory_Src = inventorySourceDetailRepository.GetAll();
                var Event_SrcModel = eventManagementRepository.GetAll();

                orderInfo = Inventory_create_ViewModel
                            .Parse(stock_Master, Inventory_Src, Event_SrcModel);
            }

            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            var barCodeImg = barcode.Draw(master.DispatchInfo.Packet_Id, 50);


            var _result = new
            {
                dispatchInfo = new
                {
                    dispatch_mId = master.Id,
                    master.DispatchInfo.Invoice_No
                },
                packetInfo = new { master.DispatchInfo.Invoice_No, master.Id, master.DispatchInfo.Packet_Id, master.DispatchInfo.CreationDate, imgBase = dispatchRepository.getbase64(barCodeImg) },     // new objects
                orderInfo = orderInfo,
                status = "success"
            };
            return Ok(new { result = _result });
        }

        #endregion



        public StockController(IinventorySourceDetailRepository _inventorySourceDetailRepository,
            IinventorySourceRepository _inventorySourceRepository,
            IStockRepository _stockRepository,
            IStock_MasterRepository _stock_MasterRepository,
            IErrorLogsRepository _errorLogsRepository,
            IPurchaseOrderRepository _purchaseOrderRepository,
            IPurchaseOrder_MasterRepository _purchaseOrder_MasterRepository,
            IBookRepository _bookRepository,
            IUserRepository _userRepository,
            IDealerBookDiscountRepository _dealerBookDiscountRepository,
            IDealerSceondaryAddressRepository _dealerSceondaryAddressRepository,
            ICityRepository _cityRepository,
            IDispatchRepository _dispatchRepository,
            IDispatch_MasterRepository _dispatch_MasterRepository,
            ISchoolRepository schoolRepository,
            IEventManagementRepository _eventManagementRepository,
            ICounterCustomerRepository _counterCustomerRepository)
        {
            purchaseOrder_MasterRepository = _purchaseOrder_MasterRepository;
            purchaseOrderRepository = _purchaseOrderRepository;
            inventorySourceDetailRepository = _inventorySourceDetailRepository;
            inventorySourceRepository = _inventorySourceRepository;
            stockRepository = _stockRepository;
            stock_MasterRepository = _stock_MasterRepository;
            errorLogsRepository = _errorLogsRepository;
            bookRepository = _bookRepository;
            userRepository = _userRepository;
            dealerBookDiscountRepository = _dealerBookDiscountRepository;
            dealerSceondaryAddressRepository = _dealerSceondaryAddressRepository;
            cityRepository = _cityRepository;
            dispatchRepository = _dispatchRepository;
            dispatch_MasterRepository = _dispatch_MasterRepository;
            this.schoolRepository = schoolRepository;
            eventManagementRepository = _eventManagementRepository;
            counterCustomerRepository = _counterCustomerRepository;
        }

    }
}
