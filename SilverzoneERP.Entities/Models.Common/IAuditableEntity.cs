using System;

namespace SilverzoneERP.Entities.Models.Common
{
    public interface IAuditableEntity
    {
        #region Property

        DateTime CreationDate { get; set; }
        long CreatedBy { get; set; }
        DateTime UpdationDate { get; set; }
        long UpdatedBy { get; set; }
       
        byte[] RowVersion { set; get; }

        //User User { set; get; }
       
        #endregion

        //#region ForeignKey

        //[ForeignKey("CreatedBy")]
        //User User_CreatedBy { set; get; }

        //[ForeignKey("UpdatedBy")]
        //User User_UpdatedBy { set; get; }

        //#endregion
    }
}
