using WinUI3Localizer;

namespace TemplateStudioWinUI3LocalizerSampleApp.Contracts.Services;

public interface ILocalizationService
{
    Task InitializeAsync();

    IEnumerable<string> GetAvailableLanguages();

    string GetCurrentLanguage();

    Task SetLanguageAsync(string language);
}