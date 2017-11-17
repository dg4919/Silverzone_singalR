namespace SilverzoneERP.Entities.Models.Common
{
    public abstract class BaseEntity { }
    public abstract class Entity<T> : BaseEntity
    {
        public virtual T Id { get; set; }

        public bool Status { set; get; }
    }
}
