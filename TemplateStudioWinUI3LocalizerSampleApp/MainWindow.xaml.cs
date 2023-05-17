using TemplateStudioWinUI3LocalizerSampleApp.Helpers;

namespace TemplateStudioWinUI3LocalizerSampleApp;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalizedString();
    }
}