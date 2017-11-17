using Microsoft.AspNet.SignalR;
using SilverzoneERP.Api.hubs;
using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.Admin;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Admin
{
    public class CategoryController : ApiController
    {
        IBookCategoryRepository bookCategoryRepository;
        ICouponRepository couponRepository;
        IClassRepository classRepository;
        IEventRepository eventRepository;
        IitemTitle_MasterRepository itemTitle_MasterRepository;
        IErrorLogsRepository errorLogsRepository;

        [HttpPost]
        public IHttpActionResult Create(CategoryViewModel model)
        {
            if (model != null)
            {
                if (bookCategoryRepository.GetByName(model.Name).Count == 0)
                {
                    bookCategoryRepository.Create(new BookCategory()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Status = true,
                        CouponId = model.CouponId
                    });

                    var _context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    _context.Clients.All.addCategory("data is added... ");

                    return Ok(new { result = "Success" });
                }
            }
            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            // anonymous properties
            var category = bookCategoryRepository.GetAll().Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                Status = x.Status,
                x.Coupons.Coupon_name,
                x.CouponId,
                x.CreationDate,
                x.CreatedBy,
                x.UpdationDate,
                x.UpdatedBy
            });

            return Ok(new { result = category });
        }

        [HttpPost]  // automatically assign value if send from ajax in a single model
        public IHttpActionResult Edit(CategoryViewModel model)
        {
            if (model != null)
            {
                // if record is not exist only the update
                if (!bookCategoryRepository.Iscategory_Exist(model.Name, model.Id))
                {
                    //  find record and update data which is nescessary
                    var category = bookCategoryRepository.GetById(model.Id);
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.Status = model.Status;
                    category.CouponId = model.CouponId;

                    // update records
                    bookCategoryRepository.Update(category);
                    return Ok(new { result = "Success" });
                }
            }

            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult Delete(int categoryId)
        {
            if (categoryId != 0)
            {
                bookCategoryRepository.Delete(bookCategoryRepository.
                    GetById(categoryId));

                return Ok(new { result = "Category deleted sucessfully" });
            }
            return Ok(new { result = "Category is not found" });
        }

        [HttpGet]
        public IHttpActionResult Search(string name, bool status)
        {
            var List = name == null ?
                         bookCategoryRepository.GetByStatus(status) :
                         bookCategoryRepository.GetByNameAndStatus(name, status);

            var result = List.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                CouponId = x.Coupons.Id,
                CreatedBy = x.CreatedBy,
                CreationDate = x.CreationDate,
                UpdatedBy = x.UpdatedBy,
                UpdationDate = x.UpdationDate
            });

            return Ok(new { result = result });
        }


        // *****************  Coupon for Category of Book code start from Here  ****************************

        [HttpPost]
        public IHttpActionResult coupon_Create(Coupon model)
        {
            if (model != null)
            {
                if (couponRepository.check_Coupon(model.Coupon_name, model.DiscountType))
                    return Ok(new { result = "exist" });

                model.Status = true;
                couponRepository.Create(model);

                return Ok(new { result = "Success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult coupon_names()
        {
            // anonymous properties
            var category = couponRepository.GetAll()
                .Where(x => x.Status == true)
                .Select(x => new
                {
                    x.Id,
                    x.Coupon_name
                });

            return Ok(new { result = category });
        }


        [HttpGet]
        public IHttpActionResult coupon_List()
        {
            // anonymous properties
            var category = couponRepository.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Coupon_name,
                    x.Description,
                    x.Coupon_amount,
                    x.Start_time,
                    x.End_time,
                    x.Status,
                    DiscountType = x.DiscountType,
                    DiscountName = x.DiscountType == CouponType.FlatDiscount ? "Flat Discount" : "Percentage Discount"
                });

            return Ok(new { result = category });
        }

        [HttpPost]
        public IHttpActionResult coupon_Edit(Coupon model)
        {
            if (model != null && ModelState.IsValid)
            {
                // if record is not exist only the update
                if (couponRepository.Iscoupon_Exist(model.Id, model.Coupon_name, model.DiscountType))
                    return Ok(new { result = "exist" });

                var coupon = couponRepository.FindById(model.Id);

                coupon.Coupon_amount = model.Coupon_amount;
                coupon.Description = model.Description;
                coupon.Coupon_name = model.Coupon_name;
                coupon.DiscountType = model.DiscountType;
                coupon.Start_time = coupon.Start_time;
                coupon.End_time = model.End_time;

                couponRepository.Update(coupon);
                return Ok(new { result = "Success" });
            }

            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult coupon_Delete(int couponId, bool status)
        {
            if (couponId != 0)
            {
                var _model = couponRepository.FindById(couponId);
                _model.Status = status;

                couponRepository.Update(_model);

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "notfound" });
        }

        // *****************  Constructor  ********************************


        [HttpPost]
        public IHttpActionResult Create_ItemTitle(ItemTitle_ViewModel model)
        {
            using (var transaction = itemTitle_MasterRepository.BeginTransaction())
            {
                try
                {
                    foreach (var title in model.entitys)
                    {
                        if (title.classLimit != null &&
                            title.classLimit.from != 0 &&
                            title.classLimit.to != 0)
                        {
                            for (long i = title.classLimit.from; i <= title.classLimit.to; i++)
                            {
                                itemTitle_MasterRepository.Create(new ItemTitle_Master()
                                {
                                    SubjectId = model.subjectId,
                                    CategoryId = title.categoryId,
                                    ClassId = i,
                                    Status = true
                                }, false);
                            }
                        }
                    }
                    itemTitle_MasterRepository.Save();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    errorLogsRepository.logError(ex);
                    transaction.Rollback();
                }
            }
            return Ok(new { result = "ok" });
        }


        [HttpGet]
        public IHttpActionResult get_Item()
        {
            var _class = classRepository
                        .FindBy(x => x.Status == true)
                        .Select(y => new
                        {
                            y.Id,
                            y.className
                        });

            var _subject = eventRepository
                        .FindBy(x => x.Status == true)
                        .Select(y => new
                        {
                            y.Id,
                            y.SubjectName
                        });

            var _category = bookCategoryRepository
                        .FindBy(x => x.Status == true)
                        .Select(y => new
                        {
                            y.Id,
                            y.Name
                        });

            return Ok(new
            {
                result = new
                {
                    classList = _class,
                    subjectList = _subject,
                    categoryList = _category
                }
            });
        }


        public CategoryController(IBookCategoryRepository _bookCategoryRepository,
                                 ICouponRepository _couponRepository,
                                 IClassRepository _classRepository,
                                 IEventRepository _eventRepository,
                                 IitemTitle_MasterRepository _itemTitle_MasterRepository,
                                 IErrorLogsRepository _errorLogsRepository)
        {
            bookCategoryRepository = _bookCategoryRepository;
            couponRepository = _couponRepository;
            classRepository = _classRepository;
            eventRepository = _eventRepository;
            itemTitle_MasterRepository = _itemTitle_MasterRepository;
            errorLogsRepository = _errorLogsRepository;
        }


    }
}
