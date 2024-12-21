namespace ApplicationSettings.Components.Pages;

using ApplicationSettings.Components.AppSettings;
using ApplicationSettings.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

public sealed partial class Home
{
    [Inject]
    public IDialogService? DialogService { get; set; }

    [Inject]
    public ISnackbar? Snackbar { get; set; }

    //ApplicationSetting ApplicationSetting = new() { SettingId = Guid.NewGuid(), ParentGuid = Guid.NewGuid() };
    private ApplicationSetting ApplicationSetting = new();
    private readonly SettingTypes settingType = SettingTypes.ConnectionStringsGroup;

    protected override void OnInitialized() => this.Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;

    private async Task EditClickedHandler()
    {
        var parameters = new DialogParameters<ApplicationSettingEditorDialog>
        {
            { x => x.ApplicationSetting, this.ApplicationSetting },
            { x => x.SettingType, this.settingType },
            { x => x.OnExceptionThrown, new EventCallbackFactory().Create(this, new Action<Exception>(this.DialogExceptionThrown)) },
            { x => x.DialogWidth, 50}
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large
        };

        var dialog = await this.DialogService!.ShowAsync<ApplicationSettingEditorDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;
        if (result?.Canceled == false)
        {
            this.ApplicationSetting = (ApplicationSetting)result.Data!;
        }
    }

    private void DialogExceptionThrown(Exception ex) =>
        this.Snackbar!.Add($"Error in dialog: {ex.Message}", Severity.Error);
}
