namespace SilverzoneERP.Entities.Models.Common
{
    public class Permission : Entity<long>
    {
        public bool Add { set; get; }
        public bool Edit { set; get; }
        public bool Delete { set; get; }
        public bool Read { set; get; }
        public bool Print { set; get; }

    }
}
