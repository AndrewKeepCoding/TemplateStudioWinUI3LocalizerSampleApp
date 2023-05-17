using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using TemplateStudioWinUI3LocalizerSampleApp.Contracts.ViewModels;
using TemplateStudioWinUI3LocalizerSampleApp.Core.Contracts.Services;
using TemplateStudioWinUI3LocalizerSampleApp.Core.Models;
using TemplateStudioWinUI3LocalizerSampleApp.Helpers;

namespace TemplateStudioWinUI3LocalizerSampleApp.ViewModels;

public class ListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private SampleOrder? _selected;

    public SampleOrder? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public ListDetailsViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            //item.Company = item.Company.Replace("Company", "Company".GetLocalizedString());
            //item.ShipTo = item.ShipTo.Replace("Company", "Company".GetLocalizedString());
            //item.Status = item.Status.GetLocalizedString();
            SampleItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (Selected == null)
        {
            Selected = SampleItems.First();
        }
    }
}