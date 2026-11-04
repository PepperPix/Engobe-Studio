namespace Engobe.Studio.Views;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

public partial class WelcomeView : UserControl
{
    public WelcomeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
