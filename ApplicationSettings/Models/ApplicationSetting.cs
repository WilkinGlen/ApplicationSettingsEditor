namespace ApplicationSettings.Models;

public sealed class ApplicationSetting
{
    public Guid SettingId { get; set; }

    public Guid? ParentGuid { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public int? Index { get; set; }

    public IEnumerable<ApplicationSetting>? Children { get; set; }
}
