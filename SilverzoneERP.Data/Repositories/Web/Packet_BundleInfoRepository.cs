using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class Packet_BundleInfoRepository : BaseRepository<Packet_BundleInfo>, IPacket_BundleInfoRepository
    {
        public Packet_BundleInfoRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
