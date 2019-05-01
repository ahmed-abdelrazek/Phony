using System.ComponentModel;

namespace Phony.WPF.Data
{
    public enum PaymentType : byte
    {
        [Description("نقدى")]
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
        [Description("احمر")]
        Red,
        [Description("اخضر")]
        Green,
        [Description("ازرق")]
        Blue,
        [Description("أرجوانى")]
        Purple,
        [Description("برتقالى")]
        Orange,
        [Description("لمونى")]
        Lime,
        [Description("زمردى")]
        Emerald,
        [Description("شرشيري")]
        Teal,
        [Description("سماوى")]
        Cyan,
        [Description("ازرق فاتح")]
        Cobalt,
        [Description("نيلى")]
        Indigo,
        [Description("بنفسجي")]
        Violet,
        [Description("زهرى")]
        Pink,
        [Description("وردى")]
        Magenta,
        [Description("قرمزى")]
        Crimson,
        [Description("عنبرى")]
        Amber,
        [Description("اصفر")]
        Yellow,
        [Description("بنى")]
        Brown,
        [Description("زيتونى")]
        Olive,
        [Description("رصاصى")]
        Steel,
        [Description("موف")]
        Mauve,
        [Description("")]
        Taupe,
        [Description("")]
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

    public enum BarCodeEncoders : byte
    {
        [Description("UPC-A")]
        UPCA,
        [Description("UPC-E")]
        UPCE,
        [Description("UPC 2 Digit Ext.")]
        UPC2DigitExt,
        [Description("UPC 5 Digit Ext.")]
        UPC5DigitExt,
        [Description("EAN-13")]
        EAN13,
        [Description("JAN-13")]
        JAN13,
        [Description("EAN-8")]
        EAN8,
        [Description("ITF-14")]
        ITF14,
        [Description("Interleaved 2 of 5")]
        Interleaved2of5,
        [Description("Standard 2 of 5")]
        Standard2of5,
        [Description("Codabar")]
        Codabar,
        [Description("PostNet")]
        PostNet,
        [Description("Bookland/ISBN")]
        BooklandISBN,
        [Description("Code 11")]
        Code11,
        [Description("Code 39")]
        Code39,
        [Description("Code 39 Extended")]
        Code39Extended,
        [Description("Code 39 Mod 43")]
        Code39Mod43,
        [Description("Code 93")]
        Code93,
        [Description("Code 128")]
        Code128,
        [Description("Code 128-A")]
        Code128A,
        [Description("Code 128-B")]
        Code128B,
        [Description("ode 128-C")]
        Code128C,
        [Description("LOGMARS")]
        LOGMARS,
        [Description("MSI")]
        MSI,
        [Description("Telepen")]
        Telepen,
        [Description("FIM")]
        FIM,
        [Description("Pharmacode")]
        Pharmacode
    }
}