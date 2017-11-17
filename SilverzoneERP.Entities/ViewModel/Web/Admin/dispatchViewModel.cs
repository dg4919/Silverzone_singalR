using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace Silverzone.Web.ViewModel.Admin
{
    public class dispatchViewModel
    {
        public string packetNumber { get; set; }
        public decimal wheight { get; set; }
    }

    public class search_dispatchViewModel
    {
        public orderSourceType? srcId { get; set; }
        public string srchTxt { get; set; }
        public bool is_likeSrch { get; set; }

        public orderStatusType? pcktSts { get; set; }
        public int? pcktType { get; set; }

        public decimal? minWheight { get; set; }
        public decimal? maxWheight { get; set; }

        public string packetNo { get; set; }
        public string invoiceNo { get; set; }
        public itemType? ItemType { get; set; }
        public long courierId { get; set; }
        public long courier_modeId { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

    }

    public class searchModel
    {
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string cityName { get; set; }
        public string PinCode { get; set; }

        public static searchModel parse(Dispatch_otherItem_address model)
        {
            return new searchModel()
            {
                PhoneNo = model.PhoneNo,
                EmailId = string.Empty,
                Address = model.Address,
                countryName = model.CityInfo.Country.CountryName,
                stateName = model.CityInfo.State.StateName,
                cityName = model.CityInfo.CityName,
                PinCode = model.PinCode.ToString()
            };
        }

        public static searchModel parse(UserShippingAddress model)
        {
            return new searchModel()
            {
                Address = model.Address,
                PinCode = model.PinCode,
                cityName = model.City,
                countryName = model.CountryType.ToString(),
                EmailId = model.Email,
                PhoneNo = model.Mobile,
                stateName = model.State
            };
        }


        public static searchModel parse(Stock_Master model,
                                        IQueryable<InventorySourceDetail> inventorySrc,
                                        IQueryable<EventManagement> schoolSrc)
        {
            var poM = model.Stocks.FirstOrDefault().PO_Master;

            dynamic src;
            if(poM.From == orderSourceType.School)
                src = schoolSrc.SingleOrDefault(x => x.Id == poM.srcFrom).School;
                else
                src = inventorySrc.SingleOrDefault(x => x.Id == poM.srcFrom);

            return new searchModel()
            {
                Address = poM.From != orderSourceType.School ? src.SourceAddress : src.SchAddress,
                EmailId = poM.From != orderSourceType.School ? src.SourceEmail : src.SchEmail,
                PhoneNo = poM.From != orderSourceType.School ? src.SourceMobile : src.SchPhoneNo.ToString(),
                PinCode = poM.From != orderSourceType.School ? src.PinCode.ToString() : src.SchPinCode.ToString(),
                countryName = poM.From != orderSourceType.School ? (src.City == null ? string.Empty : src.City.Country.CountryName) : src.Country.CountryName,
                stateName = poM.From != orderSourceType.School ? (src.City == null ? string.Empty : src.City.State.StateName) : src.State.StateName,
                cityName = poM.From != orderSourceType.School ? (src.City == null ? string.Empty : src.City.CityName) : src.City.CityName
            };
        }

    }


}