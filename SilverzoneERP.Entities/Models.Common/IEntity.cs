namespace SilverzoneERP.Entities.Models.Common
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
