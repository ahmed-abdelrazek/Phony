using System.ComponentModel;

namespace DataCore.Data
{
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
