using System.ComponentModel;

namespace Phony.ViewModel
{
    public enum PaymentType : byte
    {
        [Description("كاش")]
        Cash,
        [Description("اجل")]
        Credit
    }

    public enum Month : byte
    {
        [Description("يناير")]
        January = 1,
        [Description("فبراير")]
        February,
        [Description("مارس")]
        March,
        [Description("ابريل")]
        April,
        [Description("مايو")]
        May,
        [Description("يونيو")]
        June,
        [Description("يوليو")]
        July,
        [Description("اغسطس")]
        August,
        [Description("سبتمبر")]
        September,
        [Description("اكتوبر")]
        October,
        [Description("نوفمبر")]
        November,
        [Description("ديسمبر")]
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
        [Description("لا يوجد")]
        None,
        [Description("مدير")]
        Manager,
        [Description("مستخدم")]
        Employee
    }

    public enum ItemGroup : byte
    {
        [Description("لا يوجد")]
        None,
        [Description("اخرى")]
        Other,
        [Description("كارت شحن")]
        Card
    }

    public enum NoteGroup : byte
    {
        [Description("ارقام")]
        Numbers,
        [Description("اخرى")]
        Other
    }
}