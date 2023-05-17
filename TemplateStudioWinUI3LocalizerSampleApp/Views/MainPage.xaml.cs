using Microsoft.UI.Xaml.Controls;

using TemplateStudioWinUI3LocalizerSampleApp.ViewModels;

namespace TemplateStudioWinUI3LocalizerSampleApp.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
