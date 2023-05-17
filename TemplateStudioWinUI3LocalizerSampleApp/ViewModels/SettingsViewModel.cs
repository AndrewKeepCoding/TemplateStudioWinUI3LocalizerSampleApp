using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using TemplateStudioWinUI3LocalizerSampleApp.Contracts.Services;
using TemplateStudioWinUI3LocalizerSampleApp.Helpers;
using TemplateStudioWinUI3LocalizerSampleApp.Models;
using TemplateStudioWinUI3LocalizerSampleApp.Services;
using Windows.ApplicationModel;

namespace TemplateStudioWinUI3LocalizerSampleApp.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    private readonly ILocalizationService _localizationService;

    [ObservableProperty]
    private LanguageItem _selectedLanguage;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, ILocalizationService localizationService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        _localizationService = localizationService;
        AvailableLanguages = _localizationService.GetAvailableLanguages()
            .Select(x => new LanguageItem(Language: x, UidKey: $"{"Settings_Language"}_{x}"))
            .ToList();

        _selectedLanguage = AvailableLanguages.First(x => x.Language == _localizationService.GetCurrentLanguage());
    }

    async partial void OnSelectedLanguageChanged(LanguageItem value)
    {
        await _localizationService.SetLanguageAsync(value.Language);
    }

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public List<LanguageItem> AvailableLanguages
    {
        get; set;
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalizedString()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}