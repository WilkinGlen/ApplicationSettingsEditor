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
    private readonly List<SettingTypes> SettingThatCannotHaveChildren = [
        SettingTypes.Individual,
        SettingTypes.ConnectionString,
        SettingTypes.GroupItem,
        SettingTypes.ArrayItem];
    private string? key;
    private string? value;
    private bool keyIsValid = true;
    private bool valueIsValid = true;
    private MudTextField<string?>? initialFocusItem;

    private string KeyAdornmentText => this.keyIsValid ? " " : "Key required";
    private string ValueAdornmentText => this.valueIsValid ? " " : "Value required";

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public SettingTypes SettingType { get; set; }

    [Parameter]
    public ApplicationSetting? ApplicationSetting { get; set; }

    [Parameter]
    public EventCallback<Exception> OnExceptionThrown { get; set; }

    [Parameter]
    public int DialogWidth { get; set; } = 75;
    private string DialogCssStyle => $"width: {this.DialogWidth}%";

    protected override async void OnInitialized()
    {
        this.ApplicationSetting ??= new();
        this.key = $"{this.ApplicationSetting?.Key}";
        this.value = $"{this.ApplicationSetting?.Value}";
        await this.ValidateArguments();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            if(this.initialFocusItem != null)
            {
                await this.initialFocusItem.FocusAsync();
            }
        }
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
        }

        if (this.SettingThatCannotHaveChildren.Contains(this.SettingType))
        {
            if (this.ApplicationSetting?.Children?.Any() == true)
            {
                await this.OnExceptionThrown.InvokeAsync(
                    new ArgumentException($"{this.SettingType} application settings cannot have children"));
                this.MudDialog!.Cancel();
                return;
            }
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

        if (this.AppSettingIsValid(appSetting))
        {
            this.MudDialog!.Close(DialogResult.Ok(appSetting));
        }
    }

    private bool AppSettingIsValid(ApplicationSetting applicationSetting)
    {
        this.keyIsValid = true;
        this.valueIsValid = true;

        if (this.SettingType is SettingTypes.Individual or SettingTypes.ConnectionString or SettingTypes.GroupItem)
        {
            (this.keyIsValid, this.valueIsValid) = (!string.IsNullOrEmpty(this.key), !string.IsNullOrEmpty(this.value));
        }

        if (this.SettingType is SettingTypes.ArrayItem)
        {
            this.valueIsValid = !string.IsNullOrEmpty(this.value);
        }

        if (this.SettingType is SettingTypes.ConnectionStringsGroup or SettingTypes.Group or SettingTypes.Array)
        {
            this.keyIsValid = !string.IsNullOrEmpty(this.key);
        }

        return this.keyIsValid && this.valueIsValid;
    }

    private void Cancel() => this.MudDialog!.Cancel();
}
