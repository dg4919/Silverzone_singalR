using SilverzoneERP.Entities.ViewModel.Site;
using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class BannerController : ApiController
    {
        private IBannerRepository bannerRepository;

        public BannerController(IBannerRepository _bannerRepository)
        {
            bannerRepository = _bannerRepository;
        }
       [HttpGet]
       public IHttpActionResult GetBannerAll()
        {
            var bannerList = bannerRepository.GetAll().ToList();
            return Ok(new { result = bannerList });
        }

        [HttpGet]
        public IHttpActionResult GetBannerById(int id)
        {
            var banner = bannerRepository.GetById(id);
            return Ok(new { result = banner });
        }
        [HttpGet]
        public IHttpActionResult delete_book(int Id)
        {
            var model = bannerRepository.FindById(Id);
            model.Status = false;
            bannerRepository.Update(model);

            return Ok();
        }

      

        [HttpPut]
        public IHttpActionResult update_book(BannerViewModel model)
        {
            // return new insterted record Identity value
            var entity = bannerRepository.FindById(model.Id);

            if (entity != null)
            {
                // now transaction start > we can change in any table & commit OR rollback records
                using (var transaction = bannerRepository.BeginTransaction())
                {
                    try
                    {

                       bannerRepository.Save();       // save changes
                        transaction.Commit();       // it must be there if want to save record :)

                        return Ok(new { result = "success" });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error" });
                    }
                }
            }

            return Ok(new { result = "notfound" });
        }

    }
}
