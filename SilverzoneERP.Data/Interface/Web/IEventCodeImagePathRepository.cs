using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public  interface IEventCodeImagePathRepository:IRepository<EventCodeImagePath>
    {
        EventCodeImagePath GeByEventCode(int eid);
    }
}
