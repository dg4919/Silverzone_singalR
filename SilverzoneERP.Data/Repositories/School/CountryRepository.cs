using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class CountryRepository:BaseRepository<Country>,ICountryRepository
    {
        public CountryRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(string CountryName)
        {
            return _dbset.FirstOrDefault(x => x.CountryName.Trim().ToLower() == CountryName.Trim().ToLower()) == null ? false : true;
        }
        public bool Exists(long CountryId, string CountryName)
        {
            return _dbset.FirstOrDefault(x =>x.Id!= CountryId && x.CountryName.Trim().ToLower() == CountryName.Trim().ToLower()) == null ? false : true;
        }
        public Country Get(long CountryId)
        {
            return _dbset.FirstOrDefault(x => x.Id == CountryId);
        }
    }
}
