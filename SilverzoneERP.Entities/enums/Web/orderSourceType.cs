namespace SilverzoneERP.Entities
{
    // this must be matched from inventorysource table
    public enum orderSourceType
    {
        Press = 1,
        Dealer,
        Online,
        Branch,
        InHouse,
        School,
        Silverzone,
        Scrap,
        Other
    }

    public enum itemType
    {
       Broucher = 1,
       Other 
    }

    public enum labelType
    {
        None,
        Principal,
        HM,
        Cordinator,
        Other_Name
    }
}
