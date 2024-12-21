namespace ApplicationSettings.Models;

using System.ComponentModel;

public enum SettingTypes
{
    [Description("Individual")]
    Individual = 0,

    [Description("ConnectionStrings Group")]
    ConnectionStringsGroup = 1,

    [Description("ConnectionString")]
    ConnectionString = 2,

    [Description("Array")]
    Array = 3,

    [Description("Array Item")]
    ArrayItem = 4,

    [Description("Group")]
    Group = 5,

    [Description("Group Item")]
    GroupItem = 6
}
