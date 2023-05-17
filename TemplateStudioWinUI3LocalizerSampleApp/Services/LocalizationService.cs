using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using TemplateStudioWinUI3LocalizerSampleApp.Contracts.Services;
using Windows.Storage;
using WinUI3Localizer;

namespace TemplateStudioWinUI3LocalizerSampleApp.Services;

public class LocalizationService : ILocalizationService
{
    private const string SettingsKey = "AppLocalizationLanguage";

    private readonly ILocalSettingsService _localSettingsService;

    public LocalizationService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    private ILocalizer Localizer { get; set; } = WinUI3Localizer.Localizer.Get();

    public async Task InitializeAsync()
    {
        await InitializeLocalizer();

        if (await LoadLanguageFromSettingsAsync() is string language)
        {
            await Localizer.SetLanguage(language);
        }
    }

    public async Task SetLanguageAsync(string language)
    {
        await Localizer.SetLanguage(language);
        await SaveLanguageInSettingsAsync(language);
    }

    public IEnumerable<string> GetAvailableLanguages() => Localizer.GetAvailableLanguages();

    public string GetCurrentLanguage() => Localizer.GetCurrentLanguage();

    private async Task InitializeLocalizer()
    {
        // Initialize a "Strings" folder in the "LocalFolder" for the packaged app.
        var localFolder = ApplicationData.Current.LocalFolder;
        var stringsFolder = await localFolder.CreateFolderAsync("Strings", CreationCollisionOption.OpenIfExists);

        // Create string resources file from app resources if doesn't exists.
        await MakeSureStringResourceFileExists(stringsFolder, "en-us", "Resources.resw");
        await MakeSureStringResourceFileExists(stringsFolder, "es-es", "Resources.resw");

        Localizer = await new LocalizerBuilder()
            .AddStringResourcesFolderForLanguageDictionaries(stringsFolder.Path)
            .SetOptions(options =>
            {
                options.UseUidWhenLocalizedStringNotFound = true;
                options.DefaultLanguage = "en-us";
            })
            .AddLocalizationAction(
                new LocalizationActions.ActionItem(
                    typeof(ListDetailsView),
                    args =>
                    {
                        if (args.DependencyObject is ListDetailsView listDetailsView)
                        {
                            listDetailsView.Loaded += (sender, e) =>
                            {
                                var textBlocks = listDetailsView.FindDescendants().OfType<TextBlock>().ToList();

                                foreach (var textBlock in textBlocks)
                                {
                                    if (textBlock.Text.GetLocalizedString() is string localizedString)
                                    {
                                        textBlock.Text = localizedString;
                                    }
                                }
                            };
                        }
                    }))
           .AddLocalizationAction(
                new LocalizationActions.ActionItem(
                    typeof(DataGrid),
                    args =>
                    {
                        if (args.DependencyObject is DataGrid dataGrid)
                        {
                            dataGrid.Loaded += (s, e) =>
                            {
                                if (s is DataGrid dataGrid)
                                {
                                    foreach (var column in dataGrid.Columns)
                                    {
                                        if (column.Header is string header)
                                        {
                                            column.Header = header.GetLocalizedString();
                                        }
                                    }
                                }
                            };
                        }
                    }))
            .Build();
    }

    private static async Task MakeSureStringResourceFileExists(StorageFolder stringsFolder, string language, string resourceFileName)
    {
        var languageFolder = await stringsFolder.CreateFolderAsync(
            desiredName: language,
            CreationCollisionOption.OpenIfExists);

        var appResourceFilePath = Path.Combine(stringsFolder.Name, language, resourceFileName);
        var appResourceFile = await LoadStringResourcesFileFromAppResource(appResourceFilePath);

        var localResourceFile = await languageFolder.TryGetItemAsync(resourceFileName);

        if (localResourceFile is null ||
            (await GetModifiedDate(appResourceFile)) > (await GetModifiedDate(localResourceFile)))
        {
            _ = await appResourceFile.CopyAsync(
                destinationFolder: languageFolder,
                desiredNewName: appResourceFile.Name,
                option: NameCollisionOption.ReplaceExisting);
        }
    }

    private static async Task<StorageFile> LoadStringResourcesFileFromAppResource(string filePath)
    {
        Uri resourcesFileUri = new($"ms-appx:///{filePath}");
        return await StorageFile.GetFileFromApplicationUriAsync(resourcesFileUri);
    }

    private static async Task<DateTimeOffset> GetModifiedDate(IStorageItem file)
    {
        return (await file.GetBasicPropertiesAsync()).DateModified;
    }

    private async Task<string?> LoadLanguageFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
    }

    private async Task SaveLanguageInSettingsAsync(string language)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, language);
    }
}