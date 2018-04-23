using Phony.Extensions;

namespace Phony.ViewModel
{
    public enum PaymentType : byte
    {
        [LocalizedDescription("كاش", typeof(string))]
        Cash,
        [LocalizedDescription("اجل", typeof(string))]
        Credit
    }

    public enum Month : byte
    {
        [LocalizedDescription("يناير", typeof(string))]
        January = 1,
        [LocalizedDescription("فبراير", typeof(string))]
        February,
        [LocalizedDescription("مارس", typeof(string))]
        March,
        [LocalizedDescription("ابريل", typeof(string))]
        April,
        [LocalizedDescription("مايو", typeof(string))]
        May,
        [LocalizedDescription("يونيو", typeof(string))]
        June,
        [LocalizedDescription("يوليو", typeof(string))]
        July,
        [LocalizedDescription("اغسطس", typeof(string))]
        August,
        [LocalizedDescription("سبتمبر", typeof(string))]
        September,
        [LocalizedDescription("اكتوبر", typeof(string))]
        October,
        [LocalizedDescription("نوفمبر", typeof(string))]
        November,
        [LocalizedDescription("ديسمبر", typeof(string))]
        December
    }

    public enum Theme
    {
        BaseLight,
        BaseDark
    }

    public enum Color
    {
        Red,
        Green,
        Blue,
        Purple,
        Orange,
        Lime,
        Emerald,
        Teal,
        Cyan,
        Cobalt,
        Indigo,
        Violet,
        Pink,
        Magenta,
        Crimson,
        Amber,
        Yellow,
        Brown,
        Olive,
        Steel,
        Mauve,
        Taupe,
        Sienna,
    }

    public enum UserGroup : byte
    {
        [LocalizedDescription("لا يوجد", typeof(string))]
        None,
        [LocalizedDescription("مدير", typeof(string))]
        Manager,
        [LocalizedDescription("مستخدم", typeof(string))]
        Employee
    }

    public enum ItemGroup : byte
    {
        [LocalizedDescription("لا يوجد", typeof(string))]
        None,
        [LocalizedDescription("اخرى", typeof(string))]
        Other,
        [LocalizedDescription("كارت شحن", typeof(string))]
        Card
    }

    public enum NoteGroup : byte
    {
        [LocalizedDescription("ارقام", typeof(string))]
        Numbers,
        [LocalizedDescription("اخرى", typeof(string))]
        Other
    }
}