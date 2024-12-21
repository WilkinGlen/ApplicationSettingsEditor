namespace ApplicationSettings.Components.AppSettings;

using ApplicationSettings.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

public sealed partial class ApplicationSettingEditorDialog
{
    private readonly List<SettingTypes> SettingTypesThatMustHaveParentGuid = [
        SettingTypes.ConnectionString, 
        SettingTypes.ArrayItem, 
        SettingTypes.GroupItem];

    private string? key;
    private string? value;
    private Guid? parentId;

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public SettingTypes SettingType { get; set; }

    [Parameter]
    public ApplicationSetting? ApplicationSetting { get; set; }

    [Parameter]
    public EventCallback<Exception> OnExceptionThrown { get; set; }

    protected override async void OnInitialized()
    {
        this.ApplicationSetting ??= new ();
        this.key = $"{this.ApplicationSetting?.Key}";
        this.value = $"{this.ApplicationSetting?.Value}";
        await this.ValidateArguments();
    }

    private async Task ValidateArguments()
    {
        if (this.SettingTypesThatMustHaveParentGuid.Contains(this.SettingType))
        {
            if (this.ApplicationSetting!.ParentGuid == null || this.ApplicationSetting!.ParentGuid == Guid.Empty)
            {
                await this.OnExceptionThrown.InvokeAsync(
                    new ArgumentException($"ParentGuid cannot be null or empty for a {this.SettingType} application setting"));
                this.MudDialog!.Cancel();
                return;
            }

            this.parentId = Guid.Parse(this.ApplicationSetting!.ParentGuid.ToString()!);
        }
    }

    private void Save()
    {
        var appSetting = new ApplicationSetting()
        {
            Key = this.key,
            Value = this.value,
            SettingId = this.ApplicationSetting!.SettingId,
            ParentGuid = this.ApplicationSetting!.ParentGuid
        };

        this.MudDialog!.Close(DialogResult.Ok(appSetting));
    }

    private void Cancel() => this.MudDialog!.Cancel();
}
