using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IUserRepository: IRepository<ERPuser>
    {
        ERPuser GetById(long id);
        ERPuser GetByEmail(string EmailID);
        ERPuser SignIn(string EmailID, string Password);
        dynamic Get(int StartIndex, int limit, out long Count);
        dynamic Get();
        ERPuser Get(long UserId);
        dynamic GetCreated_UpdatedBy(long UserId);
        string WriteImage(string Base64image);
        bool DeleteImage(string ImagePath);
        string GetUserName(long UserId);
    }
}
