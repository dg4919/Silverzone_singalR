namespace SilverzoneERP.Entities
{
    // for all enums we suffix with type > so that we can know this is a enum type
    public enum orderStatusType
    {
        Pending = 1,  // when order is recived from online/offline/silverzone etc and label is generated
        Shipped,
        Intransit,
        Delivered,
        RTO,
        Settled,      // when user return/lost item then settled
        Resend
    }
}
