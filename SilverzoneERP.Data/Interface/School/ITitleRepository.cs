using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ITitleRepository : IRepository<Title>
    {
        bool Exists(string TitleName);
        bool Exists(long TitleId, string TitleName);
        Title Get(long TitleId);
        IList<Title> Get(bool Status);
    }
}
