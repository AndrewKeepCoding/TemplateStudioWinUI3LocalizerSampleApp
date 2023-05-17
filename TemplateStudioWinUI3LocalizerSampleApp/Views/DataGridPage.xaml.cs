using Microsoft.UI.Xaml.Controls;

using TemplateStudioWinUI3LocalizerSampleApp.ViewModels;
using WinUI3Localizer;

namespace TemplateStudioWinUI3LocalizerSampleApp.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class DataGridPage : Page
{
    public DataGridViewModel ViewModel
    {
        get;
    }

    public DataGridPage()
    {
        ViewModel = App.GetService<DataGridViewModel>();
        InitializeComponent();

        ILocalizer localizer = WinUI3Localizer.Localizer.Get();
        var currentLanguage = localizer.GetCurrentLanguage();
        localizer.SetLanguage(currentLanguage);
    }
}