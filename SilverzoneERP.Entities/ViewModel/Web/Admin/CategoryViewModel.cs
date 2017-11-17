using System.Collections.Generic;

namespace SilverzoneERP.Entities.ViewModel.Admin
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int? CouponId { get; set; }
    }

    public class ItemTitle_ViewModel
    {
        public long subjectId { get; set; }
        public IEnumerable<ItemTitle_ClassViewModel> entitys { get; set; }

        //public static ItemTitle_Master parse(ItemTitle_ClassViewModel model, long subjectId)
        //{

        //}
    }

    public class ItemTitle_ClassViewModel
    {
        public ItemTitle_ClassLimitViewModel classLimit { get; set; }
        public long categoryId { get; set; }
    }

    public class ItemTitle_ClassLimitViewModel
    {
        public long from { get; set; }
        public long to { get; set; }
    }



}

