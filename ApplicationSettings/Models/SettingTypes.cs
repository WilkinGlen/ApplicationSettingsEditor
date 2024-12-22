namespace ApplicationSettings.Models;

using System.ComponentModel;

public enum SettingTypes
{
    [Description("Not Set")]
    NotSet = 0,

    [Description("Individual")]
    Individual = 1,

    [Description("ConnectionStrings Group")]
    ConnectionStringsGroup = 2,

    [Description("ConnectionString")]
    ConnectionString = 3,

    [Description("Array")]
    Array = 4,

    [Description("Array Item")]
    ArrayItem = 5,

    [Description("Group")]
    Group = 6,

    [Description("Group Item")]
    GroupItem = 7
}
