using Microsoft.UI.Xaml.Controls;

using TemplateStudioWinUI3LocalizerSampleApp.ViewModels;

namespace TemplateStudioWinUI3LocalizerSampleApp.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
