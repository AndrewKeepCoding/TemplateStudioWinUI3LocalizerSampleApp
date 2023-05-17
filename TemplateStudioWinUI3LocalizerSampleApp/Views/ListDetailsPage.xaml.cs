using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using TemplateStudioWinUI3LocalizerSampleApp.ViewModels;
using WinUI3Localizer;

namespace TemplateStudioWinUI3LocalizerSampleApp.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();

        ILocalizer localizer = WinUI3Localizer.Localizer.Get();
        var currentLanguage = localizer.GetCurrentLanguage();
        localizer.SetLanguage(currentLanguage);
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}