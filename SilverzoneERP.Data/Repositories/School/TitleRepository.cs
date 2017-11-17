using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public class TitleRepository : BaseRepository<Title>, ITitleRepository
    {       
        public TitleRepository(SilverzoneERPContext _dbContext) : base(_dbContext) { }

        public bool Exists(string TitleName)
        {
            return _dbset.FirstOrDefault(x => x.TitleName.Trim().ToLower() == TitleName.Trim().ToLower()) == null ? false : true;
        }
        public bool Exists(long TitleId, string TitleName)
        {
            return _dbset.FirstOrDefault(x =>x.Id!= TitleId && x.TitleName.Trim().ToLower() == TitleName.Trim().ToLower()) == null ? false : true;
        }
        public Title Get(long TitleId)
        {
            return _dbset.FirstOrDefault(x=>x.Id==TitleId);
        }
        public IList<Title> Get(bool Status)
        {
            return _dbset.Where(x => x.Status == true).ToList();
        }
    }
}
