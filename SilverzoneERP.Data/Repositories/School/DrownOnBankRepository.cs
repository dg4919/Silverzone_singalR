using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class DrownOnBankRepository : BaseRepository<DrownOnBank>, IDrownOnBankRepository
    {
        public DrownOnBankRepository(SilverzoneERPContext context) : base(context) { }

      
    }
}
