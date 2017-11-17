using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ICountryRepository:IRepository<Country>
    {
        bool Exists(string CountryName);
        bool Exists(long CountryId, string CountryName);
        Country Get(long CountryId);
    }
}
