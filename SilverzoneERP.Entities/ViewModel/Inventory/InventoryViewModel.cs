using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SilverzoneERP.Entities.ViewModel
{
    //public abstract class commonViewModel
    //{
    //    public long Id { get; set; }
    //    public DateTime CreationDate { get; set; }
    //    public long CreatedBy { get; set; }
    //    public bool Status { set; get; }
    //}

    public class Inventory_outward_ViewModel
    {
        #region properties 

        public long stockId { get; set; }   // default = 0
        public string Remarks { get; set; }

        [Required]
        public long srcId { get; set; }

        [Required]
        public long SourceInfo_Id { get; set; }

        [Required]
        public inventoryType InventoryType { get; set; }

        //[Required]
        public long? PO_Number { get; set; }

        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }

        [Required]
        public long BookId { get; set; }

        [Required]
        public long Quantity { get; set; }

        public long? dealer_adressId { get; set; }

        #endregion

        public static Stock_Master ParseM(Inventory_outward_ViewModel model)
        {
            return new Stock_Master()
            {
                SourceInfo_Id = model.SourceInfo_Id,
                InventoryType = model.InventoryType,
                Remarks = model.Remarks,
                ChallanDate = DateTime.Now,
                dealerAdresId = model.dealer_adressId,
                Status = true
            };
        }

        public static Stock Parse(Inventory_outward_ViewModel model)
        {
            return new Stock()
            {
                BookId = model.BookId,
                Quantity = model.Quantity,
                Status = true
            };
        }

        public static dynamic parse(IQueryable<PurchaseOrder_Master> modelList,
                                    IQueryable<Stock> stock_IN,
                                    IQueryable<Stock> stock_OUT,
                                    IQueryable<Stock> stock_List)
        {
            var aa = stock_IN
                    .GroupBy(x => x.BookId)
                    .Select(y => new
                    {
                        bookId = y.Key,
                        Inward = y.Sum(z => z.Quantity),
                    });

            var bb = stock_OUT
                     .GroupBy(x => x.BookId)
                     .Select(y => new
                     {
                         bookId = y.Key,
                         Outward = y.Sum(z => z.Quantity)
                     });

            var zz = (from inwards in aa
                      join outward in bb
                      on inwards.bookId equals outward.bookId into leftJoin
                      from res in leftJoin.DefaultIfEmpty()
                      select new
                      {
                          inwards.bookId,
                          inwards.Inward,
                          Outward = res != null ? res.Outward : 0
                      });

            var s = modelList.ToList()
                             .Select(x => new
                             {
                                 x.PO_Number,
                                 x.Remarks,
                                 PO_detail = (from PO in x.PurchaseOrders
                                              .Where(y => y.Status == true && y.is_Adjusted == false)
                                              join res1 in zz
                                              on PO.BookId equals res1.bookId into list
                                              from data in list.DefaultIfEmpty()
                                              select new
                                              {
                                                  PO.Id,
                                                  Quantity = PO.Quantity - get_stockQty(PO.PO_mId, PO.BookId, stock_List),
                                                  bookName = string.Format("{0} : {1} - {2}",
                                                              PO.Book.ItemTitle_Master.Subject.SubjectName,
                                                              PO.Book.ItemTitle_Master.Class.className,
                                                              PO.Book.ItemTitle_Master.BookCategory.Name),
                                                  bookId = PO.Book.Id,
                                                  AvailableStock = data != null ? data.Inward - data.Outward : 0
                                              })
                             })
                             .Select(a => new
                             {
                                 a.PO_Number,
                                 PO_detail = a.PO_detail.Where(b => b.Quantity > 0)
                             });

            return s;
        }

        private static long get_stockQty(long poId, long bookId, IQueryable<Stock> stock_List)
        {
            var stock = stock_List.Where(a => a.PO_Id == poId
                                      && a.BookId == bookId
                                      && a.Status == true);

            if (!stock.Any())
                return 0;

            return stock
                   .GroupBy(x => new { x.PO_Id, x.BookId })
                   .FirstOrDefault()
                   .Sum(x => x.Quantity);
        }

    }

    public class Inventory_outwardList_ViewModel
    {
        [Required]
        public long BookId { get; set; }

        [Required]
        public long Quantity { get; set; }
    }

    public class Inventory_create_ViewModel
    {
        #region properties

        // for update record
        public long stockId { get; set; }

        public long stock_mId { get; set; }   // default = 0
        public bool chekQty { get; set; }   // default value = false

        public string Remarks { get; set; }

        public long srcId { get; set; }
        public long SourceInfo_Id { get; set; }

        public inventoryType InventoryType { get; set; }

        public long? PO_Number { get; set; }

        [Required]
        public long BookId { get; set; }

        [Required]
        public long Quantity { get; set; }

        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }

        public long? dealer_adressId { get; set; }

        #endregion

        public static Stock_Master ParseM(Inventory_create_ViewModel model)
        {
            var stockM = new Stock_Master();

            stockM.SourceInfo_Id = model.SourceInfo_Id;
            stockM.InventoryType = model.InventoryType;
            stockM.Remarks = model.Remarks;
            stockM.Status = true;
            stockM.dealerAdresId = model.dealer_adressId;

            if (model.InventoryType == inventoryType.Inward)
            {
                stockM.ChallanNo = model.ChallanNo;
                stockM.ChallanDate = model.ChallanDate;
            }
            else if (model.InventoryType == inventoryType.Outward)
                stockM.ChallanDate = DateTime.Now;

            return stockM;
        }


        public static Stock Parse(Inventory_create_ViewModel model)
        {
            return new Stock()
            {
                BookId = model.BookId,
                Quantity = model.Quantity,
                Status = true
            };
        }

        public static Stock Parse(Stock model, Inventory_create_ViewModel vm)
        {
            model.Quantity = vm.Quantity;

            return model;
        }

        public static dynamic Parse(Stock_Master model,
                                    IQueryable<InventorySourceDetail> inventorySrc,
                                    IQueryable<Models.EventManagement> schoolSrc)
        {
            var poM = model.Stocks.FirstOrDefault().PO_Master;
            dynamic srcFrom, srcTo = null;
            Parse(model, inventorySrc, schoolSrc, poM, out srcFrom, out srcTo);

            // anonymous props
            return new
            {
                stock_mId = model.Id,
                stock_isVerified = model.isVerified,
                dealerAdress = getdealerAdres(srcTo, model, poM.To),
                model.Remarks,
                srcFrom_info = new
                {
                    SourceName = poM.From != orderSourceType.School ? srcFrom.SourceName : srcFrom.SchName,
                    SourceAddress = poM.From != orderSourceType.School ? srcFrom.SourceAddress : srcFrom.SchAddress,
                    SourceEmail = poM.From != orderSourceType.School ? srcFrom.SourceEmail : srcFrom.SchEmail,
                    SourceMobile = poM.From != orderSourceType.School ? srcFrom.SourceMobile : srcFrom.SchPhoneNo.ToString()
                },
                srcTo = poM.To != orderSourceType.School ? srcTo.Source.SourceName : "School",
                SourceType = poM.To != orderSourceType.School ? srcTo.SourceName : srcTo.SchName,
                SourceInfo_Id = poM.To != orderSourceType.School ? poM.srcFrom : srcTo.Id,    // for outward
                srcId = poM.To,               // for outward
                InventoryType = model.InventoryType,
                Status = model.Status,
                ChallanNo = model.ChallanNo,
                ChallanDate = model.ChallanDate,
                stockInfo = getInventory_details(model.Stocks, poM.To != orderSourceType.School ? srcTo.DealerBookDiscounts : null)
            };
        }

        private static void Parse(Stock_Master model,
                                  IQueryable<InventorySourceDetail> inventorySrc,
                                  IQueryable<Models.EventManagement> schoolSrc,
                                  PurchaseOrder_Master poM,
                                  out dynamic srcFrom,
                                  out dynamic srcTo)
        {
            if (poM.From == orderSourceType.School)      // 6 is use for school code :)
                srcFrom = schoolSrc.SingleOrDefault(x => x.Id == poM.srcFrom).School;
            else
                srcFrom = inventorySrc.SingleOrDefault(x => x.Id == poM.srcFrom);

            if (poM.To == orderSourceType.School)
                srcTo = schoolSrc.SingleOrDefault(x => x.Id == poM.srcTo).School;
            else
                srcTo = inventorySrc.SingleOrDefault(x => x.Id == poM.srcTo);

            if (model.InventoryType == inventoryType.Outward)
            {
                var temp = srcFrom;
                var temp1 = poM.From;

                srcFrom = srcTo;
                srcTo = temp;

                poM.From = poM.To;
                poM.To = temp1;
            }
        }

        private static dynamic getdealerAdres(dynamic model, Stock_Master master, orderSourceType ToId)
        {
            if (master.dealerAdresId == 0 || master.dealerAdresId == null)
                return new
                {
                    dealerAdresId = 0,
                    SourceMobile = ToId != orderSourceType.School ? model.SourceMobile : Convert.ToString(model.SchPhoneNo),
                    SourceEmail = ToId != orderSourceType.School ? model.SourceEmail : model.SchEmail,
                    SourceAddress = ToId != orderSourceType.School ? model.SourceAddress : model.SchAddress
                };

            return new
            {
                dealerAdresId = master.dealerAdresId,
                model.SourceMobile,
                model.SourceEmail,
                SourceAddress = master.SourceDetail.DealerSceondaryAddressess.ElementAt(Convert.ToInt16(master.dealerAdresId) - 1).Address
            };
        }

        private static dynamic getInventory_details(ICollection<Stock> stockList, ICollection<DealerBookDiscount> discountList)
        {
            return stockList.Where(x => x.Status == true)
                .OrderBy(y => y.Book.ItemTitle_Master.Class.className)      // order by in multiple fields
                .ThenBy(y => y.Book.ItemTitle_Master.Subject.SubjectName)
                .ThenBy(y => y.Book.ItemTitle_Master.BookCategory.Name)
                .Select(x => getInventory_details(x, discountList));
        }

        public static dynamic getInventory_details(Stock stock, ICollection<DealerBookDiscount> discountList = null)
        {
            var discount = discountList == null ? null : discountList.FirstOrDefault(y => y.BookCategoryId == stock.Book.ItemTitle_Master.CategoryId);
            decimal discountAmt = discount == null ? 0 : discount.DiscountPercentage;

            return new
            {
                stock.Id,
                stockId = stock.Stock_mId,
                PO_Number = stock.PO_Master == null ? "" : stock.PO_Master.PO_Number.ToString(),
                stock.Quantity,
                bookPrice = stock.Book.Price,
                Book = PurchaseOrder_ViewModel.get_BookInfo(stock.Book),
                DiscountPrice = discountAmt
                //DiscountPrice = x.Book.Price * discountAmt / 100,
            };
        }

        public static decimal getStock_TotalPrice(Stock_Master stockM)
        {

            var discounts = stockM.SourceDetail == null ? null : stockM.SourceDetail.DealerBookDiscounts;

            return stockM.Stocks.Where(x => x.Status == true)
                    .Sum(x => get_stockPrice(x, discounts));
        }

        private static decimal get_stockPrice(Stock stockInfo, ICollection<DealerBookDiscount> discountList = null)
        {
            var discount = discountList == null ? null : discountList.FirstOrDefault(y => y.BookCategoryId == stockInfo.Book.ItemTitle_Master.CategoryId);
            decimal discount_percent = discount == null ? 0 : discount.DiscountPercentage;

            decimal bookPrice = stockInfo.Book.Price * stockInfo.Quantity;
            decimal discountPrice = (bookPrice * discount_percent) / 100;
            return bookPrice - discountPrice;
        }

    }

    public class Inventory_source_ViewModel
    {
        #region Properties

        [Required]
        public long SourceId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string contactPerson { get; set; }
        [Required]
        public string mobile { get; set; }

        public string landline { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string panNo { get; set; }
        [Required]
        public string tanNo { get; set; }

        #endregion

        public static InventorySourceDetail parse(Inventory_source_ViewModel vm)
        {
            return new InventorySourceDetail()
            {
                SourceName = vm.name,
                SourceAddress = vm.address,
                Contact_Person_Name = vm.contactPerson,
                SourceMobile = vm.mobile,
                SourceLandline = vm.landline,
                SourceEmail = vm.email,
                SourcePAN = vm.panNo,
                SourceTAN = vm.tanNo,
                SourceId = vm.SourceId,
                Status = true
            };

        }

        public static InventorySourceDetail parse(Inventory_source_ViewModel vm, InventorySourceDetail model)
        {
            model.SourceName = vm.name;
            model.SourceAddress = vm.address;
            model.Contact_Person_Name = vm.contactPerson;
            model.SourceMobile = vm.mobile;
            model.SourceLandline = vm.landline;
            model.SourceEmail = vm.email;
            model.SourcePAN = vm.panNo;
            model.SourceTAN = vm.tanNo;
            model.SourceId = vm.SourceId;

            return model;
        }

        public static dynamic parse(IQueryable<InventorySourceDetail> model, IQueryable<City> cityList)
        {
            return model.Select(x => new
            {
                x.Id,
                name = x.SourceName,
                address = x.SourceAddress,
                mobile = x.SourceMobile,
                landline = x.SourceLandline,
                email = x.SourceEmail,
                panNo = x.SourcePAN,
                tanNo = x.SourceTAN,
                contactPerson = x.Contact_Person_Name,
                x.PinCode,
                x.Status,
                x.CreatedBy,
                x.CreationDate,
                cityModel = x.City_Id != null ? new
                {
                    x.City_Id,
                    State_Id = cityList.FirstOrDefault(y => y.Id == x.City_Id).StateId,
                    Country_Id = cityList.FirstOrDefault(y => y.Id == x.City_Id).State.CountryId,
                } : null,
                delaerBookDiscounts = x.DealerBookDiscounts
                                       .Where(q => q.Status == true)
                                       .Select(y => new
                                       {
                                           category = new
                                           {
                                               id = y.BookCategorys.Id,
                                               name = y.BookCategorys.Name
                                           },
                                           amount = y.DiscountPercentage
                                       }),
                DealerSceondaryAddressess = x.DealerSceondaryAddressess
                                             .Where(q => q.Status == true)
                                             .Select(z => new
                                             {
                                                 adresName = z.Address,
                                                 z.isDefault
                                             })
            });
        }

    }

    public class Dealer_inventory_source_ViewModel : Inventory_source_ViewModel
    {
        public long cityId { get; set; }
        public int pincode { get; set; }

        public int? defaultAdres_radio { get; set; }
        public IEnumerable<delaerBookDiscount_ViewModel> delaerBookDiscounts { get; set; }
        public string[] addressList { get; set; }
    }

    public class delaerBookDiscount_ViewModel
    {
        public long categoryId { get; set; }
        public decimal amount { get; set; }
    }


    public class PurchaseOrder_ViewModel
    {
        #region Properties 

        public long POId { get; set; }

        public long? PO_masterId { get; set; }

        public orderSourceType From { get; set; }
        public orderSourceType To { get; set; }

        public long srcFrom { get; set; }
        public long srcTo { get; set; }

        //[Required]
        public double Rate { get; set; }
        public string Remarks { get; set; }

        [Required]
        public long BookId { get; set; }
        [Required]
        public long Quantity { get; set; }

        #endregion

        public static PurchaseOrder parse(PurchaseOrder_ViewModel vm)
        {
            return new PurchaseOrder()
            {
                PO_mId = Convert.ToInt64(vm.PO_masterId),
                Rate = vm.Rate,
                Quantity = vm.Quantity,
                BookId = vm.BookId,
                Status = true
            };
        }

        public static PurchaseOrder_Master parseM(PurchaseOrder_ViewModel vm)
        {
            return new PurchaseOrder_Master()
            {
                From = vm.From,
                To = vm.To,
                srcFrom = vm.srcFrom,
                srcTo = vm.srcTo,
                PO_Date = DateTime.UtcNow,
                Remarks = vm.Remarks,
                Status = true
            };
        }

        public static PurchaseOrder parse(PurchaseOrder model, PurchaseOrder_ViewModel vm)
        {
            model.Rate = vm.Rate;
            model.Quantity = vm.Quantity;
            model.BookId = vm.BookId;

            return model;
        }

        public static InventorySourceDetail get_inventorySourceInfo(IQueryable<InventorySourceDetail> sorce, long srcId)
        {
            return sorce.SingleOrDefault(x => x.Id == srcId);
        }

        public static dynamic parse(PurchaseOrder_Master model, IQueryable<InventorySourceDetail> sorce)
        {
            var srcFrom = get_inventorySourceInfo(sorce, model.srcFrom);
            var srcTo = get_inventorySourceInfo(sorce, model.srcTo);

            return new
            {
                po_mId = model.Id,
                model.PO_Number,
                model.PO_Date,
                model.Remarks,
                From = srcFrom.SourceName,
                srcFrom = model.From,
                To = srcTo.Source.SourceName,
                Source = srcTo.SourceName,
                SourceEmail = srcTo.SourceEmail,
                PO_detail = getPO_details(model.PurchaseOrders)
            };
        }

        public static dynamic parse(IQueryable<PurchaseOrder_Master> entity, IQueryable<InventorySourceDetail> sorce)
        {
            var srcFrom = get_inventorySourceInfo(sorce, entity.FirstOrDefault().srcFrom);
            var srcTo = get_inventorySourceInfo(sorce, entity.FirstOrDefault().srcTo);

            return new
            {
                From = srcFrom.SourceName,
                To = srcTo.Source.SourceName,
                Source = srcTo.SourceName,
                PO_Masters = entity.ToList().Select(model => new        // to use any fxs in select its must to convert into a list
                {
                    model.PO_Number,
                    model.PO_Date,
                    PO_detail = getPO_details(model.PurchaseOrders)
                })
            };
        }

        public static dynamic parse(IQueryable<PurchaseOrder_Master> po_master, IQueryable<Stock> stockList)
        {
            return po_master
                  .GroupBy(x => x.srcTo)
                  .ToList()
                  .Select(model => new        // to use any fxs in select its must to convert into a list
                  {
                      PO_Masters = model.Select(PO_M => new
                      {
                          PO_M.PO_Number,
                          PO_detail = getPO_details(PO_M.PurchaseOrders, stockList)
                      })
                  }).FirstOrDefault();
        }

        private static dynamic getPO_details(ICollection<PurchaseOrder> List, IQueryable<Stock> stockList)
        {
            return List.Where(x => x.Status == true
                                && x.is_Adjusted == false)
                   .Select(x => new
                   {
                       pendingQty = x.Quantity - getstock_totalQuantity(stockList, x),
                       Book = get_BookInfo(x.Book)
                   })
                   .Where(x => x.pendingQty > 0);
        }

        public static long getstock_totalQuantity(IQueryable<Stock> stockList, PurchaseOrder PO)
        {
            var list = stockList.Where(y => y.PO_Id == PO.PO_mId && y.BookId == PO.BookId);

            if (list.Count() == 0)
                return 0;

            return list.GroupBy(g => new { g.BookId, g.PO_Id })
                 .Sum(s => s.Sum(a => a.Quantity));
        }


        private static dynamic getPO_details(ICollection<PurchaseOrder> List)
        {
            return List.Where(x => x.Status == true)
                   .Select(x => getPO_details(x));
        }

        public static dynamic getPO_details(PurchaseOrder x)
        {
            return new
            {
                x.Id,
                x.Rate,
                x.Quantity,
                Book = get_BookInfo(x.Book)
            };
        }

        public static dynamic get_BookInfo(Book book)
        {
            return new
            {
                bookISBN = book.ISBN,
                bookName = string.Format("{0} : {1} - {2}",
                              book.ItemTitle_Master.Subject.SubjectName,
                              book.ItemTitle_Master.Class.className,
                              book.ItemTitle_Master.BookCategory.Name),
                bookId_bundle = new
                {
                    bookId = book.Id,
                    classId = book.ItemTitle_Master.ClassId,
                    subjectId = book.ItemTitle_Master.SubjectId
                }
            };
        }

        public static dynamic parse(InventorySourceDetail model)
        {
            var List = model.DealerSceondaryAddressess.
                  Where(x => x.Status == true)
                  .Select(y => new sourceAdresList()
                  {
                      adressId = y.Id,
                      adress = y.Address,
                      isDefault = y.isDefault
                  }).ToList();

            List.Add(new sourceAdresList()
            {
                adressId = 0,
                adress = model.SourceAddress,
                isDefault = model.DealerSceondaryAddressess.Any(x => x.isDefault == true
                                                                  && x.Status == true) ? false : true
            });

            return List;
        }

        class sourceAdresList
        {
            public long adressId { get; set; }
            public string adress { get; set; }
            public bool isDefault { get; set; }
        }
    }

    public class pendingPO_ViewModel
    {
        public long BookId { get; set; }
        public long TotalQty { get; set; }
        public pendingPObook_ViewModel Book { get; set; }

        public static dynamic parse(IEnumerable<pendingPO_ViewModel> poList, IEnumerable<pendingPO_ViewModel> stockList)
        {
            var dt = (from PO in poList
                      join Stock in stockList
                      on PO.BookId equals Stock.BookId
                      into newList
                      from new_Stock in newList.DefaultIfEmpty()
                      select new
                      {
                          bookId = PO.BookId,
                          poQty = PO.TotalQty,
                          stockQty = new_Stock != null ? new_Stock.TotalQty : 0,
                          Book = PO.Book,
                      });
            return dt;
        }

    }

    public class pendingPObook_ViewModel
    {
        public string subjectName { get; set; }
        public string className { get; set; }
        public string categoryName { get; set; }
    }

    public class searchPO_ViewModel
    {
        public long SourceId { get; set; }
        public long SourceInfo_Id { get; set; }

        public int? poType { get; set; }
        public long poNo { get; set; }

        public long subjectId { get; set; }
        public long classId { get; set; }
        public long CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime from { get; set; }

        [DataType(DataType.Date)]
        public DateTime to { get; set; }


        //public static dynamic parse(IQueryable<PurchaseOrder_Master> model, long bookId)
        //{
        //    return model.ToList().Select(x => parse(x.PurchaseOrders, bookId));
        //}

        public static IEnumerable<pendingPO_ViewModel> parse(IQueryable<Stock> List)
        {
            return List
                       .ToList()
                       .GroupBy(x => x.BookId)
                       .Select(y => new pendingPO_ViewModel
                       {
                           BookId = y.Key,
                           TotalQty = y.Sum(a => a.Quantity),
                           Book = get_BookInfo(y.FirstOrDefault().Book)
                       });
        }

        public static dynamic parse(IQueryable<PurchaseOrder_Master> PO_Mlist,
                                    IQueryable<PurchaseOrder> POlist,
                                    IQueryable<Stock> stockList,
                                    IQueryable<InventorySourceDetail> sorce)
        {
            var aa = (from PO_M in PO_Mlist
                      join PO in POlist
                      on PO_M.Id equals PO.PO_mId
                      select new
                      {
                          SourceDetail = sorce.FirstOrDefault(x => x.Id == PO_M.srcTo),
                          PO.Book,
                          SourceInfo_Id = PO_M.srcTo,
                          BookId = PO.BookId,
                          POId = PO.PO_mId,
                          Qty = PO.Quantity
                      })
                      .GroupBy(x => x.SourceInfo_Id)
                      .ToList()
                      .Select(x => new
                      {
                          srcId = x.FirstOrDefault().SourceInfo_Id,
                          srcName = x.FirstOrDefault().SourceDetail.SourceName,
                          POdetails = x.GroupBy(y => y.BookId)
                          .Select(y => new
                          {
                              BookId = y.Key,
                              Book = get_BookInfo(y.FirstOrDefault().Book),
                              poQty = y.Sum(z => z.Qty),
                              stockQty = getstock_totalQuantity(stockList, y.Key, y.Select(z => z.POId))
                          })
                      });

            return aa;
        }

        private static long getstock_totalQuantity(IQueryable<Stock> stockList, long BookId, IEnumerable<long> PO_IDs)
        {
            var list = stockList.Where(y => y.BookId == BookId && PO_IDs.Contains((long)y.PO_Id));

            if (list.Count() == 0)
                return 0;

            return list.GroupBy(g => g.BookId)
                 .Sum(s => s.Sum(a => a.Quantity));
        }

        public static dynamic get_BookInfo(Book book)
        {
            return new pendingPObook_ViewModel
            {
                subjectName = book.ItemTitle_Master.Subject.SubjectName,
                className = book.ItemTitle_Master.Class.className,
                categoryName = book.ItemTitle_Master.BookCategory.Name
            };
        }

        public static string get_BookName(Book book)
        {
            return string.Format("{0} : {1} - {2}",
                              book.ItemTitle_Master.Subject.SubjectName,
                              book.ItemTitle_Master.Class.className,
                              book.ItemTitle_Master.BookCategory.Name);
        }

        public static dynamic parse(IQueryable<Stock> stock_IN, IQueryable<Stock> stock_OUT, IQueryable<Book> books)
        {

            var aa = stock_IN
                     .GroupBy(x => x.BookId)
                     .Select(y => new
                     {
                         bookId = y.Key,
                         Inward = y.Sum(z => z.Quantity)
                     });

            var bb = stock_OUT
                     .GroupBy(x => x.BookId)
                     .Select(y => new
                     {
                         bookId = y.Key,
                         Outward = y.Sum(z => z.Quantity)
                     });

            var zz = (from inwards in aa
                      join outward in bb
                      on inwards.bookId equals outward.bookId into leftJoin
                      from res in leftJoin.DefaultIfEmpty()
                      select new
                      {
                          inwards.bookId,
                          inwards.Inward,
                          Outward = res != null ? res.Outward : 0
                      });

            var record = (from book in books.ToList()
                          join stock in zz
                          on book.Id equals stock.bookId into list
                          from stoc in list.DefaultIfEmpty()
                          select new
                          {
                              BookId = book.Id,
                              BookName = get_BookName(book),
                              Total_stockIn = stoc == null ? 0 : stoc.Inward,
                              Total_stockOut = stoc == null ? 0 : stoc.Outward,
                              Available = stoc == null ? 0 : stoc.Inward - stoc.Outward,
                              book.ReorderLevel
                          });
            return record;
        }

        public static dynamic parseA(IQueryable<Stock> stocks)
        {
            return new
            {
                BookName = get_BookName(stocks.FirstOrDefault().Book),
                stockInfo = stocks.Select(stock => new
                {
                    stock.PO_Master.PO_Number,
                    stock.PO_Master.PO_Date,
                    stock.Quantity,
                    stock.Stock_Master.ChallanNo,
                    stock.Stock_Master.ChallanDate,
                })
            };
        }

    }

    public class orderViewModel
    {
        public long Id { get; set; }        // contain challan id, order id
        public orderSourceType sourceType { get; set; }

        public static Dispatch_Master parse(Dispatch_Master vm)
        {
            return new Dispatch_Master()
            {
                OrderId = vm.OrderId,
                OtherItemId = vm.OtherItemId,
                ChallanId = vm.ChallanId,
                DispatchType = vm.DispatchType,
                Status = true
            };
        }

        public static decimal parse(Stock_Master vm)
        {
            return vm.Stocks.Sum(x => x.Book.Price * x.Quantity);
        }

        public static Dispatch_Master parse(orderViewModel vm)
        {
            var model = new Dispatch_Master();
            model.Status = true;
            model.DispatchType = vm.sourceType;

            if (vm.sourceType == orderSourceType.Online)
                model.OrderId = vm.Id;
            else if (vm.sourceType == orderSourceType.Other)
                model.OtherItemId = vm.Id;
            else if (vm.sourceType != orderSourceType.Online)
                model.ChallanId = vm.Id;

            return model;
        }

        public static decimal parse(Dispatch_Master model, IQueryable<Packet_BundleInfo> packet_bundleInfos)
        {
            var res = packet_bundleInfos.Where(x => x.dispatch_mId == model.Id);

            decimal packet_wheightSum = 0;
            if (res.Any())
                packet_wheightSum = res.Sum(a => a.Package_Master.wheight);

            if (model.OrderId != null)          // online orders
                return model.Order.OrderDetails.Sum(x => parse(x)) + packet_wheightSum;
            else if (model.ChallanId != null)   // challan items
                return model.stockMasters.Stocks.Sum(x => x.Book.Weight * x.Quantity) + packet_wheightSum;
            else        // for other item 
                return 0;
        }

        private static decimal parse(OrderDetail detail)
        {
            if (detail.BookId != null)
                return detail.Book.Weight * detail.Quantity;
            else
                return detail.Bundle.bundle_details.Sum(y => y.book.Weight * detail.Quantity);
        }


        public static decimal parse(long dispatchMid, IQueryable<Packet_BundleInfo> packet_bundleInfos)
        {
            var res = packet_bundleInfos.Where(x => x.dispatch_mId == dispatchMid);

            decimal packet_wheightSum = 0;
            if (res.Any())
                packet_wheightSum = res.Sum(a => a.Netwheight);

            return packet_wheightSum;
        }

        public static dynamic parseA(long dispatchMid, IQueryable<Packet_BundleInfo> packet_bundleInfos)
        {
            var res = packet_bundleInfos.Where(x => x.dispatch_mId == dispatchMid);

            if (!res.Any())
                return null;

            return res.Select(x => new {
                x.PM_Id,
                x.Package_Master.Name,
                x.Netwheight
            });
        }

        public static decimal parse(IEnumerable<Stock> model)
        {
            return model.Sum(x => x.Book.Weight * x.Quantity);
        }
    }

    public class invoiceViewModel
    {
        public long? dealerId { get; set; }
        public string invoiceNo { get; set; }

        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }

    public class search_pendingPO_ViewModel
    {
        public orderSourceType srcId { get; set; }
        public long SourceInfo_Id { get; set; }
        public inventoryType type { get; set; }
    }


    public class Inventory_bulk_Create_ViewModel
    {
        // for update record
        public long stockId { get; set; }

        public long stock_mId { get; set; }   // default = 0

        public string Remarks { get; set; }

        public long srcId { get; set; }
        public long SourceInfo_Id { get; set; }

        public inventoryType InventoryType { get; set; }

        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }

        public long? dealer_adressId { get; set; }

        public IEnumerable<Bulk_Create_ViewModel> stockList { get; set; }

        public static Stock_Master ParseM(Inventory_bulk_Create_ViewModel model)
        {
            var stockM = new Stock_Master();

            stockM.SourceInfo_Id = model.SourceInfo_Id;
            stockM.InventoryType = model.InventoryType;
            stockM.Remarks = model.Remarks;
            stockM.Status = true;
            stockM.dealerAdresId = model.dealer_adressId;

            if (model.InventoryType == inventoryType.Inward)
            {
                stockM.ChallanNo = model.ChallanNo;
                stockM.ChallanDate = model.ChallanDate;
            }
            else if (model.InventoryType == inventoryType.Outward)
                stockM.ChallanDate = DateTime.Now;

            return stockM;
        }

        public static Stock Parse(Bulk_Create_ViewModel model)
        {
            return new Stock()
            {
                BookId = model.BookId,
                Quantity = model.Quantity,
                Status = true
            };
        }
    }

    public class Bulk_Create_ViewModel
    {
        public long PO_Number { get; set; }

        [Required]
        public long BookId { get; set; }

        [Required]
        public long Quantity { get; set; }
    }

    public class challanVerify_ViewModel
    {
        public long ChallanNumber { get; set; }
        public IEnumerable<challan_ViewModel> ChallanList { get; set; }

        public static dynamic Parse(IEnumerable<challan_ViewModel> challanList,
                                  IEnumerable<challanInfo_ViewModel> stocks)
        {
            #region Summary Code Info
            // 0 for not found
            // 1 for qty not matched
            // 2 for OK
            #endregion
            var res = challanList.Select(challan => new
            {
                status = Parse(challan, stocks)
            });

            bool record_IsVerified = true;
            
            foreach (var r in res)
            {
                if (r.status != 2)
                    record_IsVerified = false;
            }

            // both verified challan & stock detail matchd .. then true
            if (res.Count() != stocks.Count())
                record_IsVerified = false;

            return new
            {
                result = res,
                stock_isVerified = record_IsVerified
            };

        }

        public static int Parse(challan_ViewModel challan,
                                  IEnumerable<challanInfo_ViewModel> stocks)
        {
            var stock = stocks.Where(x => x.BookId == challan.BookId);

            if (!stock.Any())
                return 0;

            long qty = stock.Sum(y => y.challanQty);
            if (qty == challan.Quantity)
                return 2;
            else
                return 1;
        }

    }

    public class challan_ViewModel 
    {
        public long BookId { get; set; }
        public long Quantity { get; set; }
    }

    public class challanInfo_ViewModel
    {
        public long BookId { get; set; }
        public string BookName { get; set; }
        public long challanQty { get; set; }
    }

    public class CounterCustomer_ViewModel
    {
        public long StockId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string emailId { get; set; }
        public PaymentModeType PaymentMode { get; set; }

        public static CounterCustomer parse(CounterCustomer_ViewModel vm)
        {
            return new CounterCustomer()
            {
                StockId = vm.StockId,
                address = vm.address,
                emailId = vm.emailId,
                mobile = vm.mobile,
                name = vm.name,
                PaymentMode = vm.PaymentMode
            };
        }
    }

}
