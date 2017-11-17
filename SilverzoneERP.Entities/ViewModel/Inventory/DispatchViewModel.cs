using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SilverzoneERP.Entities.ViewModel.Inventory
{
    public class DispatchViewModel
    {
        #region Properties

        [Required]
        public orderSourceType sourceType { get; set; }
        public long? sourceId { get; set; }

        [Required]
        public itemType ItemType { get; set; }
        [Required]
        public string Items { get; set; }

        public bool packet_isOn_Hold { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public long CityId { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public int PinCode { get; set; }
        public string ContactPerson { get; set; }

        [Required]
        public labelType LabelType { get; set; }
        public string LabelName { get; set; }
        public long? CordinatorId { get; set; }

        #endregion

        public static Dispatch_otherItem_Master parse(DispatchViewModel vm)
        {
            return new Dispatch_otherItem_Master()
            {
                sourceType = vm.sourceType,
                sourceId = vm.sourceId,
                ItemType = vm.ItemType,
                Items = vm.Items,
                packet_isOn_Hold = vm.packet_isOn_Hold,
                Status = true
            };
        }

        public static Dispatch_otherItem_address parseA(DispatchViewModel vm)
        {
            return new Dispatch_otherItem_address()
            {
                Name = vm.Name,
                Address = vm.Address,
                CityId = vm.CityId,
                PhoneNo = vm.PhoneNo,
                PinCode = vm.PinCode,
                ContactPerson = vm.ContactPerson,
                LabelType = vm.LabelType,
                LabelName = vm.LabelName,
                CordinatorId = vm.CordinatorId,
                Status = true
            };
        }

        public static dynamic parse(Models.School model)
        {
            return new
            {
                sourceId = model.Id,
                Name = model.SchName,
                cityModel = new {
                   model.CountryId,
                   model.StateId,
                   model.CityId
                   },
                Address = model.SchAddress,
                PhoneNo = model.SchPhoneNo,
                PinCode = model.SchPinCode,
                //ContactPerson = model.Contact.Select(x => x.ContactName),
                Cordinators = model.EventManagement.Any() ? model.EventManagement.Select(x => x.CoOrdinator.Select(z => z.CoOrdName)) : null,
                Principal = model.Contact.Any() ? parse(model.Contact, 1) : null,
                HM = model.Contact.Any() ? parse(model.Contact, 3) : null,
                Other = model.Contact.Any() ? parse(model.Contact, 5) : null
            };
        }

        private static string parse(IList<Contact> ContactList, long DesgId)
        {
            var _entity = ContactList.FirstOrDefault(x => x.DesgId == DesgId);

            if (_entity != null)
                return _entity.ContactName;

            return string.Empty;
        }

        public static dynamic parse(IQueryable<Dispatch_otherItem_address> dispatchList, IQueryable<City> cityList)
        {
            return dispatchList
                  .ToList()
                  .Select(x => parse(x, cityList.FirstOrDefault(y => y.Id == x.CityId)));
        }


        public static dynamic parse(Dispatch_otherItem_address model, City _cityModel)
        {
            return new
            {
                Address = model.Address,
                cityModel = new
                {
                    countryName = _cityModel.Country.CountryName,
                    stateName = _cityModel.State.StateName,
                    cityName = _cityModel.CityName
                },
                ContactPerson = model.ContactPerson,
                Items = model.Dispatch_otherItem_Master.Items,
                PhoneNo = model.PhoneNo,
                PinCode = model.PinCode,
                Name = model.Name
            };
        }
    }

}
