using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using TemplateStudioWinUI3LocalizerSampleApp.Contracts.ViewModels;
using TemplateStudioWinUI3LocalizerSampleApp.Core.Contracts.Services;
using TemplateStudioWinUI3LocalizerSampleApp.Core.Models;

namespace TemplateStudioWinUI3LocalizerSampleApp.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public DataGridViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
