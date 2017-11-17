using SilverzoneERP.Entities.Models.Common;
using System;

namespace SilverzoneERP.Entities.Models
{

    public class UserShippingAddress:Entity<long>
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public countryType CountryType { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        public DateTime create_date { get; set; }
               
    }
}
