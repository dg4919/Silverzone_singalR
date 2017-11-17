namespace SilverzoneERP.Entities
{
    public enum ModeType
    {
        Cash = 1,
        Cheuque = 2,
        DD = 3,
        Online_Transfer = 4,
        Bank_Deposit = 5        
    }

    public enum PaymentType
    {
        Paid = 1,
        Refund = 2,
        Adjust = 3,
    }

    public enum PaymentAgainst
    {
        Registration = 1,
        Book = 2,
        Both = 3,
    }
}
