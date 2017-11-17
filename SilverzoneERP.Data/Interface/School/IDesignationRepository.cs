using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        bool Exists(string DesgName);
        bool Exists(long DesgId, string DesgName);
        Designation Get(long DesgId);
        IList<Designation> Get(bool Status);
    }
}
