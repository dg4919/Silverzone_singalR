using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(SilverzoneERPContext context) : base(context) { }

        public List<Contact> GetBySchoolId(long SchoolId)
        {
            try
            {
                return _dbContext.Contacts.Where(x=>x.SchId== SchoolId && x.ContactYear==DateTime.Now.Year).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }                       
        }
    }
}
