using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class FormManagementRepository:BaseRepository<FormManagement>,IFormManagementRepository
    {
        public FormManagementRepository(SilverzoneERPContext context):base(context) { }
       
    }
}
