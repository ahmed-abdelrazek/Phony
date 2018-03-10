namespace Phony.ViewModel
{
    public enum PaymentPeriodType : byte
    {
        Once,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum Month : byte
    {
        NotSet,
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum CardType : byte
    {
        PrePaid,
        DebitCard,
        CreditCard
    }

    public enum UserGroup : byte
    {
        None,
        Manager,
        Employee
    }

    public enum ItemGroup : byte
    {
        None,
        Other,
        Card
    }
}